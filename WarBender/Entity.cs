using System;
using System.Collections.Generic;

namespace WarBender {
    public interface IEntity {
        int Index { get; }
    }

    public interface IEntityContainer {
        IReadOnlyList<IEntity> TryGetEntities(Type entityType);

        IReadOnlyList<IEntity> GetEntities(Type entityType);

        IReadOnlyList<T> GetEntities<T>() where T : IEntity;

        IEntity GetEntity(IEntityReference entityReference);
    }

    internal class EntityContainer : IEntityContainer {
        private readonly Dictionary<Type, Func<IReadOnlyList<IEntity>>> _entities = new Dictionary<Type, Func<IReadOnlyList<IEntity>>>();

        private readonly IEntityContainer _fallback;

        public EntityContainer(IEntityContainer fallback = null) {
            _fallback = fallback;
        }

        public void AddEntities<TEntity>(Func<IReadOnlyList<TEntity>> entities)
            where TEntity : class, IEntity
            => _entities[typeof(TEntity)] = () => entities();

        public IReadOnlyList<IEntity> TryGetEntities(Type entityType) =>
            _entities.TryGetValue(entityType, out var entities) ? entities() :
            _fallback != null ? _fallback.TryGetEntities(entityType) : null;

        public IReadOnlyList<TEntity> GetEntities<TEntity>()
            where TEntity : IEntity
            => (IReadOnlyList<TEntity>)GetEntities(typeof(TEntity));

        public IReadOnlyList<IEntity> GetEntities(Type entityType) =>
            TryGetEntities(entityType) ?? throw new ArgumentOutOfRangeException(nameof(entityType));

        public IEntity GetEntity(IEntityReference entityReference) {
            var entities = GetEntities(entityReference.EntityType);
            var index = entityReference.Index;
            return index >= 0 && index <= entities.Count ? entities[index] : null;
        }
    }
}
