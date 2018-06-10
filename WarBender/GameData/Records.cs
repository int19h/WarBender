using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using WarBender.Modules;

namespace WarBender.GameData {
    using MapIconId = EntityReference<MapIconDefinition>;
    using MenuId = EntityReference<MenuDefinition>;
    using ParticleSystemId = EntityReference<ParticleSystemDefinition>;
    using TableauMaterialId = EntityReference<TableauMaterialDefinition>;

    using FactionId = EntityReference<Faction>;
    using ItemKindId = EntityReference<ItemKind>;
    using PartyId = EntityReference<Party>;
    using PartyTemplateId = EntityReference<PartyTemplate>;
    using TroopId = EntityReference<Troop>;

    public abstract partial class Trigger : Record<Trigger> {
        public abstract int status { get; set; }
        public abstract long check_timer { get; set; }
        public abstract long delay_timer { get; set; }
        public abstract long rearm_timer { get; set; }
    }

    public abstract partial class SimpleTrigger : Record<SimpleTrigger> {
        public abstract long check_timer { get; set; }
    }

    public abstract partial class MapTrack : Record<MapTrack> {
        public abstract float position_x { get; set; }
        public abstract float position_y { get; set; }
        public abstract float position_z { get; set; }
        public abstract float rotation { get; set; }
        public abstract float age { get; set; }
        public abstract int flags { get; set; }
    }

    public abstract partial class Note : Record<Note> {
        public abstract string text { get; set; }
        public abstract int value { get; set; }
        public abstract TableauMaterialId tableau_material_id { get; set; }
        public abstract bool available { get; set; }
    }

    public abstract partial class InfoPage : EntityRecord<InfoPage, InfoPageDefinition>, IHasId, IHasName {
        public FixedLengthCollection<Note> notes { get; } = 16;

        string IHasId.Id => Definition?.Id;

        [Computed]
        [ParenthesizePropertyName(true)]
        public string Name => Definition?.Name;
    }

    public abstract partial class Scene : EntityRecord<Scene, SceneDefinition>, IHasId {
        public Slots slots { get; } = new Slots();

        string IHasId.Id => Definition?.Id;
    }

    public abstract partial class Quest : EntityRecord<Quest, QuestDefinition>, IHasId {
        public abstract int progression { get; set; }
        public abstract TroopId giver_troop_id { get; set; }
        public abstract int number { get; set; }
        public abstract float start_date { get; set; }
        public abstract string title { get; set; }
        public abstract string text { get; set; }
        public abstract string giver { get; set; }
        public FixedLengthCollection<Note> notes { get; } = 16;
        public Slots slots { get; } = new Slots();

        string IHasId.Id => Definition?.Id;
    }

    public abstract partial class Faction : EntityRecord<Faction, FactionDefinition>, IHasId, IHasName {
        public Slots slots { get; } = new Slots();
        public ForEachOf<Faction, float> relations { get; } = new ForEachOf<Faction, float>();
        public abstract string name { get; set; }
        public abstract bool renamed { get; set; }
        public abstract Color color { get; set; }
        private int _1 { get; set; }
        public FixedLengthCollection<Note> notes { get; } = 16;

        string IHasId.Id => Definition?.Id;
        string IHasName.Name => name;
    }

    public abstract partial class PartyTemplate : EntityRecord<PartyTemplate, PartyTemplateDefinition>, IHasId, IHasName {
        public abstract int num_parties_created { get; set; }
        public abstract int num_parties_destroyed { get; set; }
        public abstract int num_parties_destroyed_by_player { get; set; }
        public Slots slots { get; } = new Slots();

        string IHasId.Id => Definition?.Id;

        [Computed]
        [ParenthesizePropertyName(true)]
        public string Name => Definition?.Name;
    }

    public abstract partial class TroopStack : Record<TroopStack> {
        public abstract TroopId troop_id { get; set; }
        public abstract int num_troops { get; set; }
        public abstract int num_wounded_troops { get; set; }
        public abstract int flags { get; set; }

        [Computed]
        public PlayerPartyTroopStack player_party_info {
            get {
                var data = this.Game().Data;
                var ppstacks = data.player_party_troop_stacks;
                int i = 0;
                foreach (var stack in (Collection<TroopStack>)Parent) {
                    if (stack.has_player_party_info()) {
                        if (i == Index) {
                            return data.player_party_troop_stacks[i];
                        }
                        ++i;
                    }
                }
                return null;
            }
        }

