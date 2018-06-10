using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using WarBender.GameData;

namespace WarBender.Modules {
    public class VariableDefinition : EntityDefinition, IHasId {
        public override Type EntityType => typeof(VariableDefinition);
        public string Id { get; }

        public VariableDefinition(int index, string id)
            : base(index) {
            Id = id;
        }
    }

    public class VariableDefinitions : EntityDefinitions<VariableDefinition> {
        public VariableDefinitions(IEnumerable<VariableDefinition> definitions)
            : base(definitions) {
        }

        public VariableDefinitions(TextReader reader)
            : this(ReadDefinitions(reader)) {
        }

        private static IEnumerable<VariableDefinition> ReadDefinitions(LineReader reader) {
            Trace.WriteLine($"Loading {reader.FileName}", nameof(VariableDefinitions));
            for (int i = 0; ; ++i) {
                var id = reader.ReadLine();
                if (id == null) {
                    break;
                }
                yield return new VariableDefinition(i, id);
            }
        }
    }
}
