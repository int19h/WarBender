using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace WarBender.Modules {
    public class ParticleSystemDefinition : EntityDefinition, IHasId {
        public override Type EntityType => typeof(ParticleSystemDefinition);
        public string Id { get; }

        public ParticleSystemDefinition(int index, string id)
            : base(index) {
            Id = id;
        }
    }

    public class ParticleSystemDefinitions : EntityDefinitions<ParticleSystemDefinition> {
        public ParticleSystemDefinitions(IEnumerable<ParticleSystemDefinition> definitions) 
            : base(definitions) {
        }

        public ParticleSystemDefinitions(TextReader reader)
            : this(ReadDefinitions(reader)) {
        }

        private static IEnumerable<ParticleSystemDefinition> ReadDefinitions(LineReader reader) {
            Trace.WriteLine($"Loading {reader.FileName}", nameof(ParticleSystemDefinitions));

            reader.Expected("particle_systemsfile version 1");
            var count = reader.ReadFields<int>(allowMore: false);
            for (int i = 0; i < count; ++i) {
                var id = reader.ReadFields<string>();
                reader.SkipLine(7);
                yield return new ParticleSystemDefinition(i, id);
            }
        }
    }
}