        internal bool has_player_party_info() =>
            this.Ancestor<Party>().Index == 0 &&
            !this.Game().Module.Troops[troop_id].Flags.HasFlag(TroopFlags.tf_hero);
    }

    public abstract partial class Party : OptionalEntityRecord<Party, PartyDefinition>, IHasId, IHasName {
        public abstract int raw_id { get; set; }
        public abstract int id { get; set; }
        public abstract string party_id { get; set; }
        public abstract string name { get; set; }
        public abstract PartyFlags flags { get; set; }
        public abstract MenuId menu_id { get; set; }
        public abstract PartyTemplateId party_template_id { get; set; }
        public abstract FactionId? faction_id { get; set; }
        public abstract int personality { get; set; }
        public abstract AIBehavior? default_behavior { get; set; }
        public abstract AIBehavior current_behavior { get; set; }
        public abstract int default_behavior_object_id { get; set; }
        public abstract int current_behavior_object_id { get; set; }
        public abstract float initial_position_x { get; set; }
        public abstract float initial_position_y { get; set; }
        public abstract float target_position_x { get; set; }
        public abstract float target_position_y { get; set; }
        public abstract float position_x { get; set; }
        public abstract float position_y { get; set; }
        public abstract float position_z { get; set; }
        public LengthPrefixedCollection<TroopStack> stacks { get; } = new LengthPrefixedCollection<TroopStack>();
        public abstract float bearing { get; set; }
        public abstract bool renamed { get; set; }
        public abstract string extra_text { get; set; }
        public abstract float morale { get; set; }
        public abstract float hunger { get; set; }
        private float _1 { get; set; }
        public abstract float patrol_radius { get; set; }
        public abstract float initiative { get; set; }
        public abstract float helpfulness { get; set; }
        public abstract int label_visible { get; set; }
        public abstract float bandit_attraction { get; set; }

        [GameVersion(900, 999)]
        [GameVersion(1020)]
        public abstract int marshall { get; set; }

        public abstract long ignore_player_timer { get; set; }
        public abstract MapIconId banner_map_icon_id { get; set; }

        [GameVersion(1137)]
        public abstract MapIconId extra_map_icon_id { get; set; }
        [GameVersion(1137)]
        public abstract float extra_map_icon_up_down_distance { get; set; }
        [GameVersion(1137)]
        public abstract float extra_map_icon_up_down_frequency { get; set; }
        [GameVersion(1137)]
        public abstract float extra_map_icon_rotate_frequency { get; set; }
        [GameVersion(1137)]
        public abstract float extra_map_icon_fade_frequency { get; set; }

        public abstract PartyId attached_to_party_id { get; set; }

        [GameVersion(1162)]
        private int _2 { get; set; }

        public abstract bool is_attached { get; set; }
        public LengthPrefixedCollection<PartyId> attached_party_ids { get; } = new LengthPrefixedCollection<PartyId>();
        public LengthPrefixedCollection<ParticleSystemId> particle_system_ids { get; } = new LengthPrefixedCollection<ParticleSystemId>();
        public FixedLengthCollection<Note> notes { get; } = 16;
        public Slots slots { get; } = new Slots();

        string IHasName.Name => name;
        string IHasId.Id => Definition?.Id;
    }

    public abstract partial class PlayerPartyTroopStack : Record<PlayerPartyTroopStack> {
        public abstract float experience { get; set; }
        public abstract int num_upgradeable { get; set; }

        private bool has_troop_dnas() => Index < 32;

        [ConditionalOn(nameof(has_troop_dnas))]
        public FixedLengthCollection<int> troop_dnas { get; } = 32;
    }

    public abstract partial class MapEvent : OptionalRecord<MapEvent> {
        public abstract int id { get; set; }
        public abstract string text { get; set; }
        public abstract int type { get; set; }
        public abstract float position_x { get; set; }
        public abstract float position_y { get; set; }
        public abstract float land_position_x { get; set; }
        public abstract float land_position_y { get; set; }
        private float _1 { get; set; }
        private float _2 { get; set; }
        public abstract PartyId attacker_party_id { get; set; }
        public abstract PartyId defender_party_id { get; set; }
        public abstract long battle_simulation_timer { get; set; }
        public abstract float next_battle_simulation { get; set; }
    }

