using System;
using System.Diagnostics;
using System.IO;

namespace WarBender {
    public interface IDataObjectChild {
        IDataObject Parent { get; }

        IDataObjectChild WithParent(IDataObject parent, int index = -1);
    }

    public interface IDataObject : IDataObjectChild {
        void ReadFrom(BinaryReader reader);

        void WriteTo(BinaryWriter writer);
    }

    public interface IRootDataObject : IDataObject {
        Game Game { get; }
    }

    public interface IHasId {
        string Id { get; }
    }

    public interface IHasName {
        string Name { get; }
    }

    public static class DataObjectExtensions {
        public static bool IsInCollection(this IDataObjectChild obj) =>
            obj.Parent is ICollection;

        public static T Ancestor<T>(this IDataObjectChild child)
            where T : class {

            for (var obj = child.Parent; obj != null; obj = obj.Parent) {
                if (obj is T x) {
                    return x;
                }
            }
            return null;
        }

        public static Game TryGame(this IDataObjectChild obj) {
            if (obj is IRootDataObject root) {
                return root.Game;
            }

            root = obj.Ancestor<IRootDataObject>();
            return root?.Game;
        }

        public static Game Game(this IDataObjectChild obj) {
            var game = obj.TryGame();
            if (game == null) {
                throw new InvalidOperationException("Data object is not part of the object tree");
            }
            return game;
        }

        public static void Validate(this IDataObject obj) {
            var name = obj is IRecord record ? record.Type.Name : obj.GetType().Name;
            Trace.WriteLine($"Validating", name);
            var sw = Stopwatch.StartNew();
            obj.WriteTo(BinaryWriter.Null);
            sw.Stop();
            Trace.WriteLine($"Validated in {sw.Elapsed}", name);
        }
    }

    public interface IDataObjectFactory<T> {
        T Create();
    }

    public partial class DataObjectFactory {
        public static readonly DataObjectFactory Instance = new DataObjectFactory();

        public static IDataObjectFactory<T> TryGet<T>() =>
            Instance as IDataObjectFactory<T>;
    }
}
