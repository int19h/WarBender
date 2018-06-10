using System;
using System.Collections;
using System.Collections.Generic;
using WarBender.GameData;
using WarBender.Modules;

namespace WarBender.UI {
    internal static class FriendlyNames {
        private static readonly Dictionary<Type, (string singular, string plural)> _names = new Dictionary<Type, (string, string)> {
            [typeof(int)] = ("number", "numbers"),
            [typeof(long)] = ("number", "numbers"),
            [typeof(float)] = ("number", "numbers"),
            [typeof(string)] = ("string", "strings"),
            [typeof(Faction)] = ("faction", "factions"),
            [typeof(GroupDefinition)] = ("group", "groups"),
            [typeof(InfoPage)] = ("info page", "info pages"),
            [typeof(Item)] = ("item", "items"),
            [typeof(ItemKind)] = ("item kind", "item kinds"),
            [typeof(MapEvent)] = ("map event", "map events"),
            [typeof(MapTrack)] = ("map track", "map tracks"),
            [typeof(Note)] = ("note", "notes"),
            [typeof(Party)] = ("party", "parties"),
            [typeof(PartyTemplate)] = ("party template", "party templates"),
            [typeof(Quest)] = ("quest", "quests"),
            [typeof(Scene)] = ("scene", "scenes"),
            [typeof(SimpleTrigger)] = ("simple trigger", "simple triggers"),
            [typeof(Trigger)] = ("trigger", "triggers"),
            [typeof(Troop)] = ("troop", "troops"),
            [typeof(TroopStack)] = ("stack", "stacks"),
        };

        private static (string singular, string plural) Of(Type type) {
            if (type == null) {
                return ("nothing", "nothing");
            }
            if (_names.TryGetValue(type, out var names)) {
                return names;
            }
            return ("object", "objects");
        }

        public static string Singular(Type type) => Of(type).singular;

        public static string Singular<T>() => Singular(typeof(T));

        public static string Plural(Type type) => Of(type).plural;

        public static string Plural<T>() => Plural(typeof(T));

        public static string Describe(object obj) =>
            obj is IEnumerable objs && !(obj is string) ? Describe(objs) :
            Singular(obj is IRecord record ? record.Type : obj?.GetType());

        public static string Describe(IEnumerable objects) {
            Type commonType = null;
            int count = 0;
            foreach (var obj in objects) {
                ++count;

                var type = obj is IRecord record ? record.Type : obj.GetType();
                if (commonType == null) {
                    commonType = type;
                    continue;
                }

                bool found = false;
                for (var t = commonType; t != null; t = t.BaseType) {
                    if (t.IsAssignableFrom(type)) {
                        found = true;
                        commonType = t;
                        break;
                    }
                }

                if (!found) {
                    commonType = typeof(object);
                }
            }

            return count == 0
                ? "none"
                : $"{count} {(count == 1 ? Singular(commonType) : Plural(commonType))}";
        }
    }
}
