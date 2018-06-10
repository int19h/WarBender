using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using WarBender.GameData;

namespace WarBender.Modules {
    public class ItemKindDefinition : EntityDefinition, IHasId, IHasName {
        public override Type EntityType => typeof(ItemKindDefinition);
        public string Id { get; }
        public string Name { get; }

        public ItemKindDefinition(int index, string id, string name)
            : base(index) {
            Id = id;
            Name = name;
        }
    }

    public class ItemKindDefinitions : EntityDefinitions<ItemKindDefinition> {
        public ItemKindDefinitions(IEnumerable<ItemKindDefinition> definitions) 
            : base(definitions) {
        }

        public ItemKindDefinitions(TextReader reader)
            : this(ReadDefinitions(reader)) {
        }

        private static IEnumerable<ItemKindDefinition> ReadDefinitions(LineReader reader) {
            Trace.WriteLine($"Loading {reader.FileName}", nameof(ItemKindDefinitions));

            reader.Expected("itemsfile version 3");
            var count = reader.ReadFields<int>(allowMore: false);
            for (int i = 0; i < count; ++i) {
                var (id, name) = reader.ReadFields<string, string>();
                var n = reader.ReadFields<int>(allowMore: false);
                if (n != 0) {
                    reader.SkipLine();
                }

                n = reader.ReadFields<int>(allowMore: false);
                reader.SkipLine(n);

                reader.Expected("");
                yield return new ItemKindDefinition(i, id, name);
            }
        }
    }
}
