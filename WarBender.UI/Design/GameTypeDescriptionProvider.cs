using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Reflection;
using WarBender.Modules;

namespace WarBender.UI.Design {
    using ICollection = System.Collections.ICollection;

    internal class GameTypeDescriptionProvider : TypeDescriptionProvider {
        public static readonly GameTypeDescriptionProvider Instance = new GameTypeDescriptionProvider();

        private static readonly Dictionary<Type, Func<Type, ICustomTypeDescriptor, ICustomTypeDescriptor>> _descriptors = new Dictionary<Type, Func<Type, ICustomTypeDescriptor, ICustomTypeDescriptor>>() {
            [typeof(IRecord)] = (t, otd) => new RecordDescriptor(t, otd),
            [typeof(ICollection)] = (t, otd) => new GameTypeDescriptor<ICollection, CollectionConverter>(otd),
            [typeof(IEntityReference)] = (t, otd) => new EntityReferenceDescriptor(otd),
            [typeof(EntityDefinition)] = (t, otd) => new GameTypeDescriptor<EntityDefinition, EntityDefinitionConverter>(otd),
            [typeof(Enum)] = (t, otd) => new EnumDescriptor(t, otd),
        };

        private static readonly Dictionary<Type, TypeDescriptionProvider> _originalProviders = new Dictionary<Type, TypeDescriptionProvider>();

        public static void Register() {
            foreach (var type in _descriptors.Keys) {
                _originalProviders[type] = TypeDescriptor.GetProvider(type);
                TypeDescriptor.AddProvider(Instance, type);
            }
        }

        public static TypeDescriptionProvider GetOriginalProvider<T>() => _originalProviders[typeof(T)];

        public override bool IsSupportedType(Type type) =>
            _descriptors.Keys.Any(t => t.IsAssignableFrom(type));

        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance) {
            foreach (var kv in _descriptors) {
                if (kv.Key.IsAssignableFrom(objectType)) {
                    var otd = _originalProviders[kv.Key].GetTypeDescriptor(objectType);
                    return kv.Value(objectType, otd);
                }
            }
            return base.GetTypeDescriptor(objectType, instance);
        }

        public override object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args) {
            if (objectType.IsAbstract && typeof(IRecord).IsAssignableFrom(objectType)) {
                return objectType.InvokeMember("Create", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, args);
            }
            return base.CreateInstance(provider, objectType, argTypes, args);
        }
    }

    internal class GameTypeDescriptor<T, TConverter> : CustomTypeDescriptor
        where TConverter : TypeConverter, new() {

        public GameTypeDescriptor(ICustomTypeDescriptor originalDescriptor)
            : base(originalDescriptor) {
        }

        public override TypeConverter GetConverter() => new TConverter();

        public override AttributeCollection GetAttributes() {
            var attrs = base.GetAttributes().Cast<Attribute>().ToList();
            attrs.Add(new TypeConverterAttribute(typeof(TConverter)));
            return new AttributeCollection(attrs.ToArray());
        }
    }

    internal class EntityReferenceDescriptor : GameTypeDescriptor<IEntityReference, EntityReferenceConverter> {
        public EntityReferenceDescriptor(ICustomTypeDescriptor originalDescriptor)
            : base(originalDescriptor) {
        }

        public override AttributeCollection GetAttributes() {
            var attrs = base.GetAttributes().Cast<Attribute>().ToList();
            attrs.Add(new EditorAttribute(typeof(EntityReferenceEditor), typeof(UITypeEditor)));
            return new AttributeCollection(attrs.ToArray());
        }
    }
}
