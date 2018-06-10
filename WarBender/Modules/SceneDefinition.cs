using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using WarBender.GameData;

namespace WarBender.Modules {
    public class SceneDefinition : EntityDefinition, IHasId {
        public override Type EntityType => typeof(Scene);
        public string Id { get; }

        public SceneDefinition(int index, string id)
            : base(index) {
            Id = id;
        }
    }

    public class SceneDefinitions : EntityDefinitions<SceneDefinition> {
        public SceneDefinitions(IEnumerable<SceneDefinition> definitions)
            : base(definitions) {
        }

        public SceneDefinitions(TextReader reader)
            : this(ReadDefinitions(reader)) {
        }

        private static IEnumerable<SceneDefinition> ReadDefinitions(LineReader reader) {
            Trace.WriteLine($"Loading {reader.FileName}", nameof(SceneDefinitions));

            reader.Expected("scenesfile version 1");
            var count = reader.ReadFields<int>(allowMore: false);
            for (int i = 0; i < count; ++i) {
                var id = reader.ReadFields<string>();
                reader.SkipLine(3);
                yield return new SceneDefinition(i, id);
            }
        }
    }
}
