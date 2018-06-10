using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using WarBender.GameData;

namespace WarBender.Modules {
    public class PartyTemplateDefinition : EntityDefinition, IHasId, IHasName {
        public override Type EntityType => typeof(Faction);
        public string Id { get; }
        public string Name { get; }

        public PartyTemplateDefinition(int index, string id, string name)
            : base(index) {
            Id = id;
            Name = name;
        }
    }

    public class PartyTemplateDefinitions : EntityDefinitions<PartyTemplateDefinition> {
        public PartyTemplateDefinitions(IEnumerable<PartyTemplateDefinition> definitions) 
            : base(definitions) {
        }

        public PartyTemplateDefinitions(TextReader reader)
            : this(ReadDefinitions(reader)) {
        }

        private static IEnumerable<PartyTemplateDefinition> ReadDefinitions(LineReader reader) {
            Trace.WriteLine($"Loading {reader.FileName}", nameof(PartyTemplateDefinitions));

            reader.Expected("partytemplatesfile version 1");
            var count = reader.ReadFields<int>(allowMore: false);
            for (int i = 0; i < count; ++i) {
                var (id, name) = reader.ReadFields<string, string>();
                yield return new PartyTemplateDefinition(i, id, name);
            }
        }
    }
}