    public partial class ItemKind : EntityRecord<ItemKind, ItemKindDefinition>, IHasId, IHasName {
        public Slots slots { get; } = new Slots();

        string IHasId.Id => Definition?.Id;

        [Computed]
        [ParenthesizePropertyName(true)]
        public string Name => Definition?.Name;
    }

    public abstract partial class Item : Record<Item> {
        public abstract ItemKindId item_kind_id { get; set; }
        public abstract ushort hit_points { get; set; }
        private byte _1 { get; set; }
        public abstract ItemModifier modifier { get; set; }
    }

    public abstract partial class EquippedItems : Record<EquippedItems> {
        public abstract Item item_0 { get; set; }
        public abstract Item item_1 { get; set; }
        public abstract Item item_2 { get; set; }
        public abstract Item item_3 { get; set; }
        public abstract Item head { get; set; }
        public abstract Item body { get; set; }
        public abstract Item foot { get; set; }
        public abstract Item gloves { get; set; }
        public abstract Item horse { get; set; }
        public abstract Item food { get; set; }
    }

    public abstract partial class Inventory : Record<Inventory> {
        public abstract uint gold { get; set; }
        public abstract int experience { get; set; }
        public abstract float health { get; set; }
        public abstract FactionId faction_id { get; set; }
        public FixedLengthCollection<Item> inventory_items { get; } = 96;
        public abstract EquippedItems equipped_items { get; set; }
        public FixedLengthCollection<ulong> face_keys { get; } = 4;
    }

    public abstract partial class Attributes : Record<Attributes> {
        public abstract int strength { get; set; }
        public abstract int agility { get; set; }
        public abstract int intelligence { get; set; }
        public abstract int charisma { get; set; }
    }

    public abstract partial class Proficiencies : Record<Proficiencies> {
        public abstract float one_handed_weapon { get; set; }
        public abstract float two_handed_weapon { get; set; }
        public abstract float polearm { get; set; }
        public abstract float archery { get; set; }
        public abstract float crossbow { get; set; }
        public abstract float throwing { get; set; }
        public abstract float firearm { get; set; }
    }

    public abstract partial class Skills : Record<Skills> {

        [BitField(4)]
        public abstract byte trade { get; set; }

        [BitField(4)]
        public abstract byte leadership { get; set; }

        [BitField(4)]
        public abstract byte prisoner_management { get; set; }

        [BitField(4)]
        public abstract byte reserved_1 { get; set; }

        [BitField(4)]
        public abstract byte reserved_2 { get; set; }

        [BitField(4)]
        public abstract byte reserved_3 { get; set; }

        [BitField(4)]
        public abstract byte reserved_4 { get; set; }

        [BitField(4)]
        public abstract byte persuasion { get; set; }

        [BitField(4)]
        public abstract byte engineer { get; set; }

        [BitField(4)]
        public abstract byte first_aid { get; set; }

        [BitField(4)]
        public abstract byte surgery { get; set; }

        [BitField(4)]
        public abstract byte wound_treatment { get; set; }

        [BitField(4)]
        public abstract byte inventory_management { get; set; }

        [BitField(4)]
        public abstract byte spotting { get; set; }

        [BitField(4)]
        public abstract byte pathfinding { get; set; }

        [BitField(4)]
        public abstract byte tactics { get; set; }

        [BitField(4)]
        public abstract byte tracking { get; set; }

        [BitField(4)]
        public abstract byte trainer { get; set; }

        [BitField(4)]
        public abstract byte reserved_5 { get; set; }

        [BitField(4)]
        public abstract byte reserved_6 { get; set; }

        [BitField(4)]
        public abstract byte reserved_7 { get; set; }

        [BitField(4)]
        public abstract byte reserved_8 { get; set; }

        [BitField(4)]
        public abstract byte looting { get; set; }

        [BitField(4)]
        public abstract byte horse_archery { get; set; }

        [BitField(4)]
        public abstract byte riding { get; set; }

