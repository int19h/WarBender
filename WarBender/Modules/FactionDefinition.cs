using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using WarBender.GameData;

namespace WarBender.Modules {
    public class FactionDefinition : EntityDefinition, IHasId, IHasName {
        public override Type EntityType => typeof(Faction);
        public string Id { get; }
        public string Name { get; }

        public FactionDefinition(int index, string id, string name)
            : base(index) {
            Id = id;
            Name = name;
        }
    }

    public class FactionDefinitions : EntityDefinitions<FactionDefinition> {
        public FactionDefinitions(IEnumerable<FactionDefinition> definitions) 
            : base(definitions) {
        }

        public FactionDefinitions(TextReader reader)
            : this(ReadDefinitions(reader)) {
        }

        private static IEnumerable<FactionDefinition> ReadDefinitions(LineReader reader) {
            Trace.WriteLine($"Loading {reader.FileName}", nameof(FactionDefinitions));

            reader.Expected("factionsfile version 1");
            var count = reader.ReadFields<int>(allowMore: false);
            for (int i = 0; i < count; ++i) {
                string id, name;
                if (i == 0) {
                    (id, name) = reader.ReadFields<string, string>();
                } else {
                    (_, id, name) = reader.ReadFields<object, string, string>();
                }
                reader.SkipLine();
                yield return new FactionDefinition(i, id, name);
            }
        }
    }
}
