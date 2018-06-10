using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace WarBender.Modules {
    public abstract class EntityDefinition : IEntity {
        public int Index { get; }

        [Browsable(false)]
        public abstract Type EntityType { get; }

        public EntityDefinition(int index) {
            Index = index;
        }
    }

    public abstract class EntityDefinitions<T> : ReadOnlyCollection<T>
        where T: EntityDefinition {

        public EntityDefinitions(IEnumerable<T> definitions)
            : base(definitions.ToArray()) {
        }
    }
}
