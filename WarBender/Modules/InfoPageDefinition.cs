using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace WarBender.Modules {
    public class InfoPageDefinition : EntityDefinition, IHasId, IHasName {
        public override Type EntityType => typeof(InfoPageDefinition);
        public string Id { get; }
        public string Name { get; }

        public InfoPageDefinition(int index, string id, string name)
            : base(index) {
            Id = id;
            Name = name;
        }
    }

    public class InfoPageDefinitions : EntityDefinitions<InfoPageDefinition> {
        public InfoPageDefinitions(IEnumerable<InfoPageDefinition> definitions) 
            : base(definitions) {
        }

        public InfoPageDefinitions(TextReader reader)
            : this(ReadDefinitions(reader)) {
        }

        private static IEnumerable<InfoPageDefinition> ReadDefinitions(LineReader reader) {
            Trace.WriteLine($"Loading {reader.FileName}", nameof(InfoPageDefinitions));

            reader.Expected("infopagesfile version 1");
            var count = reader.ReadFields<int>(allowMore: false);
            for (int i = 0; i < count; ++i) {
                var (id, name) = reader.ReadFields<string, string>();
                yield return new InfoPageDefinition(i, id, name);
            }
        }
    }
}
