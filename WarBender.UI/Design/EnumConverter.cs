using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;

namespace WarBender.UI.Design {
    internal class EnumConverter : System.ComponentModel.EnumConverter {
        private readonly Type _underlyingType;
        private readonly bool _flags;

        public EnumConverter(Type type)
            : base(type) {
            _underlyingType = Enum.GetUnderlyingType(type);
            _flags = type.IsDefined(typeof(FlagsAttribute), false);
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) => false;

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
            var underlyingType = Enum.GetUnderlyingType(value.GetType());

            object result;
            try {
                result = base.ConvertTo(context, culture, value, destinationType);
            } catch (ArgumentException) {
                var tdp = (TypeDescriptionProvider)context.GetService(typeof(TypeDescriptionProvider));
                var conv = tdp?.GetTypeDescriptor(underlyingType).GetConverter();
                if (conv == null) {
                    throw new ArgumentException($"No converter for underlying type {underlyingType} of enum {EnumType}");
                }
                result = conv.ConvertTo(context, culture, value, destinationType);
            }

            // For [Flags], format raw numbers in hexadecimal.
            if (_flags && result is string s && decimal.TryParse(s, out var _)) {
                object raw = null;
                if (ulong.TryParse(s, out var d)) {
                    raw = d;
                } else if (long.TryParse(s, out var u)) {
                    raw = u;
                } 

                if (raw != null) {
                    return $"0x{raw:X}";
                }
            }

            return result;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
            if (value is string s && s.StartsWith("0x", StringComparison.OrdinalIgnoreCase)) {
                value = ulong.Parse(s.Substring(2), NumberStyles.HexNumber).ToString(culture);
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

    internal class EnumDescriptor : CustomTypeDescriptor {
        private readonly Type _type;

        public EnumDescriptor(Type type, ICustomTypeDescriptor originalDescriptor)
            : base(originalDescriptor) {
            _type = type;
        }

        public override TypeConverter GetConverter() => new EnumConverter(_type);

        public override AttributeCollection GetAttributes() {
            var attrs = GameTypeDescriptionProvider.GetOriginalProvider<Enum>()
                .GetTypeDescriptor(_type).GetAttributes().Cast<Attribute>().ToList();
            attrs.Add(new TypeConverterAttribute(typeof(EnumConverter)));
            if (attrs.Any(attr => attr is FlagsAttribute)) {
                attrs.Add(new EditorAttribute(typeof(FlagsEditor), typeof(UITypeEditor)));
            }
            return new AttributeCollection(attrs.ToArray());
        }
    }
}
