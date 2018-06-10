using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using WarBender.GameData;

namespace WarBender.Modules {
    public class QuestDefinition : EntityDefinition, IHasId {
        public override Type EntityType => typeof(Quest);
        public string Id { get; }
        public string Title { get; }

        public QuestDefinition(int index, string id, string title)
            : base(index) {
            Id = id;
            Title = title;
        }
    }

    public class QuestDefinitions : EntityDefinitions<QuestDefinition> {
        public QuestDefinitions(IEnumerable<QuestDefinition> definitions)
            : base(definitions) {
        }

        public QuestDefinitions(TextReader reader)
            : this(ReadDefinitions(reader)) {
        }

        private static IEnumerable<QuestDefinition> ReadDefinitions(LineReader reader) {
            Trace.WriteLine($"Loading {reader.FileName}", nameof(SceneDefinitions));

            reader.Expected("questsfile version 1");
            var count = reader.ReadFields<int>(allowMore: false);
            for (int i = 0; i < count; ++i) {
                var (id, title) = reader.ReadFields<string, string>();
                yield return new QuestDefinition(i, id, title);
            }
        }
    }
}