        [BitField(4)]
        public abstract byte athletics { get; set; }

        [BitField(4)]
        public abstract byte shield { get; set; }

        [BitField(4)]
        public abstract byte weapon_master { get; set; }

        [BitField(4)]
        public abstract byte reserved_9 { get; set; }

        [BitField(4)]
        public abstract byte reserved_10 { get; set; }

        [BitField(4)]
        public abstract byte reserved_11 { get; set; }

        [BitField(4)]
        public abstract byte reserved_12 { get; set; }

        [BitField(4)]
        public abstract byte reserved_13 { get; set; }

        [BitField(4)]
        public abstract byte power_draw { get; set; }

        [BitField(4)]
        public abstract byte power_throw { get; set; }

        [BitField(4)]
        public abstract byte power_strike { get; set; }

        [BitField(4)]
        public abstract byte ironflesh { get; set; }

        [BitField(4)]
        public abstract byte reserved_14 { get; set; }

        [BitField(4)]
        public abstract byte reserved_15 { get; set; }

        [BitField(4)]
        public abstract byte reserved_16 { get; set; }

        [BitField(4)]
        public abstract byte reserved_17 { get; set; }

        [BitField(4)]
        public abstract byte reserved_18 { get; set; }

        private FixedLengthCollection<byte> _1 { get; } = 3;
    }

    public abstract partial class Troop : EntityRecord<Troop, TroopDefinition>, IHasId, IHasName {
        public Slots slots { get; } = new Slots();
        public abstract Attributes attributes { get; set; }
        public abstract Proficiencies proficiencies { get; set; }
        public abstract Skills skills { get; set; }
        public FixedLengthCollection<Note> notes { get; } = 16;
        public abstract TroopFlags flags { get; set; }
        public abstract int site_id_and_entry_no { get; set; }
        public abstract int skill_points { get; set; }
        public abstract int attribute_points { get; set; }
        public abstract int proficiency_points { get; set; }
        public abstract int level { get; set; }

        private bool has_inventory() =>
            !this.Game().Module.Settings.DontLoadRegularTroopInventories ||
            (flags & TroopFlags.tf_hero) != 0;

        [ConditionalOn(nameof(has_inventory))]
        public abstract Inventory inventory { get; set; }

        [ConditionalOn(nameof(has_inventory))]
        public abstract bool renamed { get; set; }

        [ConditionalOn(nameof(renamed))]
        public abstract string name { get; set; }

        [ConditionalOn(nameof(renamed))]
        public abstract string name_plural { get; set; }

        public abstract int class_no { get; set; }

        [Computed]
        [Browsable(false)]
        public string Id => Definition?.Id;

        [Computed]
        [ParenthesizePropertyName(true)]
        public string Name => renamed ? name : Definition?.Name;
    }

    public abstract partial class GameHeader : Record<GameHeader> {
        public abstract int magic_number { get; set; }
        public abstract int game_version { get; set; }
        public abstract int module_version { get; set; }
        public abstract string savegame_name { get; set; }
        public abstract string player_name { get; set; }
        public abstract int player_level { get; set; }
        public abstract float date { get; set; }
    }

    public class VariableCollection : LengthPrefixedCollection<long> {
        public override string GetKeyOfIndex(int index) {
            var vars = this.TryGame()?.Module.Variables;
            if (vars != null && index >= 0 && index < vars.Count) {
                return vars[index].Id;
            }
            return base.GetKeyOfIndex(index);
        }
    }

    public abstract partial class SavedGame : Record<SavedGame>, IRootDataObject {
        public abstract GameHeader header { get; set; }
        public abstract ulong game_time { get; set; }
        public abstract int random_seed { get; set; }
        public abstract int save_mode { get; set; }

        [GameVersion(1137)]
        public abstract int combat_difficulty { get; set; }
        [GameVersion(1137)]
        public abstract int combat_difficulty_friendlies { get; set; }
        [GameVersion(1137)]
        public abstract int reduce_combat_ai { get; set; }
        [GameVersion(1137)]
        public abstract int reduce_campaign_ai { get; set; }
        [GameVersion(1137)]
        public abstract int combat_speed { get; set; }

