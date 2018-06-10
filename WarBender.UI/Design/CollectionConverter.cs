using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace WarBender.UI.Design {
    internal class CollectionConverter : TypeConverter {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) =>
            destinationType == typeof(string) || base.CanConvertTo(context, destinationType);

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
            if (destinationType != typeof(string) || !(value is System.Collections.ICollection coll)) {
                return base.ConvertTo(context, culture, value, destinationType);
            }
            return FriendlyNames.Describe(coll);
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context) => true;

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes) {
            if (!(value is ICollection coll)) {
                return base.GetProperties(context, value, attributes);
            }

            var w = (coll.Count - 1).ToString().Length;
            string GetName(object item, int i) {
                var name = i.ToString().PadLeft(w, '\u2007');
                var key = coll.GetKeyOfIndex(i);
                if (key != null) {
                    name += $" ({key})";
                }
                return name;
            }

            var desc = coll.Cast<object>().Select((item, i) =>
                new ItemPropertyDescriptor(value.GetType(), i, GetName(item, i), item.GetType()));
            return new PropertyDescriptorCollection(desc.ToArray());
        }

        private class ItemPropertyDescriptor : SimplePropertyDescriptor {
            private readonly int _index;

            public ItemPropertyDescriptor(Type componentType, int index, string name, Type propertyType)
                : base(componentType, name, propertyType) {
                _index = index;
            }

            public override object GetValue(object component) =>
                ((IList)component)[_index];

            public override void SetValue(object component, object value) =>
                ((IList)component)[_index] = value;
        }
    }
}
