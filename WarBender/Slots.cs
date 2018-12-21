using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using WarBender.Modules;

namespace WarBender {
    public class Slots : IDataObject, ICollection, IList<object> {
        private IReadOnlyList<SlotDefinition> _slotDefinitions;

        public Slots() {
        }

        [ParenthesizePropertyName(true)]
        public LengthPrefixedCollection<long> Raw { get; } = new LengthPrefixedCollection<long>();

        public IReadOnlyList<SlotDefinition> SlotDefinitions {
            get {
                if (_slotDefinitions == null) {
                    _slotDefinitions = this.TryGame()?.Module.Metadata.SlotsDefinitions.For(Parent, Count)
                        ?? Enumerable.Range(0, Count).Select(i => new SlotDefinition(i)).ToArray();
                }
                return _slotDefinitions;
            }
        }

        public IDataObjectChild WithParent(IDataObject parent, int index = -1) {
            if (!(parent is IRecord)) {
                throw new ArgumentOutOfRangeException(nameof(parent));
            }

            var raw = ((IDataObject)Raw).WithParent(parent, index);
            Trace.Assert(Raw == raw);
            return this;
        }

        public string GetKeyOfIndex(int index) => index < SlotDefinitions.Count ? SlotDefinitions[index].Name : null;

        private static long RawValue(object value) =>
            value == null ? -1 :
            value is IRecord record ? record.Index :
            value is IEntityReference eref ? eref.Index :
            value is IConvertible conv ? conv.ToInt64(CultureInfo.InvariantCulture) :
            value is Color color ? color.ToArgb() :
            throw new ArgumentOutOfRangeException("value");

        private object TypedValue(long value, int index) {
            if (index >= SlotDefinitions.Count) {
                return value;
            }

            var raw = Raw[index];
            var slotType = SlotDefinitions[index].Type;

            object slot;
            if (typeof(Color) == slotType) {
                slot = Color.FromArgb((int)raw);
            } else if (slotType.IsEnum) {
                slot = Enum.ToObject(slotType, raw);
            } else if (typeof(IEntity).IsAssignableFrom(slotType)) {
                slotType = typeof(EntityReference<,>).MakeGenericType(slotType, typeof(int));
                slot = Activator.CreateInstance(slotType, (int)raw);
            } else {
                slot = ((IConvertible)raw).ToType(slotType, null);
            }

            if (slot is IDataObjectChild child) {
                slot = child.WithParent(this, index);
            }

            return slot;
        }

        public object this[int index] {
            get => TypedValue(Raw[index], index);
            set => Raw[index] = RawValue(value);
        }

        public IEnumerator<object> GetEnumerator() => Raw.Select((x, i) => TypedValue(x, i)).GetEnumerator();

        public int Add(object value) => ((ICollection)Raw).Add(RawValue(value));

        public int GetLength() => Raw.GetLength();

        public bool Contains(object value) => Raw.Contains(RawValue(value));

        public void Clear() => Raw.Clear();

        public int IndexOf(object value) =>Raw.IndexOf(RawValue(value));

        public void Insert(int index, object value) => ((ICollection)Raw).Insert(index, RawValue(value));

        public void Remove(object value) => ((ICollection)Raw).Remove(RawValue(value));

        public void RemoveAt(int index) => Raw.RemoveAt(index);

        public void CopyTo(Array array, int index) {
            foreach (var value in this) {
                array.SetValue(value, index++);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        void ICollection<object>.Add(object item) => Add(RawValue(item));

        public void CopyTo(object[] array, int arrayIndex) => CopyTo(array, arrayIndex);

        bool ICollection<object>.Remove(object item) => Raw.Remove(RawValue(item));

        [ParenthesizePropertyName(true)]
        public long SizeInBytes => Raw.SizeInBytes;

        IDataObject IDataObjectChild.Parent => Raw.Parent;

        [Browsable(false)]
        IRecord Parent => (IRecord)Raw.Parent;

        [Browsable(false)]
        public Type ItemType => Raw.ItemType;

        [Browsable(false)]
        public bool IsReadOnly => ((ICollection)Raw).IsReadOnly;

        [Browsable(false)]
        public bool IsFixedSize => ((ICollection)Raw).IsFixedSize;

        [ParenthesizePropertyName(true)]
        public int Count => Raw.Count;

        [Browsable(false)]
        public object SyncRoot => ((ICollection)Raw).SyncRoot;

        [Browsable(false)]
        public bool IsSynchronized => ((ICollection)Raw).IsSynchronized;

        public void ReadFrom(BinaryReader reader) => Raw.ReadFrom(reader);

        public void WriteTo(BinaryWriter writer) => Raw.WriteTo(writer);
    }
}
