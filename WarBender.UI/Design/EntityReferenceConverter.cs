using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace WarBender.UI.Design {
    internal class EntityReferenceConverter : ExpandableObjectConverter {
        private static Type GetEntityReferenceType(ITypeDescriptorContext context) {
            var erefType = context?.PropertyDescriptor?.PropertyType;
            if (erefType != null) {
                if (erefType.IsGenericType && erefType.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                    erefType = erefType.GenericTypeArguments[0];
                }
                if (erefType.IsGenericType && erefType.GetGenericTypeDefinition() == typeof(EntityReference<>)) {
                    return erefType;
                }
            }
            return null;
        }

        private static Game GetGame(ITypeDescriptorContext context) =>
            (context?.Instance as IDataObjectChild)?.TryGame() ??
            ((context?.Instance as IEnumerable<object>)?.FirstOrDefault() as IDataObject)?.TryGame();

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) =>
            destinationType == typeof(string) || base.CanConvertTo(context, destinationType);

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
            if (destinationType == typeof(string) && value is IEntityReference eref) {
                var s = eref.Index.ToString();
                var game = eref.TryGame() ?? GetGame(context);
                var entity = game?.Entities.GetEntity(eref);
                if (entity != null) {
                    if (entity is IHasId hasId && !string.IsNullOrEmpty(hasId.Id)) {
                        s = hasId.Id;
                    }
                    if (entity is IHasName hasName) {
                        s += $" ({hasName.Name})";
                    }
                }
                return s;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) =>
            sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
            if (value is string s && GetEntityReferenceType(context) is Type erefType) {
                var tail = s.IndexOf('(');
                if (tail >= 0) {
                    s = s.Substring(0, tail);
                }
                s = s.Trim();

                if (int.TryParse(s, out var n)) {
                    return Activator.CreateInstance(erefType, n);
                }

                var game = GetGame(context);
                if (game != null) {
                    var entityType = erefType.GenericTypeArguments[0];
                    var entities = game.Entities.GetEntities(entityType).OfType<IHasId>();
                    var entity = entities.FirstOrDefault(ent => ent.Id == s);
                    if (entity == null) {
                        throw new IndexOutOfRangeException(
                            $"There is no {FriendlyNames.Singular(entityType)} with ID '{s}'");
                    }
                    return Activator.CreateInstance(erefType, entity);
                }
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) =>
            context?.Instance != null && context.PropertyDescriptor?.GetValue(context.Instance) is IEntityReference;

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) {
            if (context?.Instance != null &&
                context.PropertyDescriptor.GetValue(context.Instance) is IEntityReference eref) {
                var values = eref.Entities.Cast<IEntity>().Select(entity => eref.WithIndex(entity.Index));
                return new StandardValuesCollection(values.ToArray());
            }

            return base.GetStandardValues(context);
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes) {
            if (!(value is IEntityReference eref)) {
                return base.GetProperties(context, value, attributes);
            }

            if (eref.Entity == null) {
                return PropertyDescriptorCollection.Empty;
            }

            var converter = TypeDescriptor.GetConverter(eref.Entity);
            return new PropertyDescriptorCollection(
                converter.GetProperties(eref.Entity).Cast<PropertyDescriptor>().Select(pd =>
                    new EntityPropertyDescriptor(eref, pd)).ToArray());
        }

        private class EntityPropertyDescriptor : PropertyDescriptor {
            private readonly IEntityReference _eref;
            private readonly PropertyDescriptor _desc;

            public EntityPropertyDescriptor(IEntityReference eref, PropertyDescriptor desc)
                : base(desc.Name, desc.Attributes.Cast<Attribute>().ToArray()) {
                _eref = eref;
                _desc = desc;
            }

            public override Type ComponentType => _desc.ComponentType;

            public override bool IsReadOnly => _desc.IsReadOnly;

            public override Type PropertyType => _desc.PropertyType;

            public override bool CanResetValue(object component) =>
                _desc.CanResetValue(component);

            public override object GetValue(object component) =>
                _desc.GetValue(((IEntityReference)component).Entity);

            public override void ResetValue(object component) =>
                _desc.ResetValue(((IEntityReference)component).Entity);

            public override void SetValue(object component, object value) =>
                _desc.SetValue(((IEntityReference)component).Entity, value);

            public override bool ShouldSerializeValue(object component) =>
                _desc.ShouldSerializeValue(((IEntityReference)component).Entity);
        }
    }
}
