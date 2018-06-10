using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using WarBender.GameData;

namespace WarBender.Modules {
    public class ModuleMetadata {
        public static readonly XNamespace XmlNamespace = "https://int19h.org/WarBender/wmmx.xsd";
        private static readonly XNamespace _xmlns = XmlNamespace;

        private static Lazy<ModuleMetadata> _native = new Lazy<ModuleMetadata>(() => new ModuleMetadata(
            typeof(ModuleMetadata).Assembly.GetManifestResourceStream(typeof(ModuleMetadata), "Native.wmmx")));

        public static ModuleMetadata Native => _native.Value;

        private Dictionary<string, Type> _types = new Dictionary<string, Type>() {
            ["bool"] = typeof(bool),
            ["color"] = typeof(Color),
            ["imod"] = typeof(ItemModifier),
            ["int64"] = typeof(long),
            ["faction"] = typeof(Faction),
            ["info_page"] = typeof(InfoPage),
            ["item_kind"] = typeof(ItemKind),
            ["map_icon"] = typeof(MapIconDefinition),
            ["menu"] = typeof(MenuDefinition),
            ["mesh"] = typeof(MeshDefinition),
            ["particle_system"] = typeof(ParticleSystemDefinition),
            ["party"] = typeof(Party),
            ["party_template"] = typeof(PartyTemplate),
            ["quest"] = typeof(Quest),
            ["scene"] = typeof(Scene),
            ["string"] = typeof(StringDefinition),
            ["tableau_material"] = typeof(TableauMaterialDefinition),
            ["troop"] = typeof(Troop),
        };

        public IReadOnlyDictionary<string, Type> Types => _types;

        public string ModuleName { get; }

        public SlotsDefinitions SlotsDefinitions { get; }

        public GroupsDefinitions GroupsDefinitions { get; }

        internal ModuleMetadata() {
            SlotsDefinitions = new SlotsDefinitions(Enumerable.Empty<SlotsDefinition>());
            GroupsDefinitions = new GroupsDefinitions(Enumerable.Empty<GroupsDefinition>());
        }

        private static readonly XmlSchema _schema = XmlSchema.Read(
            typeof(ModuleMetadata).Assembly.GetManifestResourceStream(typeof(ModuleMetadata), "wmmx.xsd"),
            delegate {});

        private static XmlReaderSettings GetXmlSettings() {
            var settings = new XmlReaderSettings {
                ValidationType = ValidationType.Schema,
            };
            settings.Schemas.Add(_schema);
            return settings;
        }

        public ModuleMetadata(string fileName)
            : this(XDocument.Load(
                XmlReader.Create(fileName, GetXmlSettings()) is var reader ? reader : null,
                LoadOptions.SetBaseUri | LoadOptions.SetLineInfo).Root, reader) {
        }

        private ModuleMetadata(Stream stream)
            : this(XDocument.Load(stream).Root, null) {
        }

        public ModuleMetadata(XmlReader reader)
            : this(XDocument.Load(reader = XmlReader.Create(reader, GetXmlSettings()),
                LoadOptions.SetBaseUri | LoadOptions.SetLineInfo).Root) {
        }

        public ModuleMetadata(XDocument document)
            : this(document.Root) {
        }

        public ModuleMetadata(XElement element)
            : this(element, null) {
        }

        private ModuleMetadata(XElement element, IDisposable disposable) {
            Trace.WriteLine($"Reading {element.BaseUri}", nameof(ModuleMetadata));
            using (disposable) {
                if (element.Name != _xmlns + "module") {
                    throw element.InvalidXml("<module> expected");
                }

                ModuleName = (string)element.Attribute("name");

                var aname = typeof(ModuleMetadata).Assembly.GetName().Name +
                    ".Modules.ModuleDefinition._" + Guid.NewGuid().ToString("N");
                Trace.WriteLine($"Dynamic type assembly: {aname}", nameof(ModuleMetadata));
                var abuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(aname), AssemblyBuilderAccess.RunAndCollect);
                var mbuilder = abuilder.DefineDynamicModule(aname);
                var typeElements = element.Elements(_xmlns + "enum").Concat(element.Elements(_xmlns + "flags"));
                foreach (var typeElement in typeElements) {
                    ParseType(typeElement, mbuilder);
                }

                var groupsElements = element.Elements(_xmlns + "groups");
                GroupsDefinitions = GroupsDefinitions.Parse(groupsElements, this);

                var slotsElements = element.Elements(_xmlns + "slots");
                SlotsDefinitions = SlotsDefinitions.Parse(slotsElements, this);
            }
        }

        internal Type GetType(XAttribute attr) {
            var name = (string)attr;
            if (name == null) {
                return typeof(long);
            } else if (_types.TryGetValue(name, out var type)) {
                return type;
            } else {
                throw attr.InvalidXml($"unrecognized type '{name}'");
            }
        }

        public void AddType(string name, Type type) => _types.Add(name, type);

        private string ParseType(XElement elem, ModuleBuilder mbuilder) {
            var name = (string)elem.Attribute("name");
            if (_types.ContainsKey(name)) {
                throw new InvalidDataException($"Type '{name}' is already defined");
            }

            var isFlags = elem.Name.LocalName == "flags";

            var ebuilder = mbuilder.DefineEnum(name, TypeAttributes.Public, isFlags ? typeof(ulong) : typeof(long));
            foreach (var optionElem in elem.Elements(_xmlns + "option")) {
                var optionName = (string)optionElem.Attribute("name");
                var optionValue = (long)optionElem.Attribute("value");
                ebuilder.DefineLiteral(optionName, optionValue);
            }

            if (isFlags) {
                ebuilder.SetCustomAttribute(new CustomAttributeBuilder(
                    typeof(FlagsAttribute).GetConstructor(Array.Empty<Type>()), Array.Empty<object>()));
            }

            var type = ebuilder.CreateType();
            AddType(name, type);
            Trace.WriteLine($"Type {name} -> {type}", nameof(ModuleMetadata));
            return name;
        }
    }
}
