using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace WarBender.Modules {
    public class MeshDefinition : EntityDefinition, IHasId {
        public override Type EntityType => typeof(MeshDefinition);
        public string Id { get; }

        public MeshDefinition(int index, string id)
            : base(index) {
            Id = id;
        }
    }

    public class MeshDefinitions : EntityDefinitions<MeshDefinition> {
        public MeshDefinitions(IEnumerable<MeshDefinition> definitions) 
            : base(definitions) {
        }

        public MeshDefinitions(TextReader reader)
            : this(ReadDefinitions(reader)) {
        }

        private static IEnumerable<MeshDefinition> ReadDefinitions(LineReader reader) {
            Trace.WriteLine($"Loading {reader.FileName}", nameof(MeshDefinitions));

            var count = reader.ReadFields<int>(allowMore: false);
            for (int i = 0; i < count; ++i) {
                var id = reader.ReadFields<string>();
                yield return new MeshDefinition(i, id);
            }
        }
    }
}
