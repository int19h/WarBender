using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;

namespace WarBender.UI.Design {
    internal class RecordConverter : ExpandableObjectConverter {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) =>
            destinationType == typeof(string) || base.CanConvertTo(context, destinationType);

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
            if (destinationType != typeof(string)) {
                return base.ConvertTo(context, culture, value, destinationType);
            }

            switch (value) {
                case IHasName hasName:
                    return hasName.Name;
                case IHasId hasId:
                    return hasId.Id;
                default:
                    return FriendlyNames.Describe(value);
            }
        }
    }

    internal class RecordDescriptor : CustomTypeDescriptor {
        private readonly Type _type;

        public RecordDescriptor(Type type, ICustomTypeDescriptor originalDescriptor)
            : base(originalDescriptor) {
            _type = type;
        }

        public override TypeConverter GetConverter() => new EnumConverter(_type);

        public override AttributeCollection GetAttributes() {
            var attrs = GameTypeDescriptionProvider.GetOriginalProvider<IRecord>()
                .GetTypeDescriptor(_type).GetAttributes().Cast<Attribute>().ToList();
            attrs.Add(new TypeConverterAttribute(typeof(RecordConverter)));
            attrs.Add(new EditorAttribute(typeof(RecordEditor), typeof(UITypeEditor)));
            return new AttributeCollection(attrs.ToArray());
        }
    }

}
