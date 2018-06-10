using System;
using System.ComponentModel;
using System.Globalization;
using WarBender.Modules;

namespace WarBender.UI.Design {
    internal class EntityDefinitionConverter : ExpandableObjectConverter {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) =>
            destinationType == typeof(string) || base.CanConvertTo(context, destinationType);

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
            if (destinationType != typeof(string)) {
                return base.ConvertTo(context, culture, value, destinationType);
            }

            switch (value) {
                case IHasId hasId:
                    return hasId.Id;
                case EntityDefinition def:
                    return FriendlyNames.Singular(def.EntityType) + " definition #" + def.Index;
                default:
                    return null;
            }
        }
    }
}
