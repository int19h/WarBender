using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WarBender.GameData;
using WarBender.Modules;

namespace WarBender.UI {
    internal class ModelGetters {
        public Game Game { get; set; }

        public bool CanExpand(object obj) =>
            obj is SavedGame ||
            obj is GroupsDefinition ||
            obj is GroupDefinition ||
            obj is ICollection;
        
        public object GetName(object obj) {
            var n = (obj as IRecord)?.Index ?? -1;
            switch (obj) {
                case SavedGame data:
                    var header = data.header;
                    var day = (int)(header.date / 24) + 1;
                    return $"{header.savegame_name} (day {day})";
                case GroupsDefinition groupsDef:
                    return "Groups";
                case GroupDefinition groupDef:
                    //var ranges = groupDef.Ranges.Select(range => $"{range.LowerBound}..{range.UpperBound}");
                    //return $"{groupDef.Name} ({string.Join(", ", ranges)})";
                    return groupDef.Name;
                case IHasName hasName:
                    var name = hasName.Name;
                    if (obj is Party party && party.IsPresent && string.IsNullOrEmpty(name)) {
                        name = party.party_template_id.Entity.Name;
                        if (name != null) {
                            name = "<" + name + ">";
                        }
                    }
                    if (string.IsNullOrEmpty(name)) {
                        if (obj is IRecord record) {
                            return record.Index;
                        }
                    }
                    return name;
                case IHasId hasId:
                    return hasId.Id;
                case IRecord record:
                    return record.Index;
                case ICollection coll:
                    return CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(FriendlyNames.Plural(coll.ItemType));
                default:
                    return null;
            }
        }

        public string GetToolTip(object obj) {
            if (obj is SavedGame data) {
                return data.Game.FileName;
            } else if (obj is IRecord record) {
                var s = record.Index.ToString();
                if (obj is IHasId hasId) {
                    s = $"{hasId.Id} ({s})";
                }
                return s;
            } else {
                return FriendlyNames.Describe(obj);
            }
        }

        public string GetImage(object obj) {
            switch (obj) {
                case GroupsDefinition _:
                    return "Folder";
                case GroupDefinition groupDef:
                    return "Folder";
                case ICollection collection:
                    return collection.ItemType.Name + "Folder";
                case IRecord record:
                    return record.Type.Name;
                default:
                    return null;
            }
        }

        public IEnumerable<object> GetChildren(object obj) {
            switch (obj) {
                case SavedGame data:
                    return new object[] {
                        data.factions,
                        data.info_pages,
                        data.item_kinds,
                        data.map_events,
                        data.map_tracks,
                        data.parties,
                        data.party_templates,
                        data.quests,
                        data.scenes,
                        data.simple_triggers,
                        data.triggers,
                        data.troops,
                    };
                case GroupsDefinition groupsDef:
                    return groupsDef.GroupDefinitions.Values;
                case GroupDefinition groupDef:
                    var entities = Game.Entities.GetEntities(groupDef.EntityType);
                    return groupDef.Indices.Where(i => i >= 0 && i < entities.Count).Select(i => entities[i]);
                case ICollection col:
                    var children = (IEnumerable<object>)col;
                    // TODO: TreeListView doesn't handle duplicate model items in different branches well.
                    //var defs = col.TryGame()?.Module.Metadata.GroupsDefinitions.TryGetGroupsDefinition(col.ItemType);
                    //if (defs != null) {
                    //    children = new object[] { defs }.Concat(children);
                    //}
                    return children;
                default:
                    throw new ArgumentException($"Cannot retrieve children for {obj}", nameof(obj));
            }
        }
    }
}
