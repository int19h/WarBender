using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using WarBender.GameData;

namespace WarBender.Modules {
    public class MapIconDefinition : EntityDefinition, IHasId {
        public override Type EntityType => typeof(MapIconDefinition);
        public string Id { get; }

        public MapIconDefinition(int index, string id)
            : base(index) {
            Id = id;
        }
    }

    public class MapIconDefinitions : EntityDefinitions<MapIconDefinition> {
        public MapIconDefinitions(IEnumerable<MapIconDefinition> definitions)
            : base(definitions) {
        }

        public MapIconDefinitions(TextReader reader)
            : this(ReadDefinitions(reader)) {
        }

        private static IEnumerable<MapIconDefinition> ReadDefinitions(LineReader reader) {
            Trace.WriteLine($"Loading {reader.FileName}", nameof(MapIconDefinitions));

            reader.Expected("map_icons_file version 1");
            var count = reader.ReadFields<int>(allowMore: false);
            for (int i = 0; i < count; ++i) {
                var id = reader.ReadFields<string>();
                var s = reader.ReadLine();
                reader.SkipLine(string.IsNullOrEmpty(s) ? 1 : 2);
                yield return new MapIconDefinition(i, id);
            }
        }
    }
}
