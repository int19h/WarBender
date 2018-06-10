using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace WarBender {
    public interface IEntityReference : IDataObjectChild {
        Type EntityType { get; }

        IEntity Entity { get; }

        IList Entities { get; }

        int Index { get; }

        IEntityReference WithIndex(int value);
    }

    public struct EntityReference<T> : IEntityReference, IEquatable<EntityReference<T>>
        where T : class, IEntity {

        [Browsable(false)]
        public Type EntityType => typeof(T);

        [Browsable(false)]
        public IDataObject Parent { get; private set; }

        public int Index { get; private set; }

        internal EntityReference(IDataObject parent, int index) {
            Parent = parent;
            Index = index;
        }

        internal EntityReference(IDataObject parent, T entity)
            : this(parent, entity?.Index ?? -1) {
        }

        internal EntityReference(IDataObject parent, EntityReference<T> other)
            : this(parent, other.Index) {
        }

        public EntityReference(int index)
            : this(null, index) {
        }

        public EntityReference(T entity)
            : this(null, entity) {
        }

        public EntityReference(EntityReference<T> other)
            : this(null, other) {
        }

        public static implicit operator int(EntityReference<T> eref) => eref.Index;

        public static implicit operator T(EntityReference<T> eref) => eref.Entity;

        public static implicit operator EntityReference<T>(int index) => new EntityReference<T>(index);

        public static implicit operator EntityReference<T>(T entity) => new EntityReference<T>(entity);

        public bool Equals(EntityReference<T> other) => Index == other.Index;

        public override bool Equals(object obj) =>
            obj is EntityReference<T> other ? Equals(other) : false;

        public override int GetHashCode() => Index.GetHashCode();

        IDataObjectChild IDataObjectChild.WithParent(IDataObject parent, int index) {
            Parent = parent;
            return this;
        }

        IEntityReference IEntityReference.WithIndex(int index) {
            Index = index;
            return this;
        }

        public T Entity {
            get {
                var entities = Entities;
                if (Index < 0 || Index >= entities.Count) {
                    return null;
                }
                return entities[Index];
            }
        }

        private IReadOnlyList<T> Entities => this.Game().Entities.GetEntities<T>();

        IEntity IEntityReference.Entity => Entity;

        IList IEntityReference.Entities => (IList)Entities;
    }

    internal class EntityReferenceSerializer<T> : IValueSerializer<EntityReference<T>>
        where T : class, IEntity {

        public static readonly EntityReferenceSerializer<T> Instance = new EntityReferenceSerializer<T>();

        public EntityReference<T> Read(BinaryReader reader) => reader.ReadInt32();

        public void Write(BinaryWriter writer, EntityReference<T> value) => writer.Write(value.Index);
    }
}
