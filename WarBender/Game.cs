using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using WarBender.Modules;
using WarBender.GameData;

namespace WarBender {
    public class Game {
        public string FileName { get; }

        public Module Module { get; }

        public SavedGame Data { get; }

        private readonly EntityContainer _entities = new EntityContainer();

        public IEntityContainer Entities => _entities;

        private Game(Module module, string fileName) {
            FileName = fileName;
            Module = module;
            Data = SavedGame.Create();
            Data.Game = this;

            _entities = new EntityContainer(Module.EntityDefinitions);
            _entities.AddEntities(() => Data.factions);
            _entities.AddEntities(() => Data.info_pages);
            _entities.AddEntities(() => Data.item_kinds);
            _entities.AddEntities(() => Data.parties);
            _entities.AddEntities(() => Data.party_templates);
            _entities.AddEntities(() => Data.quests);
            _entities.AddEntities(() => Data.scenes);
            _entities.AddEntities(() => Data.troops);
        }

        public static Game Load(BinaryReader reader, Module module) {
            var fileName = reader.FileName();
            Trace.WriteLine($"Loading from {fileName}", nameof(Game));

            var sw = Stopwatch.StartNew();
            var game = new Game(module, fileName);
            game.Data.ReadFrom(reader);
            sw.Stop();

            if (reader.Read(new byte[1], 0, 1) > 0) {
                throw new InvalidDataException("Leftover unparsed data");
            }

            Trace.WriteLine($"Loaded in {sw.Elapsed}", nameof(Game));
            return game;
        }

        public static Game Load(Stream stream, Module module) {
            using (var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true)) {
                return Load(reader, module);
            }
        }

        public static Game Load(string fileName, Module module) { 
            using (var stream = File.OpenRead(fileName)) {
                return Load(stream, module);
            }
        }

        public void Save(BinaryWriter writer) {
            Trace.WriteLine($"Saving to {writer.FileName()}", nameof(Game));

            var sw = Stopwatch.StartNew();
            Data.WriteTo(writer);
            sw.Stop();

            Trace.WriteLine($"Saved in {sw.Elapsed}", nameof(Game));
        }

        public void Save(Stream stream) {
            using (var writer = new BinaryWriter(stream)) {
                Save(writer);
            }
        }

        public void Save(string fileName) {
            using(var stream = File.OpenWrite(fileName)) {
                Save(stream);
            }
        }

        public int Version => Data.header.game_version;

        internal bool MatchVersions(int l1, int u1) =>
            l1 <= Version && Version <= u1;

        internal bool MatchVersions(int l1, int u1, int l2, int u2) =>
            (l1 <= Version && Version <= u1) || (l2 <= Version && Version <= u2);

        public Snapshot CreateSnapshot() => new Snapshot(this);

        public sealed class Snapshot : IDisposable {
            private MemoryStream _stream = new MemoryStream();

            public Game Game { get; }

            public Snapshot(Game game) {
                Game = game;

                Trace.WriteLine($"Creating snapshot", nameof(Game));

                var sw = Stopwatch.StartNew();
                using (var writer = new BinaryWriter(_stream, Encoding.Default, true)) {
                    game.Data.WriteTo(writer);
                }
                sw.Stop();

                Trace.WriteLine($"Created in {sw.Elapsed}", nameof(Game));
            }

            public void Restore() {
                if (_stream == null) {
                    throw new ObjectDisposedException(GetType().FullName);
                }

                Trace.WriteLine($"Restoring from snapshot", nameof(Game));

                _stream.Position = 0;
                using (var reader = new BinaryReader(_stream, Encoding.Default, true)) {
                    Game.Data.ReadFrom(reader);
                }
            }

            public void Dispose() {
                Trace.WriteLine($"Discarding snapshot", nameof(Game));

                _stream?.Dispose();
                _stream = null;
            }
        }
    }
}
