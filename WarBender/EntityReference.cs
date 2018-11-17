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

    public struct EntityReference<TEntity, TIndex> : IEntityReference, IEquatable<EntityReference<TEntity, TIndex>>
        where TEntity : class, IEntity
        where TIndex : struct, IConvertible, IEquatable<TIndex> {

        [Browsable(false)]
        public Type EntityType => typeof(TEntity);

        [Browsable(false)]
        public IDataObject Parent { get; private set; }

        public int Index { get; private set; }

        internal EntityReference(IDataObject parent, int index) {
            Parent = parent;
            Index = index;
        }

        internal EntityReference(IDataObject parent, TEntity entity)
            : this(parent, entity?.Index ?? -1) {
        }

        internal EntityReference(IDataObject parent, EntityReference<TEntity, TIndex> other)
            : this(parent, other.Index) {
        }

        public EntityReference(int index)
            : this(null, index) {
        }

        public EntityReference(TEntity entity)
            : this(null, entity) {
        }

        public EntityReference(EntityReference<TEntity, TIndex> other)
            : this(null, other) {
        }

        public static implicit operator int(EntityReference<TEntity, TIndex> eref) => eref.Index;

        public static implicit operator TEntity(EntityReference<TEntity, TIndex> eref) => eref.Entity;

        public static implicit operator EntityReference<TEntity, TIndex>(int index) => new EntityReference<TEntity, TIndex>(index);

        public static implicit operator EntityReference<TEntity, TIndex>(TEntity entity) => new EntityReference<TEntity, TIndex>(entity);

        public bool Equals(EntityReference<TEntity, TIndex> other) => Index.Equals(other.Index);

        public override bool Equals(object obj) =>
            obj is EntityReference<TEntity, TIndex> other ? Equals(other) : false;

        public override int GetHashCode() => Index.GetHashCode();

        IDataObjectChild IDataObjectChild.WithParent(IDataObject parent, int index) {
            Parent = parent;
            return this;
        }

        IEntityReference IEntityReference.WithIndex(int index) {
            Index = index;
            return this;
        }

        public TEntity Entity {
            get {
                var entities = Entities;
                var index = ((IEntityReference)this).Index;
                if (index < 0 || index >= entities.Count) {
                    return null;
                }
                return entities[index];
            }
        }

        private IReadOnlyList<TEntity> Entities => this.Game().Entities.GetEntities<TEntity>();

        IEntity IEntityReference.Entity => Entity;

        IList IEntityReference.Entities => (IList)Entities;
    }

    internal class EntityReferenceSerializer<TEntity, TIndex> : IValueSerializer<EntityReference<TEntity, TIndex>>
        where TEntity : class, IEntity
        where TIndex : struct, IConvertible, IEquatable<TIndex> {

        public static readonly EntityReferenceSerializer<TEntity, TIndex> Instance = new EntityReferenceSerializer<TEntity, TIndex>();

        private static readonly IValueSerializer<TIndex> _valueSerializer = ValueSerializer.Get<TIndex>();

        public EntityReference<TEntity, TIndex> Read(BinaryReader reader) => new EntityReference<TEntity, TIndex>(Convert.ToInt32(_valueSerializer.Read(reader)));

        public void Write(BinaryWriter writer, EntityReference<TEntity, TIndex> value) => _valueSerializer.Write(writer, (TIndex)Convert.ChangeType(value.Index, typeof(TIndex)));
    }
}
