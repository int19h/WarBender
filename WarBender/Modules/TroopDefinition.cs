using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using WarBender.GameData;

namespace WarBender.Modules {
    public class TroopDefinition : EntityDefinition, IHasId, IHasName {
        public override Type EntityType => typeof(Troop);
        public string Id { get; }
        public string Name { get; }
        public string NamePlural { get; }
        public TroopFlags Flags { get; }

        public TroopDefinition(int index, string id, string name, string namePlural, TroopFlags flags)
            : base(index) {
            Id = id;
            Name = name;
            NamePlural = namePlural;
            Flags = flags;
        }
    }

    public class TroopDefinitions : EntityDefinitions<TroopDefinition> {
        public TroopDefinitions(IEnumerable<TroopDefinition> definitions) 
            : base(definitions) {
        }

        public TroopDefinitions(TextReader reader)
            : this(ReadDefinitions(reader)) {
        }

        private static IEnumerable<TroopDefinition> ReadDefinitions(LineReader reader) {
            Trace.WriteLine($"Loading {reader.FileName}", nameof(TroopDefinitions));

            reader.Expected("troopsfile version 2");
            var count = reader.ReadFields<int>(allowMore: false);
            for (int i = 0; i < count; ++i) {
                if (i != 0) {
                    reader.Expected("");
                }
                var (id, name, plural, _, flags) = reader.ReadFields<string, string, string, object, TroopFlags>();
                reader.SkipLine(5);
                yield return new TroopDefinition(i, id, name, plural, flags);
            }
        }
    }
}
