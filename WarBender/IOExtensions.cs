using System.IO;

namespace WarBender {
    internal static class IOExtensions {
        public static string FileName(this Stream stream) {
            switch (stream) {
                case FileStream fstream:
                    return fstream.Name;
                case IHasName hasName:
                    return hasName.Name;
                default:
                    return stream?.GetType().Name;
            }
        }

        public static string FileName(this TextReader reader) =>
            FileName((reader as StreamReader)?.BaseStream) ?? reader?.GetType().Name;

        public static string FileName(this BinaryReader reader) =>
            FileName(reader?.BaseStream) ?? reader?.GetType().Name;

        public static string FileName(this BinaryWriter writer) =>
            FileName(writer?.BaseStream) ?? writer?.GetType().Name;
    }
}
