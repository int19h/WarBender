using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace WarBender.Modules {
    public class GroupDefinition {
        public Type EntityType { get; }

        public string Name { get; }

        public IReadOnlyCollection<Range> Ranges { get; }

        public IReadOnlyCollection<int> Indices { get; }

        public GroupDefinition(Type entityType, string name, IEnumerable<Range> ranges) {
            EntityType = entityType;
            Name = name;
            Ranges = ranges.ToArray();
            Indices = new HashSet<int>(ranges.SelectMany(range => range));
        }

        private static readonly char[] whitespace = { '\r', '\n', '\t', ' ' };

        internal static GroupDefinition Parse(XElement element, Type entityType, IReadOnlyDictionary<string, GroupDefinition> groupDefs) {
            var name = (string)element.Attribute("name");

            var ranges = new List<Range>();
            var rangeAttr = element.Attribute("range");
            if (rangeAttr != null) {
                var rangeTokens = ((string)rangeAttr)
                    .Split(whitespace, StringSplitOptions.RemoveEmptyEntries)
                    .Concat(new string[] { null })
                    .GetEnumerator();
                var atEnd = !rangeTokens.MoveNext();
                while (rangeTokens.Current != null) {
                    var token = rangeTokens.Current;
                    Range range;
                    if (int.TryParse(token, out range.LowerBound)) {
                        rangeTokens.MoveNext();
                        token = rangeTokens.Current;
                        if (token == null) {
                            throw rangeAttr.InvalidXml($"lower bound {range.LowerBound} missing matching upper bound");
                        }
                        if (!int.TryParse(token, out range.UpperBound)) {
                            throw rangeAttr.InvalidXml($"upper bound is not an integer: '{token}'");
                        }
                        rangeTokens.MoveNext();
                        if (range.LowerBound > range.UpperBound) {
                            throw rangeAttr.InvalidXml($"lower bound {range.LowerBound} is greater than upper bound {range.UpperBound}");
                        }
                        ranges.Add(range);
                    } else if (token.StartsWith("$")) {
                        if (!groupDefs.TryGetValue(token, out var groupDef)) {
                            throw rangeAttr.InvalidXml($"undefined group {token}");
                        }
                        rangeTokens.MoveNext();
                        ranges.AddRange(groupDef.Ranges);
                    } else {
                        throw rangeAttr.InvalidXml($"bound is neither an index nor a group name: '{token}'");
                    }
                }
            }

            return new GroupDefinition(entityType, name, ranges);
        }
    }

    public class GroupsDefinition {
        public Type EntityType { get; }

        public IReadOnlyDictionary<string, GroupDefinition> GroupDefinitions { get; }

        public GroupsDefinition(
            Type entityType,
            IEnumerable<GroupDefinition> groupDefinitions = null) {

            EntityType = entityType;
            GroupDefinitions = (groupDefinitions ?? Array.Empty<GroupDefinition>()).ToDictionary(def => def.Name);
        }

        public static GroupsDefinition Parse(XElement element, ModuleMetadata module) {
            var xmlns = ModuleMetadata.XmlNamespace;

            var entityType = module.GetType(element.Attribute("of"));

            var groupDefs = new Dictionary<string, GroupDefinition>();
            foreach (var groupElement in element.Elements(xmlns + "group")) {
                var groupDef = GroupDefinition.Parse(groupElement, entityType, groupDefs);
                if (groupDefs.ContainsKey(groupDef.Name)) {
                    throw groupElement.InvalidXml($"Group {groupDef.Name} is already defined for {entityType.Name}");
                }
                groupDefs.Add(groupDef.Name, groupDef);
            }

            return new GroupsDefinition(entityType, groupDefs.Values);
        }
    }

    public class GroupsDefinitions {
        private readonly Dictionary<Type, GroupsDefinition> _definitions = new Dictionary<Type, GroupsDefinition>();

        public GroupsDefinitions(IEnumerable<GroupsDefinition> definitions) {
            foreach (var def in definitions) {
                _definitions.Add(def.EntityType, def);
            }
        }

        public static GroupsDefinitions Parse(IEnumerable<XElement> elements, ModuleMetadata module) =>
            new GroupsDefinitions(elements.Select(element => GroupsDefinition.Parse(element, module)));

        public GroupsDefinition TryGetGroupsDefinition(Type entityType) =>
            _definitions.TryGetValue(entityType, out var def) ? def : null;

        public GroupsDefinition TryGetGroupsDefinition<TEntity>() =>
            TryGetGroupsDefinition(typeof(TEntity));

        public GroupDefinition TryGetGroupDefinition(Type entityType, string name) =>
            TryGetGroupsDefinition(entityType).GroupDefinitions.TryGetValue(name, out var def) ? def : null;

        public GroupDefinition TryGetGroupDefinition<TEntity>(string name) =>
            TryGetGroupDefinition(typeof(TEntity), name);

        internal GroupDefinition GetGroupDefinition(Type entityType, string name, XObject errObj) {
            var def = TryGetGroupDefinition(entityType, name);
            if (def == null) {
                throw errObj.InvalidXml($"undefined group {name}");
            }
            return def;
        }
    }
}
