using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace WarBender.Modules {
    public class MenuDefinition : EntityDefinition, IHasId {
        public override Type EntityType => typeof(MenuDefinition);
        public string Id { get; }

        public MenuDefinition(int index, string id)
            : base(index) {
            Id = id;
        }
    }

    public class MenuDefinitions : EntityDefinitions<MenuDefinition> {
        public MenuDefinitions(IEnumerable<MenuDefinition> definitions) 
            : base(definitions) {
        }

        public MenuDefinitions(TextReader reader)
            : this(ReadDefinitions(reader)) {
        }

        private static IEnumerable<MenuDefinition> ReadDefinitions(LineReader reader) {
            Trace.WriteLine($"Loading {reader.FileName}", nameof(MenuDefinitions));

            reader.Expected("menusfile version 1");
            var count = reader.ReadFields<int>(allowMore: false);
            for (int i = 0; i < count; ++i) {
                var id = reader.ReadFields<string>();
                reader.SkipLine();
                yield return new MenuDefinition(i, id);
            }
        }
    }
}
