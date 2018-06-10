using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using WarBender.Modules;

namespace WarBender {
    public class Slots : LengthPrefixedCollection<long>, IList, IEnumerable<object> {
        public new IRecord Parent => (IRecord)((IDataObject)this).Parent;

        public LengthPrefixedCollection<long> Raw => this;

        private IReadOnlyList<SlotDefinition> _slotDefinitions;

        public IReadOnlyList<SlotDefinition> SlotDefinitions {
            get {
                if (_slotDefinitions == null) {
                    _slotDefinitions = this.TryGame()?.Module.Metadata.SlotsDefinitions.For(Parent, Count)
                        ?? Enumerable.Range(0, Count).Select(i => new SlotDefinition(i)).ToArray();
                }
                return _slotDefinitions;
            }
        }

        protected override void SetParent(IDataObject parent) {
            if (!(parent is IRecord)) {
                throw new InvalidOperationException($"{nameof(Slots)} must have a record as parent");
            }

            _slotDefinitions = null;
            base.SetParent(parent);
        }

        protected override void OnItemAdded(long item, int index) {
            base.OnItemAdded(item, index);
            if (_slotDefinitions != null && Count > _slotDefinitions.Count) {
                _slotDefinitions = null;
            }
        }

        public override string GetKeyOfIndex(int index) => SlotDefinitions[index].Name;

        public new object this[int index] {
            get {
                var raw = Raw[index];
                var slotType = SlotDefinitions[index].Type;

                object slot;
                if (typeof(Color) == slotType) {
                    slot = Color.FromArgb((int)raw);
                } else if (slotType.IsEnum) {
                    slot = Enum.ToObject(slotType, raw);
                } else if (typeof(IEntity).IsAssignableFrom(slotType)) {
                    slotType = typeof(EntityReference<>).MakeGenericType(slotType);
                    slot = Activator.CreateInstance(slotType, (int)raw);
                } else {
                    slot = ((IConvertible)raw).ToType(slotType, null);
                }

                if (slot is IDataObjectChild child) {
                    slot = child.WithParent(this, index);
                }

                return slot;
            }
            set =>
                Raw[index] = RawValue(value);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public new IEnumerator<object> GetEnumerator() {
            for (int i = 0; i < Count; ++i) {
                yield return this[i];
            }
        }

        int IList.Add(object value) {
            Raw.Add(RawValue(value));
            return Count - 1;
        }

        bool IList.Contains(object value) => Raw.Contains(RawValue(value));

        int IList.IndexOf(object value) => Raw.IndexOf(RawValue(value));

        void IList.Insert(int index, object value) => Raw.Insert(index, RawValue(value));

        void IList.Remove(object value) => Raw.Remove(RawValue(value));

        private static long RawValue(object value) =>
            value == null ? -1 :
            value is IRecord record ? record.Index :
            value is IEntityReference eref ? eref.Index :
            value is IConvertible conv ? conv.ToInt64(CultureInfo.InvariantCulture) :
            value is Color color ? color.ToArgb() :
            throw new ArgumentOutOfRangeException("value");
    }
}
