using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using WarBender.GameData;

namespace WarBender.Modules {
    public class PartyDefinition : EntityDefinition, IHasId, IHasName {
        public override Type EntityType => typeof(Party);
        public string Id { get; }
        public string Name { get; }

        public PartyDefinition(int index, string id, string name)
            : base(index) {
            Id = id;
            Name = name;
        }
    }

    public class PartyDefinitions : EntityDefinitions<PartyDefinition> {
        public PartyDefinitions(IEnumerable<PartyDefinition> definitions) 
            : base(definitions) {
        }

        public PartyDefinitions(TextReader reader)
            : this(ReadDefinitions(reader)) {
        }

        private static IEnumerable<PartyDefinition> ReadDefinitions(LineReader reader) {
            Trace.WriteLine($"Loading {reader.FileName}", nameof(PartyDefinitions));

            reader.Expected("partiesfile version 1");
            var (count, _) = reader.ReadFields<int, int>(allowMore: false);
            for (int i = 0; i < count; ++i) {
                var (_, _, _, id, name) = reader.ReadFields<int, int, int, string, string>();
                reader.SkipLine();
                yield return new PartyDefinition(i, id, name);
            }
        }
    }
}