        public abstract long date_timer { get; set; }
        public abstract int hour { get; set; }
        public abstract int day { get; set; }
        public abstract int week { get; set; }
        public abstract int month { get; set; }
        public abstract int year { get; set; }
        private int _1 { get; set; }
        public abstract float global_cloud_amount { get; set; }
        public abstract float global_haze_amount { get; set; }
        public abstract float average_difficulty { get; set; }
        public abstract float average_difficulty_period { get; set; }
        private string _2 { get; set; }
        private bool _3 { get; set; }
        public abstract int tutorial_flags { get; set; }
        public abstract int default_prisoner_price { get; set; }
        public abstract int encountered_party_1_id { get; set; }
        public abstract int encountered_party_2_id { get; set; }
        public abstract MenuId current_menu_id { get; set; }
        public abstract int current_site_id { get; set; }
        public abstract int current_entry_no { get; set; }
        public abstract int current_mission_template_id { get; set; }
        public abstract int party_creation_min_random_value { get; set; }
        public abstract int party_creation_max_random_value { get; set; }
        public abstract string game_log { get; set; } // TODO: multiline?
        private FixedLengthCollection<int> _4 { get; } = 6;
        private long _5 { get; set; }
        public abstract float rest_period { get; set; }
        public abstract int rest_time_speed { get; set; }
        public abstract int rest_is_interactive { get; set; }
        public abstract int rest_remain_attackable { get; set; }
        public FixedLengthCollection<string> class_names { get; } = 9;
        public LengthPrefixedCollection<long> global_variables { get; } = new VariableCollection();
        public LengthPrefixedCollection<Trigger> triggers { get; } = new LengthPrefixedCollection<Trigger>();
        public LengthPrefixedCollection<SimpleTrigger> simple_triggers { get; } = new LengthPrefixedCollection<SimpleTrigger>();
        public LengthPrefixedCollection<Quest> quests { get; } = new LengthPrefixedCollection<Quest>();
        public LengthPrefixedCollection<InfoPage> info_pages { get; } = new LengthPrefixedCollection<InfoPage>();
        public LengthPrefixedCollection<Scene> scenes { get; } = new LengthPrefixedCollection<Scene>();
        public LengthPrefixedCollection<Faction> factions { get; } = new LengthPrefixedCollection<Faction>();
        public LengthPrefixedCollection<MapTrack> map_tracks { get; } = new LengthPrefixedCollection<MapTrack>();
        public LengthPrefixedCollection<PartyTemplate> party_templates { get; } = new LengthPrefixedCollection<PartyTemplate>();
        public DynamicCollection<Party> parties { get; } = new DynamicCollection<Party>();
        internal ComputedLengthCollection<PlayerPartyTroopStack> player_party_troop_stacks { get; }
        public DynamicCollection<MapEvent> map_events { get; } = new DynamicCollection<MapEvent>();
        public LengthPrefixedCollection<Troop> troops { get; } = new LengthPrefixedCollection<Troop>();
        private FixedLengthCollection<int> _6 { get; } = 42;
        public LengthPrefixedCollection<ItemKind> item_kinds { get; } = new LengthPrefixedCollection<ItemKind>();
        public abstract ulong player_face_keys0 { get; set; }
        public abstract ulong player_face_keys1 { get; set; }
        public abstract int player_kill_count { get; set; }
        public abstract int player_wounded_count { get; set; }
        public abstract int player_own_troop_kill_count { get; set; }
        public abstract int player_own_troop_wounded_count { get; set; }

        public SavedGame() {
            player_party_troop_stacks = new ComputedLengthCollection<PlayerPartyTroopStack>(game => {
                var len = parties[0].stacks.Where(stack => stack.has_player_party_info()).Count();
                return len;
            });
        }

        private Game _game;

        [Computed]
        [Browsable(false)]
        public Game Game {
            get => _game;
            internal set {
                if (_game != null) {
                    throw new InvalidOperationException($"{nameof(Game)} can only be set once");
                }
                _game = value;
            }
        }

        // We need to be able to create the root object directly to start deserialization.
        // However, Create() is generated code, and we need to be able to bootstrap the
        // build without it. This adds an overload that will only ever be selected if the
        // proper parameterless Create() hasn't been generated yet.
        internal static SavedGame Create(int dummy = 0) => throw new NotImplementedException();
    }
}
