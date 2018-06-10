using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace WarBender.Modules {
    public class StringDefinition : EntityDefinition, IHasId, IHasName {
        public override Type EntityType => typeof(StringDefinition);
        public string Id { get; }
        public string Text { get; }

        string IHasName.Name => Text;

        public StringDefinition(int index, string id, string text)
            : base(index) {
            Id = id;
            Text = text;
        }
    }

    public class StringDefinitions : EntityDefinitions<StringDefinition> {
        public StringDefinitions(IEnumerable<StringDefinition> definitions) 
            : base(definitions) {
        }

        public StringDefinitions(TextReader reader)
            : this(ReadDefinitions(reader)) {
        }

        private static IEnumerable<StringDefinition> ReadDefinitions(LineReader reader) {
            Trace.WriteLine($"Loading {reader.FileName}", nameof(StringDefinitions));

            reader.Expected("stringsfile version 1");
            var count = reader.ReadFields<int>(allowMore: false);
            for (int i = 0; i < count; ++i) {
                var (id, text) = reader.ReadFields<string, string>();
                yield return new StringDefinition(i, id, text);
            }
        }
    }
}
