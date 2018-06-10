using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using WarBender.GameData;

namespace WarBender.Modules {
    public struct SlotDefinition {
        public int Index { get; }

        public string Name { get; }

        public Type Type { get; }

        public SlotDefinition(int index, string name = null, Type type = null) {
            Index = index;
            Name = name;
            Type = type ?? typeof(long);
        }

        public static SlotDefinition Parse(XElement element, ModuleMetadata module) {
            var index = (int)element.Attribute("index");
            var name = (string)element.Attribute("name");
            var type = module.GetType(element.Attribute("type"));
            return new SlotDefinition(index, name, type);
        }
    }

    public interface ISlotDefinitions {
        SlotDefinition? this[int index] { get; }
    }

    public class SlotDefinitions : ISlotDefinitions {
        public IReadOnlyDictionary<int, SlotDefinition> Definitions { get; }

        public SlotDefinitions()
            : this(Enumerable.Empty<SlotDefinition>()) {
        }

        public SlotDefinitions(IEnumerable<SlotDefinition> slotDefinitions) {
            Definitions = slotDefinitions.ToDictionary(slotDef => slotDef.Index);
        }

        public SlotDefinition? this[int index] =>
            Definitions.TryGetValue(index, out var def) ? def : (SlotDefinition?)null;
    }

    public class ArraySlotDefinitions : ISlotDefinitions {
        public Type ElementType { get; }

        public ArraySlotDefinitions(Type elementType) {
            ElementType = elementType;
        }

        public SlotDefinition? this[int index] =>
            new SlotDefinition(index, null, ElementType);
    }

    public class SlotsDefinition {
        public Type EntityType { get; }
        public IReadOnlyCollection<object> EntityIds { get; }
        public IReadOnlyCollection<object> TemplateIds { get; }
        public ISlotDefinitions SlotDefinitions { get; }

        public SlotsDefinition(
            Type entityType,
            IEnumerable<object> entityIds = null,
            IEnumerable<object> templateIds = null,
            ISlotDefinitions slotDefinitions = null) {

            EntityType = entityType;
            EntityIds = entityIds?.ToArray() ?? Array.Empty<object>();
            TemplateIds = templateIds?.ToArray() ?? Array.Empty<object>();
            SlotDefinitions = slotDefinitions ?? new SlotDefinitions();
        }

        private static readonly char[] whitespace = { '\r', '\n', '\t', ' ' };

        public static SlotsDefinition Parse(XElement element, ModuleMetadata module) {
            var xmlns = ModuleMetadata.XmlNamespace;
            var entityType = module.GetType(element.Attribute("of"));

            IEnumerable<object> ParseId(string token, XAttribute attr) {
                if (int.TryParse(token, out var n)) {
                    return new object[] { n };
                } else if (token.StartsWith("$")) {
                    var groupDef = module.GroupsDefinitions.GetGroupDefinition(entityType, token, attr);
                    return groupDef.Ranges.SelectMany(range => range.Cast<object>());
                } else {
                    return new object[] { token };
                }
            }

            IEnumerable<object> ParseIds(XAttribute attr) => ((string)attr ?? "")
                .Split(whitespace, StringSplitOptions.RemoveEmptyEntries)
                .SelectMany(s => ParseId(s, attr))
                .Distinct();

            var entityIds = ParseIds(element.Attribute("id"));
            var templateIds = ParseIds(element.Attribute("template-id"));

            ISlotDefinitions slotDefinitions;
            var arrayElement = element.Element(xmlns + "array");
            if (arrayElement != null) {
                slotDefinitions = new ArraySlotDefinitions(module.GetType(arrayElement.Attribute("type")));
            } else {
                var slotDefs = new Dictionary<int, SlotDefinition>();
                foreach (var slotElement in element.Elements(xmlns + "slot")) {
                    var slotDef = SlotDefinition.Parse(slotElement, module);
                    if (slotDefs.ContainsKey(slotDef.Index)) {
                        throw slotElement.InvalidXml($"Slot {slotDef.Index} is already defined for {entityType.Name}");
                    }
                    slotDefs.Add(slotDef.Index, slotDef);
                }
                slotDefinitions = new SlotDefinitions(slotDefs.Values);
            }

            return new SlotsDefinition(entityType, entityIds, templateIds, slotDefinitions);
        }
    }

    public class SlotsDefinitions {
        private readonly Dictionary<Type, List<SlotsDefinition>> _definitions =
            new Dictionary<Type, List<SlotsDefinition>>();

        public SlotsDefinitions(IEnumerable<SlotsDefinition> definitions) {
            foreach (var def in definitions) {
                if (!_definitions.TryGetValue(def.EntityType, out var defs)) {
                    _definitions[def.EntityType] = defs = new List<SlotsDefinition>();
                }
                defs.Add(def);
            }
        }

        public static SlotsDefinitions Parse(IEnumerable<XElement> elements, ModuleMetadata module) =>
            new SlotsDefinitions(elements.Select(element => SlotsDefinition.Parse(element, module)));

        public IReadOnlyList<SlotDefinition> For(IRecord record, int slotCount) {
            var entityType = record.Type;
            var index = record.Index;
            var id = (record as IHasId)?.Id;
            var slotsDefs = GetDefinitions(entityType).Where(def =>
                (def.EntityIds.Count == 0 && def.TemplateIds.Count == 0) ||
                def.EntityIds.Contains(index) ||
                def.EntityIds.Contains(id) || (
                    record is Party party &&
                    party.party_template_id.Entity is PartyTemplate template && (
                        def.TemplateIds.Contains(template.Index) ||
                        def.TemplateIds.Contains(template.Definition?.Id))));

            var slotDefs = new SlotDefinition[slotCount];
            for (int i = 0; i < slotCount; ++i) {
                SlotDefinition? slotDef = null;
                foreach (var slotsDef in slotsDefs) {
                    var newDef = slotsDef.SlotDefinitions[i];
                    if (newDef != null) {
                        slotDef = newDef;
                    }
                }
                slotDefs[i] = slotDef ?? new SlotDefinition(i);
            }

            return slotDefs;
        }

        public IReadOnlyCollection<SlotsDefinition> GetDefinitions(Type entityType) =>
            _definitions.TryGetValue(entityType, out var defs) ? defs :
            (IReadOnlyCollection<SlotsDefinition>)Array.Empty<SlotsDefinition>();

        public IReadOnlyCollection<SlotsDefinition> GetDefinitions<TEntity>() =>
            GetDefinitions(typeof(TEntity));
    }
}
