using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace WarBender
{
    public interface ICollection : IList, IDataObject, IDataObjectChild {
        Type ItemType { get; }

        int GetLength();

        string GetKeyOfIndex(int index);
    }

    public abstract class Collection<T> : ObservableCollection<T>, ICollection {
        private static readonly IDataObjectFactory<T> _itemFactory = DataObjectFactory.TryGet<T>();
        private static readonly IValueSerializer<T> _itemSerializer = ValueSerializer.TryGet<T>();
        private static readonly IBatchValueSerializer<T> _batchSerializer = _itemSerializer as IBatchValueSerializer<T>;

        static Collection() {
            if (_itemSerializer == null && !typeof(IDataObject).IsAssignableFrom(typeof(T))) {
                throw new NotSupportedException(
                    $"{typeof(T).FullName} is not supported by {nameof(ValueSerializer)}, " +
                    $"nor does it implement {typeof(IDataObject).FullName}");
            }
            if (_itemSerializer == null && _itemFactory == null) {
                throw new NotSupportedException(
                    $"{typeof(T).FullName} is not supported by {nameof(ValueSerializer)} " +
                    $"nor by {nameof(DataObjectFactory)}");
            }
        }

        [ParenthesizePropertyName(true)]
        public new int Count => base.Count;

        [Browsable(false)]
        public Type ItemType => typeof(T);

        [Browsable(false)]
        public IDataObject Parent { get; private set; }

        private long? _sizeInBytes;

        [ParenthesizePropertyName(true)]
        public long SizeInBytes {
            get {
                if (_sizeInBytes == null) {
                    using (var stream = new MemoryStream()) {
                        using (var writer = new BinaryWriter(stream, Encoding.Default, true)) {
                            WriteTo(writer);
                        }
                        stream.Flush();
                        _sizeInBytes = stream.Length;
                    }
                }
                return _sizeInBytes.Value;
            }
        }

        IDataObjectChild IDataObjectChild.WithParent(IDataObject parent, int index) {
            SetParent(parent);
            return this;
        }

        protected virtual void SetParent(IDataObject parent) {
            Parent = parent;
        }

        public abstract int GetLength();

        public virtual string GetKeyOfIndex(int index) {
            if (index >= 0 && index <= Count && this[index] is IHasId hasId) {
                return hasId.Id;
            }
            return null;
        }

        public int GetIndexOfKey(string key) {
            var match = Enumerable.Range(0, Count).Where(i => GetKeyOfIndex(i) == key).ToArray();
            if (match.Length == 1) {
                return match[0];
            } else if (match.Length == 0) {
                throw new KeyNotFoundException($"No item with key '{key}'");
            } else {
                throw new KeyNotFoundException($"Multiple items match key '{key}': {string.Join(", ", match)}");
            }
        }

        public T this[string key] {
            get => this[GetIndexOfKey(key)];
            set => this[GetIndexOfKey(key)] = value;
        }

        protected abstract void ReadLength(BinaryReader reader);

        protected abstract void WriteLength(BinaryWriter writer);

        public virtual void ReadFrom(BinaryReader reader) {
            ReadLength(reader);
            var length = GetLength();
            if (_batchSerializer != null) {
                var items = _batchSerializer.Read(reader, length);
                Clear();
                foreach (var item in items) {
                    Add(item);
                }
            } else if (_itemSerializer != null) {
                Clear();
                for (int i = 0; i < length; ++i) {
                    var item = _itemSerializer.Read(reader);
                    Add(item);
                }
            } else {
                while (Count > length) {
                    RemoveAt(Count - 1);
                }

                for (int i = 0; i < length; ++i) {
                    T item;
                    if (i < Count) {
                        item = this[i];
                    } else {
                        item = _itemFactory.Create();
                        Add(item);
                    }
                    ((IDataObject)item).ReadFrom(reader);
                }
            }
        }

        public virtual void WriteTo(BinaryWriter writer) {
            var length = GetLength();
            if (length != Count) {
                throw new InvalidDataException($"Collection is supposed to have {length} elements, but has {Count}");
            }

            WriteLength(writer);
            if (_batchSerializer != null) {
                _batchSerializer.Write(writer, this.ToArray());
            } else if (_itemSerializer != null) {
                foreach (var item in this) {
                    _itemSerializer.Write(writer, item);
                }
            } else {
                foreach (var item in this) {
                    ((IDataObject)item).WriteTo(writer);
                }
            }
        }

        protected virtual void OnItemAdded(T item, int index) {
            _sizeInBytes = null;
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(SizeInBytes)));

            if (item is IDataObjectChild child) {
                child.WithParent(this, index);
            }
        }

        protected virtual void OnItemRemoved(T item) {
            _sizeInBytes = null;
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(SizeInBytes)));

            if (item is IDataObjectChild child) {
                child.WithParent(null);
            }
        }

        protected override void InsertItem(int index, T item) {
            base.InsertItem(index, item);
            OnItemAdded(item, index);
        }

        protected override void RemoveItem(int index) {
            OnItemRemoved(this[index]);
            base.RemoveItem(index);
        }

        protected override void SetItem(int index, T item) {
            OnItemRemoved(this[index]);
            base.SetItem(index, item);
            OnItemAdded(item, index);
        }

        protected override void ClearItems() {
            foreach (var item in this) {
                OnItemRemoved(item);
            }
            base.ClearItems();
        }
    }

    public class FixedLengthCollection<T> : Collection<T>, IList {
        [Browsable(false)]
        public int Length { get; }

        public FixedLengthCollection(int length) {
            Length = length;
        }

        public override int GetLength() => Length;

        public static implicit operator FixedLengthCollection<T>(int length) =>
            new FixedLengthCollection<T>(length);

        protected override void ReadLength(BinaryReader reader) { }

        protected override void WriteLength(BinaryWriter writer) { }

        bool IList.IsFixedSize => true;
    }

    public class LengthPrefixedCollection<T> : Collection<T> {
        [Browsable(false)]
        public int Length { get; private set; }

        public override int GetLength() => Length;

        protected override void ReadLength(BinaryReader reader) =>
            Length = reader.ReadInt32();

        protected override void WriteLength(BinaryWriter writer) =>
            writer.Write(Length);
    }

    public class DynamicCollection<T> : LengthPrefixedCollection<T> {
        [Browsable(false)]
        public int DynamicItemCount { get; private set; }

        protected override void ReadLength(BinaryReader reader) {
            base.ReadLength(reader);
            DynamicItemCount = reader.ReadInt32();
        }

        protected override void WriteLength(BinaryWriter writer) {
            base.WriteLength(writer);
            writer.Write(DynamicItemCount);
        }
    }

    public class ComputedLengthCollection<T> : Collection<T> {
        private readonly Func<Game, int> _computeLength;

        public ComputedLengthCollection(Func<Game, int> computeLength) {
            _computeLength = computeLength;
        }

        public override int GetLength() => _computeLength(this.Game());

        protected override void ReadLength(BinaryReader reader) { }

        protected override void WriteLength(BinaryWriter writer) { }
    }

    public class ForEachOf<TEntity, T> : Collection<T> 
        where TEntity : IEntity {

        public Collection<TEntity> Entities => (Collection<TEntity>)this.Game().Entities.GetEntities<TEntity>();

        public override int GetLength() => Entities.GetLength();

        public override string GetKeyOfIndex(int index) =>
            base.GetKeyOfIndex(index) ??
            (index >= 0 && index <= Entities.Count ? Entities.GetKeyOfIndex(index) : null);

        protected override void ReadLength(BinaryReader reader) { }

        protected override void WriteLength(BinaryWriter writer) { }
    }
}
