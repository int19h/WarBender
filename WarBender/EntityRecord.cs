using System.ComponentModel;
using System.IO;
using WarBender.Modules;

namespace WarBender {
    public abstract class EntityRecord<T, TDefinition> : Record<T>, IEntity
        where T : EntityRecord<T, TDefinition>
        where TDefinition : EntityDefinition {

        [Computed]
        [ParenthesizePropertyName(true)]
        public TDefinition Definition {
            get {
                var defs = this.TryGame()?.Module.GetEntityDefinitions<TDefinition>();
                if (defs == null || Index >= defs.Count) {
                    return null;
                }
                return defs[Index];
            }
        }
    }

    public abstract class OptionalEntityRecord<T, TDefinition> : EntityRecord<T, TDefinition>, IOptionalRecord
        where T : OptionalEntityRecord<T, TDefinition>
        where TDefinition : EntityDefinition {

        [Computed]
        [ParenthesizePropertyName(true)]
        public bool IsPresent { get; set; }

        public override void ReadFrom(BinaryReader reader) {
            IsPresent = reader.ReadInt32() != 0;
            if (IsPresent) {
                base.ReadFrom(reader);
            }
        }

        public override void WriteTo(BinaryWriter writer) {
            writer.Write(IsPresent ? 1 : 0);
            if (IsPresent) {
                base.WriteTo(writer);
            }
        }
    }
}
