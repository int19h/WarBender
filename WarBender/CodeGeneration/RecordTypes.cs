using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WarBender.CodeGeneration {
    public struct RecordProperty {
        public string RecordTypeName;
        public string Name;
        public string TypeName;
        public string UnderlyingTypeName;
        public string Visibility;
        public bool IsReadOnly;
        public bool IsRecord;
        public bool IsDataObject;
        public bool IsDataObjectChild;
        public bool NotifyPropertyChanged;
        public string Condition;
        public uint? BitField;
    }

    public partial class RecordTypes : IReadOnlyDictionary<string, RecordProperty[]> {
        public int Hash { get; private set; } = 17;

        private readonly Dictionary<string, RecordProperty[]> _types = new Dictionary<string, RecordProperty[]>();
        private readonly Dictionary<Type, string> _serializers = new Dictionary<Type, string>();

        private static bool IsRecord(Type t) =>
            !t.IsGenericType &&
            !t.IsNested &&
            t != typeof(Record) &&
            typeof(Record).IsAssignableFrom(t);

        public RecordTypes() {
            Type[] types;
            try {
                types = typeof(IRecord).Assembly.GetTypes();
            } catch (ReflectionTypeLoadException ex) {
                throw ex.LoaderExceptions[0];
            }

            foreach (var type in types.Where(IsRecord)) {
                AddHash(type.FullName);
                _types[type.FullName] = GetProperties(type).ToArray();
            }
        }

        private IEnumerable<RecordProperty> GetProperties(Type type) {
            var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
            var propInfos = type.GetProperties(bindingFlags);
            foreach (var propInfo in propInfos) {
                if (propInfo.Name.Contains('.') || propInfo.IsDefined(typeof(ComputedAttribute))) {
                    continue;
                }

                var propType = propInfo.PropertyType;
                AddSerializers(propType);

                void Error(string message) {
                    throw new NotSupportedException($"{propType.FullName}.{propInfo.Name}: {message}");
                }

                if (!propInfo.CanRead) {
                    Error("write-only properties are not supported");
                }
                if (propInfo.CanWrite && propInfo.SetMethod.IsPublic && !propInfo.SetMethod.IsAbstract) {
                    Error("public properties of records must be either readonly or abstract");
                }

                RecordProperty prop;
                AddHash(prop.RecordTypeName = type.FullName);
                AddHash(prop.Name = propInfo.Name);
                AddHash(prop.TypeName = prop.UnderlyingTypeName = propType.FullName);
                AddHash(prop.IsReadOnly = !propInfo.CanWrite);
                AddHash(prop.IsRecord = IsRecord(propType));
                AddHash(prop.NotifyPropertyChanged = propInfo.CanWrite && propInfo.SetMethod.IsAbstract);

                AddHash(prop.IsDataObject = typeof(IDataObject).IsAssignableFrom(propType));
                AddHash(prop.IsDataObjectChild = typeof(IDataObjectChild).IsAssignableFrom(propType));
                if (prop.IsDataObject && !prop.IsDataObjectChild) {
                    Error("unsupported property type " + propType.FullName);
                }

                var getInfo = propInfo.GetMethod;
                if (getInfo.IsPublic) {
                    prop.Visibility = "public";
                } else if (getInfo.IsFamily) {
                    prop.Visibility = "protected";
                } else if (getInfo.IsPrivate) {
                    prop.Visibility = "private";
                } else if (getInfo.IsAssembly) {
                    prop.Visibility = "internal";
                } else if (getInfo.IsFamilyOrAssembly) {
                    prop.Visibility = "protected internal";
                } else {
                    Error("unsupported visibility");
                }
                AddHash(prop.Visibility);

                if (propType.IsEnum) {
                    AddHash(prop.UnderlyingTypeName = Enum.GetUnderlyingType(propType).FullName);
                } 

                prop.Condition = "true";
                {
                    var conditionalAttr = propInfo.GetCustomAttribute<ConditionalOnAttribute>();
                    if (conditionalAttr != null) {
                        prop.Condition += " && this." + conditionalAttr.MemberName;
                        var member = type.GetMember(conditionalAttr.MemberName, bindingFlags).Single();
                        if (member is MethodInfo) {
                            prop.Condition += "()";
                        }
                    }

                    var verAttrs = propInfo.GetCustomAttributes<GameVersionAttribute>();
                    if (verAttrs.Any()) {
                        prop.Condition += " && game.MatchVersions(";
                        var sep = "";
                        foreach (var verAttr in verAttrs) {
                            prop.Condition += sep + verAttr.MinVersion + (sep = ", ") + verAttr.MaxVersion;
                        }
                        prop.Condition += ")";
                    }
                }
                AddHash(prop.Condition);

                var bitFieldAttr = propInfo.GetCustomAttribute<BitFieldAttribute>();
                prop.BitField = bitFieldAttr?.Width;
                if (prop.BitField is uint bitField) {
                    if (bitField > sizeof(ulong) * 8) {
                        Error($"bitfield cannot be larger than {typeof(ulong).Name}");
                    }
                    if (prop.Condition != "true") {
                        Error("conditional bitfields are not supported");
                    }
                }
                AddHash(prop.BitField);

                yield return prop;
            }
        }

        public void AddHash<T>(T x) {
            Hash = Hash * 31;
            if (x != null) {
                Hash += x.GetHashCode();
            }
        }

        static partial void GetGeneratedCodeHash(ref int hash);

        public void Validate() {
            int hash = 0;
            GetGeneratedCodeHash(ref hash);
            if (hash != Hash) {
                throw new Exception(
                    "Runtime hash of record types doesn't match hash used for generated code. " +
                    "Did you forget to re-generate code from RecordReader.tt?");
            }
        }

        public IReadOnlyDictionary<Type, string> Serializers => _serializers;

        private void AddSerializers(Type type) {
            if (_serializers.ContainsKey(type)) {
                return;
            }

            if (type.IsGenericType) {
                foreach (var targ in type.GenericTypeArguments) {
                    AddSerializers(targ);
                }
            }

            Type serType;
            if (type.IsEnum) {
                serType = typeof(EnumSerializer<>).MakeGenericType(type);
            } else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(EntityReference<,>)) {
                var targs = type.GetGenericArguments();
                serType = typeof(EntityReferenceSerializer<,>).MakeGenericType(targs);
            } else {
                return;
            }
            _serializers.Add(type, serType.FullName);
        }

        static partial void GetTemplateHash(ref int hash);

        public IEnumerable<string> Keys => ((IReadOnlyDictionary<string, RecordProperty[]>)_types).Keys;

        public IEnumerable<RecordProperty[]> Values => ((IReadOnlyDictionary<string, RecordProperty[]>)_types).Values;

        public int Count => _types.Count;

        public RecordProperty[] this[string key] => _types[key];

        public bool ContainsKey(string key) => _types.ContainsKey(key);

        public bool TryGetValue(string key, out RecordProperty[] value) => _types.TryGetValue(key, out value);

        public IEnumerator<KeyValuePair<string, RecordProperty[]>> GetEnumerator() => ((IReadOnlyDictionary<string, RecordProperty[]>)_types).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IReadOnlyDictionary<string, RecordProperty[]>)_types).GetEnumerator();
    }
}
