using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace WarBender.Modules {
    public class TableauMaterialDefinition : EntityDefinition, IHasId {
        public override Type EntityType => typeof(TableauMaterialDefinition);
        public string Id { get; }

        public TableauMaterialDefinition(int index, string id)
            : base(index) {
            Id = id;
        }
    }

    public class TableauMaterialDefinitions : EntityDefinitions<TableauMaterialDefinition> {
        public TableauMaterialDefinitions(IEnumerable<TableauMaterialDefinition> definitions) 
            : base(definitions) {
        }

        public TableauMaterialDefinitions(TextReader reader)
            : this(ReadDefinitions(reader)) {
        }

        private static IEnumerable<TableauMaterialDefinition> ReadDefinitions(LineReader reader) {
            Trace.WriteLine($"Loading {reader.FileName}", nameof(TableauMaterialDefinitions));

            var count = reader.ReadFields<int>(allowMore: false);
            for (int i = 0; i < count; ++i) {
                var id = reader.ReadFields<string>();
                yield return new TableauMaterialDefinition(i, id);
            }
        }
    }
}
