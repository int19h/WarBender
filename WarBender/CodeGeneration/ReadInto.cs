


using System.IO;

namespace WarBender {
        
    partial class RecordSerializer : IRecordSerializer<WarBender.Structure.Trigger> {
        void IRecordSerializer<WarBender.Structure.Trigger>.ReadInto(WarBender.Structure.Trigger record, BinaryReader reader, Game game, int index) {
             
                    if (true) {
                         
                                record.status = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.check_timer = _ser_System_Int64.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.delay_timer = _ser_System_Int64.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.rearm_timer = _ser_System_Int64.Read(reader);
                         
                    }
             
        }

        void IRecordSerializer<WarBender.Structure.Trigger>.Write(WarBender.Structure.Trigger record, BinaryWriter writer, Game game, int index) {
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.status);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int64.Write(writer, record.check_timer);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int64.Write(writer, record.delay_timer);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int64.Write(writer, record.rearm_timer);
                         
                    }
             
        }
    }
        
    partial class RecordSerializer : IRecordSerializer<WarBender.Structure.SimpleTrigger> {
        void IRecordSerializer<WarBender.Structure.SimpleTrigger>.ReadInto(WarBender.Structure.SimpleTrigger record, BinaryReader reader, Game game, int index) {
             
                    if (true) {
                         
                                record.check_timer = _ser_System_Int64.Read(reader);
                         
                    }
             
        }

        void IRecordSerializer<WarBender.Structure.SimpleTrigger>.Write(WarBender.Structure.SimpleTrigger record, BinaryWriter writer, Game game, int index) {
             
                    if (true) {
                         
                                _ser_System_Int64.Write(writer, record.check_timer);
                         
                    }
             
        }
    }
        
    partial class RecordSerializer : IRecordSerializer<WarBender.Structure.MapTrack> {
        void IRecordSerializer<WarBender.Structure.MapTrack>.ReadInto(WarBender.Structure.MapTrack record, BinaryReader reader, Game game, int index) {
             
                    if (true) {
                         
                                record.position_x = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.position_y = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.position_z = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.rotation = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.age = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.flags = _ser_System_Int32.Read(reader);
                         
                    }
             
        }

        void IRecordSerializer<WarBender.Structure.MapTrack>.Write(WarBender.Structure.MapTrack record, BinaryWriter writer, Game game, int index) {
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.position_x);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.position_y);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.position_z);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.rotation);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.age);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.flags);
                         
                    }
             
        }
    }
        
    partial class RecordSerializer : IRecordSerializer<WarBender.Structure.Note> {
        void IRecordSerializer<WarBender.Structure.Note>.ReadInto(WarBender.Structure.Note record, BinaryReader reader, Game game, int index) {
             
                    if (true) {
                         
                                record.text = _ser_System_String.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.value = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.tableau_material_id = _ser_WarBender_Structure_TableauMaterialId.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.available = _ser_System_Boolean.Read(reader);
                         
                    }
             
        }

        void IRecordSerializer<WarBender.Structure.Note>.Write(WarBender.Structure.Note record, BinaryWriter writer, Game game, int index) {
             
                    if (true) {
                         
                                _ser_System_String.Write(writer, record.text);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.value);
                         
                    }
             
                    if (true) {
                         
                                _ser_WarBender_Structure_TableauMaterialId.Write(writer, record.tableau_material_id);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Boolean.Write(writer, record.available);
                         
                    }
             
        }
    }
        
    partial class RecordSerializer : IRecordSerializer<WarBender.Structure.InfoPage> {
        void IRecordSerializer<WarBender.Structure.InfoPage>.ReadInto(WarBender.Structure.InfoPage record, BinaryReader reader, Game game, int index) {
             
                    if (true) {
                         
                                 
                                    record.notes.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
        }

        void IRecordSerializer<WarBender.Structure.InfoPage>.Write(WarBender.Structure.InfoPage record, BinaryWriter writer, Game game, int index) {
             
                    if (true) {
                         
                                record.notes.WriteTo(writer, game, -1);
                         
                    }
             
        }
    }
        
    partial class RecordSerializer : IRecordSerializer<WarBender.Structure.Scene> {
        void IRecordSerializer<WarBender.Structure.Scene>.ReadInto(WarBender.Structure.Scene record, BinaryReader reader, Game game, int index) {
             
                    if (true) {
                         
                                 
                                    record.slots.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
        }

        void IRecordSerializer<WarBender.Structure.Scene>.Write(WarBender.Structure.Scene record, BinaryWriter writer, Game game, int index) {
             
                    if (true) {
                         
                                record.slots.WriteTo(writer, game, -1);
                         
                    }
             
        }
    }
        
    partial class RecordSerializer : IRecordSerializer<WarBender.Structure.Quest> {
        void IRecordSerializer<WarBender.Structure.Quest>.ReadInto(WarBender.Structure.Quest record, BinaryReader reader, Game game, int index) {
             
                    if (true) {
                         
                                record.progression = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.giver_troop_id = _ser_WarBender_EntityReference_int_WarBender_Structure_Troop_.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.number = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.start_date = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.title = _ser_System_String.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.text = _ser_System_String.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.giver = _ser_System_String.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                 
                                    record.notes.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
                    if (true) {
                         
                                 
                                    record.slots.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
        }

        void IRecordSerializer<WarBender.Structure.Quest>.Write(WarBender.Structure.Quest record, BinaryWriter writer, Game game, int index) {
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.progression);
                         
                    }
             
                    if (true) {
                         
                                _ser_WarBender_EntityReference_int_WarBender_Structure_Troop_.Write(writer, record.giver_troop_id);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.number);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.start_date);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_String.Write(writer, record.title);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_String.Write(writer, record.text);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_String.Write(writer, record.giver);
                         
                    }
             
                    if (true) {
                         
                                record.notes.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                record.slots.WriteTo(writer, game, -1);
                         
                    }
             
        }
    }
        
    partial class RecordSerializer : IRecordSerializer<WarBender.Structure.Faction> {
        void IRecordSerializer<WarBender.Structure.Faction>.ReadInto(WarBender.Structure.Faction record, BinaryReader reader, Game game, int index) {
             
                    if (true) {
                         
                                 
                                    record.slots.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
                    if (true) {
                         
                                 
                                    record.relations.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
                    if (true) {
                         
                                record.name = _ser_System_String.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.renamed = _ser_System_Boolean.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.color = _ser_System_Drawing_Color.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record._1 = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                 
                                    record.notes.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
        }

        void IRecordSerializer<WarBender.Structure.Faction>.Write(WarBender.Structure.Faction record, BinaryWriter writer, Game game, int index) {
             
                    if (true) {
                         
                                record.slots.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                record.relations.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_String.Write(writer, record.name);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Boolean.Write(writer, record.renamed);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Drawing_Color.Write(writer, record.color);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record._1);
                         
                    }
             
                    if (true) {
                         
                                record.notes.WriteTo(writer, game, -1);
                         
                    }
             
        }
    }
        
    partial class RecordSerializer : IRecordSerializer<WarBender.Structure.PartyTemplate> {
        void IRecordSerializer<WarBender.Structure.PartyTemplate>.ReadInto(WarBender.Structure.PartyTemplate record, BinaryReader reader, Game game, int index) {
             
                    if (true) {
                         
                                record.num_parties_created = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.num_parties_destroyed = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.num_parties_destroyed_by_player = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                 
                                    record.slots.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
        }

        void IRecordSerializer<WarBender.Structure.PartyTemplate>.Write(WarBender.Structure.PartyTemplate record, BinaryWriter writer, Game game, int index) {
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.num_parties_created);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.num_parties_destroyed);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.num_parties_destroyed_by_player);
                         
                    }
             
                    if (true) {
                         
                                record.slots.WriteTo(writer, game, -1);
                         
                    }
             
        }
    }
        
    partial class RecordSerializer : IRecordSerializer<WarBender.Structure.TroopStack> {
        void IRecordSerializer<WarBender.Structure.TroopStack>.ReadInto(WarBender.Structure.TroopStack record, BinaryReader reader, Game game, int index) {
             
                    if (true) {
                         
                                record.troop_id = _ser_WarBender_EntityReference_int_WarBender_Structure_Troop_.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.num_troops = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.num_wounded_troops = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.flags = _ser_System_Int32.Read(reader);
                         
                    }
             
        }

        void IRecordSerializer<WarBender.Structure.TroopStack>.Write(WarBender.Structure.TroopStack record, BinaryWriter writer, Game game, int index) {
             
                    if (true) {
                         
                                _ser_WarBender_EntityReference_int_WarBender_Structure_Troop_.Write(writer, record.troop_id);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.num_troops);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.num_wounded_troops);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.flags);
                         
                    }
             
        }
    }
        
    partial class RecordSerializer : IRecordSerializer<WarBender.Structure.Party> {
        void IRecordSerializer<WarBender.Structure.Party>.ReadInto(WarBender.Structure.Party record, BinaryReader reader, Game game, int index) {
             
                    if (true) {
                         
                                record.raw_id = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.id = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.party_id = _ser_System_String.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.name = _ser_System_String.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.flags = _ser_WarBender_Structure_PartyFlags.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.menu_id = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.party_template_id = _ser_WarBender_EntityReference_int_WarBender_Structure_PartyTemplate_.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.faction_id = _ser_WarBender_EntityReference_int_WarBender_Structure_Faction_.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.personality = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.default_behavior = _ser_WarBender_Structure_AIBehavior.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.current_behavior = _ser_WarBender_Structure_AIBehavior.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.default_behavior_object_id = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.current_behavior_object_id = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.initial_position_x = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.initial_position_y = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.target_position_x = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.target_position_y = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.position_x = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.position_y = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.position_z = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                 
                                    record.stacks.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
                    if (true) {
                         
                                record.bearing = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.renamed = _ser_System_Boolean.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.extra_text = _ser_System_String.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.morale = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.hunger = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record._1 = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.patrol_radius = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.initiative = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.helpfulness = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.label_visible = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.bandit_attraction = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true && game.MatchVersions(900, 999, 1020, 2147483647)) {
                         
                                record.marshall = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.ignore_player_timer = _ser_System_Int64.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.banner_map_icon_id = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true && game.MatchVersions(1137, 2147483647)) {
                         
                                record.extra_map_icon_id = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true && game.MatchVersions(1137, 2147483647)) {
                         
                                record.extra_map_icon_up_down_distance = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true && game.MatchVersions(1137, 2147483647)) {
                         
                                record.extra_map_icon_up_down_frequency = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true && game.MatchVersions(1137, 2147483647)) {
                         
                                record.extra_map_icon_rotate_frequency = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true && game.MatchVersions(1137, 2147483647)) {
                         
                                record.extra_map_icon_fade_frequency = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.attached_to_party_id = _ser_WarBender_EntityReference_int_WarBender_Structure_Party_.Read(reader);
                         
                    }
             
                    if (true && game.MatchVersions(1162, 2147483647)) {
                         
                                record._2 = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.is_attached = _ser_System_Boolean.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                 
                                    record.attached_party_ids.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
                    if (true) {
                         
                                 
                                    record.particle_system_ids.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
                    if (true) {
                         
                                 
                                    record.notes.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
                    if (true) {
                         
                                 
                                    record.slots.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
        }

        void IRecordSerializer<WarBender.Structure.Party>.Write(WarBender.Structure.Party record, BinaryWriter writer, Game game, int index) {
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.raw_id);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.id);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_String.Write(writer, record.party_id);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_String.Write(writer, record.name);
                         
                    }
             
                    if (true) {
                         
                                _ser_WarBender_Structure_PartyFlags.Write(writer, record.flags);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.menu_id);
                         
                    }
             
                    if (true) {
                         
                                _ser_WarBender_EntityReference_int_WarBender_Structure_PartyTemplate_.Write(writer, record.party_template_id);
                         
                    }
             
                    if (true) {
                         
                                _ser_WarBender_EntityReference_int_WarBender_Structure_Faction_.Write(writer, record.faction_id);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.personality);
                         
                    }
             
                    if (true) {
                         
                                _ser_WarBender_Structure_AIBehavior.Write(writer, record.default_behavior);
                         
                    }
             
                    if (true) {
                         
                                _ser_WarBender_Structure_AIBehavior.Write(writer, record.current_behavior);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.default_behavior_object_id);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.current_behavior_object_id);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.initial_position_x);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.initial_position_y);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.target_position_x);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.target_position_y);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.position_x);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.position_y);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.position_z);
                         
                    }
             
                    if (true) {
                         
                                record.stacks.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.bearing);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Boolean.Write(writer, record.renamed);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_String.Write(writer, record.extra_text);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.morale);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.hunger);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record._1);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.patrol_radius);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.initiative);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.helpfulness);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.label_visible);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.bandit_attraction);
                         
                    }
             
                    if (true && game.MatchVersions(900, 999, 1020, 2147483647)) {
                         
                                _ser_System_Int32.Write(writer, record.marshall);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int64.Write(writer, record.ignore_player_timer);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.banner_map_icon_id);
                         
                    }
             
                    if (true && game.MatchVersions(1137, 2147483647)) {
                         
                                _ser_System_Int32.Write(writer, record.extra_map_icon_id);
                         
                    }
             
                    if (true && game.MatchVersions(1137, 2147483647)) {
                         
                                _ser_System_Single.Write(writer, record.extra_map_icon_up_down_distance);
                         
                    }
             
                    if (true && game.MatchVersions(1137, 2147483647)) {
                         
                                _ser_System_Single.Write(writer, record.extra_map_icon_up_down_frequency);
                         
                    }
             
                    if (true && game.MatchVersions(1137, 2147483647)) {
                         
                                _ser_System_Single.Write(writer, record.extra_map_icon_rotate_frequency);
                         
                    }
             
                    if (true && game.MatchVersions(1137, 2147483647)) {
                         
                                _ser_System_Single.Write(writer, record.extra_map_icon_fade_frequency);
                         
                    }
             
                    if (true) {
                         
                                _ser_WarBender_EntityReference_int_WarBender_Structure_Party_.Write(writer, record.attached_to_party_id);
                         
                    }
             
                    if (true && game.MatchVersions(1162, 2147483647)) {
                         
                                _ser_System_Int32.Write(writer, record._2);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Boolean.Write(writer, record.is_attached);
                         
                    }
             
                    if (true) {
                         
                                record.attached_party_ids.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                record.particle_system_ids.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                record.notes.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                record.slots.WriteTo(writer, game, -1);
                         
                    }
             
        }
    }
        
    partial class RecordSerializer : IRecordSerializer<WarBender.Structure.PlayerPartyTroopStack> {
        void IRecordSerializer<WarBender.Structure.PlayerPartyTroopStack>.ReadInto(WarBender.Structure.PlayerPartyTroopStack record, BinaryReader reader, Game game, int index) {
             
                    if (true) {
                         
                                record.experience = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.num_upgradeable = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true && record.has_troop_dnas(game, index)) {
                         
                                 
                                    record.troop_dnas.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
        }

        void IRecordSerializer<WarBender.Structure.PlayerPartyTroopStack>.Write(WarBender.Structure.PlayerPartyTroopStack record, BinaryWriter writer, Game game, int index) {
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.experience);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.num_upgradeable);
                         
                    }
             
                    if (true && record.has_troop_dnas(game, index)) {
                         
                                record.troop_dnas.WriteTo(writer, game, -1);
                         
                    }
             
        }
    }
        
    partial class RecordSerializer : IRecordSerializer<WarBender.Structure.MapEvent> {
        void IRecordSerializer<WarBender.Structure.MapEvent>.ReadInto(WarBender.Structure.MapEvent record, BinaryReader reader, Game game, int index) {
             
                    if (true) {
                         
                                record.id = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.text = _ser_System_String.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.type = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.position_x = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.position_y = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.land_position_x = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.land_position_y = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record._1 = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record._2 = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.attacker_party_id = _ser_WarBender_EntityReference_int_WarBender_Structure_Party_.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.defender_party_id = _ser_WarBender_EntityReference_int_WarBender_Structure_Party_.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.battle_simulation_timer = _ser_System_Int64.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.next_battle_simulation = _ser_System_Single.Read(reader);
                         
                    }
             
        }

        void IRecordSerializer<WarBender.Structure.MapEvent>.Write(WarBender.Structure.MapEvent record, BinaryWriter writer, Game game, int index) {
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.id);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_String.Write(writer, record.text);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.type);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.position_x);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.position_y);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.land_position_x);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.land_position_y);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record._1);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record._2);
                         
                    }
             
                    if (true) {
                         
                                _ser_WarBender_EntityReference_int_WarBender_Structure_Party_.Write(writer, record.attacker_party_id);
                         
                    }
             
                    if (true) {
                         
                                _ser_WarBender_EntityReference_int_WarBender_Structure_Party_.Write(writer, record.defender_party_id);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int64.Write(writer, record.battle_simulation_timer);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.next_battle_simulation);
                         
                    }
             
        }
    }
        
    partial class RecordSerializer : IRecordSerializer<WarBender.Structure.ItemKind> {
        void IRecordSerializer<WarBender.Structure.ItemKind>.ReadInto(WarBender.Structure.ItemKind record, BinaryReader reader, Game game, int index) {
             
                    if (true) {
                         
                                 
                                    record.slots.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
        }

        void IRecordSerializer<WarBender.Structure.ItemKind>.Write(WarBender.Structure.ItemKind record, BinaryWriter writer, Game game, int index) {
             
                    if (true) {
                         
                                record.slots.WriteTo(writer, game, -1);
                         
                    }
             
        }
    }
        
    partial class RecordSerializer : IRecordSerializer<WarBender.Structure.Item> {
        void IRecordSerializer<WarBender.Structure.Item>.ReadInto(WarBender.Structure.Item record, BinaryReader reader, Game game, int index) {
             
                    if (true) {
                         
                                record.item_kind_id = _ser_WarBender_EntityReference_int_WarBender_Structure_ItemKind_.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.hit_points = _ser_System_UInt16.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record._1 = _ser_System_Byte.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.modifier = _ser_WarBender_Structure_ItemModifier.Read(reader);
                         
                    }
             
        }

        void IRecordSerializer<WarBender.Structure.Item>.Write(WarBender.Structure.Item record, BinaryWriter writer, Game game, int index) {
             
                    if (true) {
                         
                                _ser_WarBender_EntityReference_int_WarBender_Structure_ItemKind_.Write(writer, record.item_kind_id);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_UInt16.Write(writer, record.hit_points);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Byte.Write(writer, record._1);
                         
                    }
             
                    if (true) {
                         
                                _ser_WarBender_Structure_ItemModifier.Write(writer, record.modifier);
                         
                    }
             
        }
    }
        
    partial class RecordSerializer : IRecordSerializer<WarBender.Structure.EquippedItems> {
        void IRecordSerializer<WarBender.Structure.EquippedItems>.ReadInto(WarBender.Structure.EquippedItems record, BinaryReader reader, Game game, int index) {
             
                    if (true) {
                         
                                 
                                    var value = new WarBender.Structure.Item();
                                    value.ReadFrom(reader, game, -1);
                                    record.item_0 = value;
                                 
                         
                    }
             
                    if (true) {
                         
                                 
                                    var value = new WarBender.Structure.Item();
                                    value.ReadFrom(reader, game, -1);
                                    record.item_1 = value;
                                 
                         
                    }
             
                    if (true) {
                         
                                 
                                    var value = new WarBender.Structure.Item();
                                    value.ReadFrom(reader, game, -1);
                                    record.item_2 = value;
                                 
                         
                    }
             
                    if (true) {
                         
                                 
                                    var value = new WarBender.Structure.Item();
                                    value.ReadFrom(reader, game, -1);
                                    record.item_3 = value;
                                 
                         
                    }
             
                    if (true) {
                         
                                 
                                    var value = new WarBender.Structure.Item();
                                    value.ReadFrom(reader, game, -1);
                                    record.head = value;
                                 
                         
                    }
             
                    if (true) {
                         
                                 
                                    var value = new WarBender.Structure.Item();
                                    value.ReadFrom(reader, game, -1);
                                    record.body = value;
                                 
                         
                    }
             
                    if (true) {
                         
                                 
                                    var value = new WarBender.Structure.Item();
                                    value.ReadFrom(reader, game, -1);
                                    record.foot = value;
                                 
                         
                    }
             
                    if (true) {
                         
                                 
                                    var value = new WarBender.Structure.Item();
                                    value.ReadFrom(reader, game, -1);
                                    record.gloves = value;
                                 
                         
                    }
             
                    if (true) {
                         
                                 
                                    var value = new WarBender.Structure.Item();
                                    value.ReadFrom(reader, game, -1);
                                    record.horse = value;
                                 
                         
                    }
             
                    if (true) {
                         
                                 
                                    var value = new WarBender.Structure.Item();
                                    value.ReadFrom(reader, game, -1);
                                    record.food = value;
                                 
                         
                    }
             
        }

        void IRecordSerializer<WarBender.Structure.EquippedItems>.Write(WarBender.Structure.EquippedItems record, BinaryWriter writer, Game game, int index) {
             
                    if (true) {
                         
                                record.item_0.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                record.item_1.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                record.item_2.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                record.item_3.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                record.head.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                record.body.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                record.foot.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                record.gloves.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                record.horse.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                record.food.WriteTo(writer, game, -1);
                         
                    }
             
        }
    }
        
    partial class RecordSerializer : IRecordSerializer<WarBender.Structure.Inventory> {
        void IRecordSerializer<WarBender.Structure.Inventory>.ReadInto(WarBender.Structure.Inventory record, BinaryReader reader, Game game, int index) {
             
                    if (true) {
                         
                                record.gold = _ser_System_UInt32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.experience = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.health = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.faction_id = _ser_WarBender_EntityReference_int_WarBender_Structure_Faction_.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                 
                                    record.inventory_items.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
                    if (true) {
                         
                                 
                                    var value = new WarBender.Structure.EquippedItems();
                                    value.ReadFrom(reader, game, -1);
                                    record.equipped_items = value;
                                 
                         
                    }
             
                    if (true) {
                         
                                 
                                    record.face_keys.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
        }

        void IRecordSerializer<WarBender.Structure.Inventory>.Write(WarBender.Structure.Inventory record, BinaryWriter writer, Game game, int index) {
             
                    if (true) {
                         
                                _ser_System_UInt32.Write(writer, record.gold);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.experience);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.health);
                         
                    }
             
                    if (true) {
                         
                                _ser_WarBender_EntityReference_int_WarBender_Structure_Faction_.Write(writer, record.faction_id);
                         
                    }
             
                    if (true) {
                         
                                record.inventory_items.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                record.equipped_items.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                record.face_keys.WriteTo(writer, game, -1);
                         
                    }
             
        }
    }
        
    partial class RecordSerializer : IRecordSerializer<WarBender.Structure.Attributes> {
        void IRecordSerializer<WarBender.Structure.Attributes>.ReadInto(WarBender.Structure.Attributes record, BinaryReader reader, Game game, int index) {
             
                    if (true) {
                         
                                record.strength = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.agility = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.intelligence = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.charisma = _ser_System_Int32.Read(reader);
                         
                    }
             
        }

        void IRecordSerializer<WarBender.Structure.Attributes>.Write(WarBender.Structure.Attributes record, BinaryWriter writer, Game game, int index) {
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.strength);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.agility);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.intelligence);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.charisma);
                         
                    }
             
        }
    }
        
    partial class RecordSerializer : IRecordSerializer<WarBender.Structure.Proficiencies> {
        void IRecordSerializer<WarBender.Structure.Proficiencies>.ReadInto(WarBender.Structure.Proficiencies record, BinaryReader reader, Game game, int index) {
             
                    if (true) {
                         
                                record.one_handed_weapon = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.two_handed_weapon = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.polearm = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.archery = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.crossbow = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.throwing = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.firearm = _ser_System_Single.Read(reader);
                         
                    }
             
        }

        void IRecordSerializer<WarBender.Structure.Proficiencies>.Write(WarBender.Structure.Proficiencies record, BinaryWriter writer, Game game, int index) {
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.one_handed_weapon);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.two_handed_weapon);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.polearm);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.archery);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.crossbow);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.throwing);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.firearm);
                         
                    }
             
        }
    }
        
    partial class RecordSerializer : IRecordSerializer<WarBender.Structure.Skills> {
        void IRecordSerializer<WarBender.Structure.Skills>.ReadInto(WarBender.Structure.Skills record, BinaryReader reader, Game game, int index) {
             
                    if (true) {
                         
                                 
                                    record._skills.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
        }

        void IRecordSerializer<WarBender.Structure.Skills>.Write(WarBender.Structure.Skills record, BinaryWriter writer, Game game, int index) {
             
                    if (true) {
                         
                                record._skills.WriteTo(writer, game, -1);
                         
                    }
             
        }
    }
        
    partial class RecordSerializer : IRecordSerializer<WarBender.Structure.Troop> {
        void IRecordSerializer<WarBender.Structure.Troop>.ReadInto(WarBender.Structure.Troop record, BinaryReader reader, Game game, int index) {
             
                    if (true) {
                         
                                 
                                    record.slots.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
                    if (true) {
                         
                                 
                                    var value = new WarBender.Structure.Attributes();
                                    value.ReadFrom(reader, game, -1);
                                    record.attributes = value;
                                 
                         
                    }
             
                    if (true) {
                         
                                 
                                    var value = new WarBender.Structure.Proficiencies();
                                    value.ReadFrom(reader, game, -1);
                                    record.proficiencies = value;
                                 
                         
                    }
             
                    if (true) {
                         
                                 
                                    var value = new WarBender.Structure.Skills();
                                    value.ReadFrom(reader, game, -1);
                                    record.skills = value;
                                 
                         
                    }
             
                    if (true) {
                         
                                 
                                    record.notes.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
                    if (true) {
                         
                                record.flags = _ser_WarBender_Structure_TroopFlags.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.site_id_and_entry_no = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.skill_points = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.attribute_points = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.proficiency_points = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.level = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true && record.has_inventory(game, index)) {
                         
                                 
                                    var value = new WarBender.Structure.Inventory();
                                    value.ReadFrom(reader, game, -1);
                                    record.inventory = value;
                                 
                         
                    }
             
                    if (true && record.has_inventory(game, index)) {
                         
                                record.renamed = _ser_System_Boolean.Read(reader);
                         
                    }
             
                    if (true && record.renamed) {
                         
                                record.name = _ser_System_String.Read(reader);
                         
                    }
             
                    if (true && record.renamed) {
                         
                                record.name_plural = _ser_System_String.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.class_no = _ser_System_Int32.Read(reader);
                         
                    }
             
        }

        void IRecordSerializer<WarBender.Structure.Troop>.Write(WarBender.Structure.Troop record, BinaryWriter writer, Game game, int index) {
             
                    if (true) {
                         
                                record.slots.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                record.attributes.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                record.proficiencies.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                record.skills.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                record.notes.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                _ser_WarBender_Structure_TroopFlags.Write(writer, record.flags);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.site_id_and_entry_no);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.skill_points);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.attribute_points);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.proficiency_points);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.level);
                         
                    }
             
                    if (true && record.has_inventory(game, index)) {
                         
                                record.inventory.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true && record.has_inventory(game, index)) {
                         
                                _ser_System_Boolean.Write(writer, record.renamed);
                         
                    }
             
                    if (true && record.renamed) {
                         
                                _ser_System_String.Write(writer, record.name);
                         
                    }
             
                    if (true && record.renamed) {
                         
                                _ser_System_String.Write(writer, record.name_plural);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.class_no);
                         
                    }
             
        }
    }
        
    partial class RecordSerializer : IRecordSerializer<WarBender.Structure.GameInfo> {
        void IRecordSerializer<WarBender.Structure.GameInfo>.ReadInto(WarBender.Structure.GameInfo record, BinaryReader reader, Game game, int index) {
             
                    if (true) {
                         
                                record.magic_number = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.game_version = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.module_version = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.savegame_name = _ser_System_String.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.player_name = _ser_System_String.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.player_level = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.date = _ser_System_Single.Read(reader);
                         
                    }
             
        }

        void IRecordSerializer<WarBender.Structure.GameInfo>.Write(WarBender.Structure.GameInfo record, BinaryWriter writer, Game game, int index) {
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.magic_number);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.game_version);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.module_version);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_String.Write(writer, record.savegame_name);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_String.Write(writer, record.player_name);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.player_level);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.date);
                         
                    }
             
        }
    }
        
    partial class RecordSerializer : IRecordSerializer<WarBender.Structure.GameData> {
        void IRecordSerializer<WarBender.Structure.GameData>.ReadInto(WarBender.Structure.GameData record, BinaryReader reader, Game game, int index) {
             
                    if (true) {
                         
                                 
                                    var value = new WarBender.Structure.GameInfo();
                                    value.ReadFrom(reader, game, -1);
                                    record.header = value;
                                 
                         
                    }
             
                    if (true) {
                         
                                record.game_time = _ser_System_UInt64.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.random_seed = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.save_mode = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true && game.MatchVersions(1137, 2147483647)) {
                         
                                record.combat_difficulty = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true && game.MatchVersions(1137, 2147483647)) {
                         
                                record.combat_difficulty_friendlies = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true && game.MatchVersions(1137, 2147483647)) {
                         
                                record.reduce_combat_ai = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true && game.MatchVersions(1137, 2147483647)) {
                         
                                record.reduce_campaign_ai = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true && game.MatchVersions(1137, 2147483647)) {
                         
                                record.combat_speed = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.date_timer = _ser_System_Int64.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.hour = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.day = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.week = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.month = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.year = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record._1 = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.global_cloud_amount = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.global_haze_amount = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.average_difficulty = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.average_difficulty_period = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record._2 = _ser_System_String.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record._3 = _ser_System_Boolean.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.tutorial_flags = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.default_prisoner_price = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.encountered_party_1_id = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.encountered_party_2_id = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.current_menu_id = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.current_site_id = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.current_entry_no = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.current_mission_template_id = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.party_creation_min_random_value = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.party_creation_max_random_value = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.game_log = _ser_System_String.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                 
                                    record._4.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
                    if (true) {
                         
                                record._5 = _ser_System_Int64.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.rest_period = _ser_System_Single.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.rest_time_speed = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.rest_is_interactive = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.rest_remain_attackable = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                 
                                    record.class_names.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
                    if (true) {
                         
                                 
                                    record.global_variables.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
                    if (true) {
                         
                                 
                                    record.triggers.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
                    if (true) {
                         
                                 
                                    record.simple_trigers.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
                    if (true) {
                         
                                 
                                    record.quests.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
                    if (true) {
                         
                                 
                                    record.info_pages.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
                    if (true) {
                         
                                 
                                    record.scenes.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
                    if (true) {
                         
                                 
                                    record.factions.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
                    if (true) {
                         
                                 
                                    record.map_tracks.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
                    if (true) {
                         
                                 
                                    record.party_templates.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
                    if (true) {
                         
                                 
                                    record.parties.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
                    if (true) {
                         
                                 
                                    record.player_party_stacks.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
                    if (true) {
                         
                                 
                                    record.map_events.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
                    if (true) {
                         
                                 
                                    record.troops.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
                    if (true) {
                         
                                 
                                    record._6.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
                    if (true) {
                         
                                 
                                    record.item_kinds.ReadFrom(reader, game, -1);
                                 
                         
                    }
             
                    if (true) {
                         
                                record.player_face_keys0 = _ser_System_UInt64.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.player_face_keys1 = _ser_System_UInt64.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.player_kill_count = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.player_wounded_count = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.player_own_troop_kill_count = _ser_System_Int32.Read(reader);
                         
                    }
             
                    if (true) {
                         
                                record.player_own_troop_wounded_count = _ser_System_Int32.Read(reader);
                         
                    }
             
        }

        void IRecordSerializer<WarBender.Structure.GameData>.Write(WarBender.Structure.GameData record, BinaryWriter writer, Game game, int index) {
             
                    if (true) {
                         
                                record.header.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_UInt64.Write(writer, record.game_time);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.random_seed);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.save_mode);
                         
                    }
             
                    if (true && game.MatchVersions(1137, 2147483647)) {
                         
                                _ser_System_Int32.Write(writer, record.combat_difficulty);
                         
                    }
             
                    if (true && game.MatchVersions(1137, 2147483647)) {
                         
                                _ser_System_Int32.Write(writer, record.combat_difficulty_friendlies);
                         
                    }
             
                    if (true && game.MatchVersions(1137, 2147483647)) {
                         
                                _ser_System_Int32.Write(writer, record.reduce_combat_ai);
                         
                    }
             
                    if (true && game.MatchVersions(1137, 2147483647)) {
                         
                                _ser_System_Int32.Write(writer, record.reduce_campaign_ai);
                         
                    }
             
                    if (true && game.MatchVersions(1137, 2147483647)) {
                         
                                _ser_System_Int32.Write(writer, record.combat_speed);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int64.Write(writer, record.date_timer);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.hour);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.day);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.week);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.month);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.year);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record._1);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.global_cloud_amount);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.global_haze_amount);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.average_difficulty);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.average_difficulty_period);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_String.Write(writer, record._2);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Boolean.Write(writer, record._3);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.tutorial_flags);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.default_prisoner_price);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.encountered_party_1_id);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.encountered_party_2_id);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.current_menu_id);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.current_site_id);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.current_entry_no);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.current_mission_template_id);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.party_creation_min_random_value);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.party_creation_max_random_value);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_String.Write(writer, record.game_log);
                         
                    }
             
                    if (true) {
                         
                                record._4.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int64.Write(writer, record._5);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Single.Write(writer, record.rest_period);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.rest_time_speed);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.rest_is_interactive);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.rest_remain_attackable);
                         
                    }
             
                    if (true) {
                         
                                record.class_names.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                record.global_variables.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                record.triggers.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                record.simple_trigers.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                record.quests.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                record.info_pages.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                record.scenes.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                record.factions.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                record.map_tracks.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                record.party_templates.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                record.parties.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                record.player_party_stacks.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                record.map_events.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                record.troops.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                record._6.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                record.item_kinds.WriteTo(writer, game, -1);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_UInt64.Write(writer, record.player_face_keys0);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_UInt64.Write(writer, record.player_face_keys1);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.player_kill_count);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.player_wounded_count);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.player_own_troop_kill_count);
                         
                    }
             
                    if (true) {
                         
                                _ser_System_Int32.Write(writer, record.player_own_troop_wounded_count);
                         
                    }
             
        }
    }
     

    partial class RecordSerializer {
         
                 
                private static readonly IValueSerializer<int> _ser_System_Int32 = ValueSerializer.Get<int>();
         
                 
                private static readonly IValueSerializer<long> _ser_System_Int64 = ValueSerializer.Get<long>();
         
                 
                private static readonly IValueSerializer<float> _ser_System_Single = ValueSerializer.Get<float>();
         
                 
                private static readonly IValueSerializer<string> _ser_System_String = ValueSerializer.Get<string>();
         
                 
                private static readonly IValueSerializer<WarBender.Structure.TableauMaterialId> _ser_WarBender_Structure_TableauMaterialId = ValueSerializer.Get<WarBender.Structure.TableauMaterialId>();
         
                 
                private static readonly IValueSerializer<bool> _ser_System_Boolean = ValueSerializer.Get<bool>();
         
                 
                private static readonly IValueSerializer<WarBender.EntityReference<int, WarBender.Structure.Troop>> _ser_WarBender_EntityReference_int_WarBender_Structure_Troop_ = ValueSerializer.Get<WarBender.EntityReference<int, WarBender.Structure.Troop>>();
         
                 
                private static readonly IValueSerializer<System.Drawing.Color> _ser_System_Drawing_Color = ValueSerializer.Get<System.Drawing.Color>();
         
                 
                private static readonly IValueSerializer<WarBender.Structure.PartyFlags> _ser_WarBender_Structure_PartyFlags = ValueSerializer.Get<WarBender.Structure.PartyFlags>();
         
                 
                private static readonly IValueSerializer<WarBender.EntityReference<int, WarBender.Structure.PartyTemplate>> _ser_WarBender_EntityReference_int_WarBender_Structure_PartyTemplate_ = ValueSerializer.Get<WarBender.EntityReference<int, WarBender.Structure.PartyTemplate>>();
         
                 
                private static readonly IValueSerializer<WarBender.EntityReference<int, WarBender.Structure.Faction>> _ser_WarBender_EntityReference_int_WarBender_Structure_Faction_ = ValueSerializer.Get<WarBender.EntityReference<int, WarBender.Structure.Faction>>();
         
                 
                private static readonly IValueSerializer<WarBender.Structure.AIBehavior> _ser_WarBender_Structure_AIBehavior = ValueSerializer.Get<WarBender.Structure.AIBehavior>();
         
                 
                private static readonly IValueSerializer<WarBender.EntityReference<int, WarBender.Structure.Party>> _ser_WarBender_EntityReference_int_WarBender_Structure_Party_ = ValueSerializer.Get<WarBender.EntityReference<int, WarBender.Structure.Party>>();
         
                 
                private static readonly IValueSerializer<WarBender.EntityReference<int, WarBender.Structure.ItemKind>> _ser_WarBender_EntityReference_int_WarBender_Structure_ItemKind_ = ValueSerializer.Get<WarBender.EntityReference<int, WarBender.Structure.ItemKind>>();
         
                 
                private static readonly IValueSerializer<ushort> _ser_System_UInt16 = ValueSerializer.Get<ushort>();
         
                 
                private static readonly IValueSerializer<byte> _ser_System_Byte = ValueSerializer.Get<byte>();
         
                 
                private static readonly IValueSerializer<WarBender.Structure.ItemModifier> _ser_WarBender_Structure_ItemModifier = ValueSerializer.Get<WarBender.Structure.ItemModifier>();
         
                 
                private static readonly IValueSerializer<uint> _ser_System_UInt32 = ValueSerializer.Get<uint>();
         
                 
                private static readonly IValueSerializer<WarBender.Structure.TroopFlags> _ser_WarBender_Structure_TroopFlags = ValueSerializer.Get<WarBender.Structure.TroopFlags>();
         
                 
                private static readonly IValueSerializer<ulong> _ser_System_UInt64 = ValueSerializer.Get<ulong>();
         

        static partial void GetRecordTypesHash(ref int hash) =>
            hash = 830646719;
    }

     
             
            partial class ValueSerializer : IValueSerializer<WarBender.Structure.TableauMaterialId> {
                private static readonly WarBender.EnumSerializer<WarBender.Structure.TableauMaterialId> _ser__0 = new WarBender.EnumSerializer<WarBender.Structure.TableauMaterialId>();

                WarBender.Structure.TableauMaterialId IValueSerializer<WarBender.Structure.TableauMaterialId>.Read(BinaryReader reader) => _ser__0.Read(reader);
                    
                void IValueSerializer<WarBender.Structure.TableauMaterialId>.Write(BinaryWriter writer, WarBender.Structure.TableauMaterialId value) => _ser__0.Write(writer, value);
            }
     
             
            partial class ValueSerializer : IValueSerializer<WarBender.EntityReference<int, WarBender.Structure.Troop>> {
                private static readonly WarBender.EntityReferenceSerializer<int, WarBender.Structure.Troop> _ser__1 = new WarBender.EntityReferenceSerializer<int, WarBender.Structure.Troop>();

                WarBender.EntityReference<int, WarBender.Structure.Troop> IValueSerializer<WarBender.EntityReference<int, WarBender.Structure.Troop>>.Read(BinaryReader reader) => _ser__1.Read(reader);
                    
                void IValueSerializer<WarBender.EntityReference<int, WarBender.Structure.Troop>>.Write(BinaryWriter writer, WarBender.EntityReference<int, WarBender.Structure.Troop> value) => _ser__1.Write(writer, value);
            }
     
             
            partial class ValueSerializer : IValueSerializer<WarBender.Structure.PartyFlags> {
                private static readonly WarBender.EnumSerializer<WarBender.Structure.PartyFlags> _ser__2 = new WarBender.EnumSerializer<WarBender.Structure.PartyFlags>();

                WarBender.Structure.PartyFlags IValueSerializer<WarBender.Structure.PartyFlags>.Read(BinaryReader reader) => _ser__2.Read(reader);
                    
                void IValueSerializer<WarBender.Structure.PartyFlags>.Write(BinaryWriter writer, WarBender.Structure.PartyFlags value) => _ser__2.Write(writer, value);
            }
     
             
            partial class ValueSerializer : IValueSerializer<WarBender.EntityReference<int, WarBender.Structure.PartyTemplate>> {
                private static readonly WarBender.EntityReferenceSerializer<int, WarBender.Structure.PartyTemplate> _ser__3 = new WarBender.EntityReferenceSerializer<int, WarBender.Structure.PartyTemplate>();

                WarBender.EntityReference<int, WarBender.Structure.PartyTemplate> IValueSerializer<WarBender.EntityReference<int, WarBender.Structure.PartyTemplate>>.Read(BinaryReader reader) => _ser__3.Read(reader);
                    
                void IValueSerializer<WarBender.EntityReference<int, WarBender.Structure.PartyTemplate>>.Write(BinaryWriter writer, WarBender.EntityReference<int, WarBender.Structure.PartyTemplate> value) => _ser__3.Write(writer, value);
            }
     
             
            partial class ValueSerializer : IValueSerializer<WarBender.EntityReference<int, WarBender.Structure.Faction>> {
                private static readonly WarBender.EntityReferenceSerializer<int, WarBender.Structure.Faction> _ser__4 = new WarBender.EntityReferenceSerializer<int, WarBender.Structure.Faction>();

                WarBender.EntityReference<int, WarBender.Structure.Faction> IValueSerializer<WarBender.EntityReference<int, WarBender.Structure.Faction>>.Read(BinaryReader reader) => _ser__4.Read(reader);
                    
                void IValueSerializer<WarBender.EntityReference<int, WarBender.Structure.Faction>>.Write(BinaryWriter writer, WarBender.EntityReference<int, WarBender.Structure.Faction> value) => _ser__4.Write(writer, value);
            }
     
             
            partial class ValueSerializer : IValueSerializer<WarBender.Structure.AIBehavior> {
                private static readonly WarBender.EnumSerializer<WarBender.Structure.AIBehavior> _ser__5 = new WarBender.EnumSerializer<WarBender.Structure.AIBehavior>();

                WarBender.Structure.AIBehavior IValueSerializer<WarBender.Structure.AIBehavior>.Read(BinaryReader reader) => _ser__5.Read(reader);
                    
                void IValueSerializer<WarBender.Structure.AIBehavior>.Write(BinaryWriter writer, WarBender.Structure.AIBehavior value) => _ser__5.Write(writer, value);
            }
     
             
            partial class ValueSerializer : IValueSerializer<WarBender.EntityReference<int, WarBender.Structure.Party>> {
                private static readonly WarBender.EntityReferenceSerializer<int, WarBender.Structure.Party> _ser__6 = new WarBender.EntityReferenceSerializer<int, WarBender.Structure.Party>();

                WarBender.EntityReference<int, WarBender.Structure.Party> IValueSerializer<WarBender.EntityReference<int, WarBender.Structure.Party>>.Read(BinaryReader reader) => _ser__6.Read(reader);
                    
                void IValueSerializer<WarBender.EntityReference<int, WarBender.Structure.Party>>.Write(BinaryWriter writer, WarBender.EntityReference<int, WarBender.Structure.Party> value) => _ser__6.Write(writer, value);
            }
     
             
            partial class ValueSerializer : IValueSerializer<WarBender.Structure.ParticleSystemId> {
                private static readonly WarBender.EnumSerializer<WarBender.Structure.ParticleSystemId> _ser__7 = new WarBender.EnumSerializer<WarBender.Structure.ParticleSystemId>();

                WarBender.Structure.ParticleSystemId IValueSerializer<WarBender.Structure.ParticleSystemId>.Read(BinaryReader reader) => _ser__7.Read(reader);
                    
                void IValueSerializer<WarBender.Structure.ParticleSystemId>.Write(BinaryWriter writer, WarBender.Structure.ParticleSystemId value) => _ser__7.Write(writer, value);
            }
     
             
            partial class ValueSerializer : IValueSerializer<WarBender.EntityReference<int, WarBender.Structure.ItemKind>> {
                private static readonly WarBender.EntityReferenceSerializer<int, WarBender.Structure.ItemKind> _ser__8 = new WarBender.EntityReferenceSerializer<int, WarBender.Structure.ItemKind>();

                WarBender.EntityReference<int, WarBender.Structure.ItemKind> IValueSerializer<WarBender.EntityReference<int, WarBender.Structure.ItemKind>>.Read(BinaryReader reader) => _ser__8.Read(reader);
                    
                void IValueSerializer<WarBender.EntityReference<int, WarBender.Structure.ItemKind>>.Write(BinaryWriter writer, WarBender.EntityReference<int, WarBender.Structure.ItemKind> value) => _ser__8.Write(writer, value);
            }
     
             
            partial class ValueSerializer : IValueSerializer<WarBender.Structure.ItemModifier> {
                private static readonly WarBender.EnumSerializer<WarBender.Structure.ItemModifier> _ser__9 = new WarBender.EnumSerializer<WarBender.Structure.ItemModifier>();

                WarBender.Structure.ItemModifier IValueSerializer<WarBender.Structure.ItemModifier>.Read(BinaryReader reader) => _ser__9.Read(reader);
                    
                void IValueSerializer<WarBender.Structure.ItemModifier>.Write(BinaryWriter writer, WarBender.Structure.ItemModifier value) => _ser__9.Write(writer, value);
            }
     
             
            partial class ValueSerializer : IValueSerializer<WarBender.Structure.TroopFlags> {
                private static readonly WarBender.EnumSerializer<WarBender.Structure.TroopFlags> _ser__10 = new WarBender.EnumSerializer<WarBender.Structure.TroopFlags>();

                WarBender.Structure.TroopFlags IValueSerializer<WarBender.Structure.TroopFlags>.Read(BinaryReader reader) => _ser__10.Read(reader);
                    
                void IValueSerializer<WarBender.Structure.TroopFlags>.Write(BinaryWriter writer, WarBender.Structure.TroopFlags value) => _ser__10.Write(writer, value);
            }
     
}
