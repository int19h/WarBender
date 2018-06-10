using System.IO;

namespace WarBender.Modules {
    public class Module { 
        public string BasePath { get; }
        public ModuleMetadata Metadata { get; }
        public ModuleSettings Settings { get; }
        public FactionDefinitions Factions { get; }
        public InfoPageDefinitions InfoPages { get; }
        public ItemKindDefinitions ItemKinds { get; }
        public MapIconDefinitions MapIcons { get; }
        public MenuDefinitions Menus { get; }
        public MeshDefinitions Meshes { get; }
        public ParticleSystemDefinitions ParticleSystems { get; }
        public PartyDefinitions Parties { get; }
        public PartyTemplateDefinitions PartyTemplates { get; }
        public QuestDefinitions Quests { get; }
        public SceneDefinitions Scenes { get; }
        public StringDefinitions Strings { get; }
        public TableauMaterialDefinitions TableauMaterials { get; }
        public TroopDefinitions Troops { get; }
        public VariableDefinitions Variables { get; }

        private EntityContainer _entityDefinitions = new EntityContainer();

        public IEntityContainer EntityDefinitions => _entityDefinitions;

        public Module(string basePath, ModuleMetadata metadata = null) {
            BasePath = basePath;
            Metadata = metadata ?? new ModuleMetadata();

            using (var reader = File.OpenText(Path.Combine(basePath, "module.ini"))) {
                Settings = new ModuleSettings(reader);
            }

            using (var reader = File.OpenText(Path.Combine(BasePath, "factions.txt"))) {
                Factions = new FactionDefinitions(reader);
                _entityDefinitions.AddEntities(() => Factions);
            }

            using (var reader = File.OpenText(Path.Combine(BasePath, "info_pages.txt"))) {
                InfoPages = new InfoPageDefinitions(reader);
                _entityDefinitions.AddEntities(() => InfoPages);
            }

            using (var reader = File.OpenText(Path.Combine(BasePath, "item_kinds1.txt"))) {
                ItemKinds = new ItemKindDefinitions(reader);
                _entityDefinitions.AddEntities(() => ItemKinds);
            }

            using (var reader = File.OpenText(Path.Combine(BasePath, "map_icons.txt"))) {
                MapIcons = new MapIconDefinitions(reader);
                _entityDefinitions.AddEntities(() => MapIcons);
            }

            using (var reader = File.OpenText(Path.Combine(BasePath, "menus.txt"))) {
                Menus = new MenuDefinitions(reader);
                _entityDefinitions.AddEntities(() => Menus);
            }

            using (var reader = File.OpenText(Path.Combine(BasePath, "meshes.txt"))) {
                Meshes = new MeshDefinitions(reader);
                _entityDefinitions.AddEntities(() => Meshes);
            }

            using (var reader = File.OpenText(Path.Combine(BasePath, "particle_systems.txt"))) {
                ParticleSystems = new ParticleSystemDefinitions(reader);
                _entityDefinitions.AddEntities(() => ParticleSystems);
            }

            using (var reader = File.OpenText(Path.Combine(BasePath, "parties.txt"))) {
                Parties = new PartyDefinitions(reader);
                _entityDefinitions.AddEntities(() => Parties);
            }

            using (var reader = File.OpenText(Path.Combine(BasePath, "party_templates.txt"))) {
                PartyTemplates = new PartyTemplateDefinitions(reader);
                _entityDefinitions.AddEntities(() => PartyTemplates);
            }

            using (var reader = File.OpenText(Path.Combine(BasePath, "quests.txt"))) {
                Quests = new QuestDefinitions(reader);
                _entityDefinitions.AddEntities(() => Quests);
            }

            using (var reader = File.OpenText(Path.Combine(BasePath, "scenes.txt"))) {
                Scenes = new SceneDefinitions(reader);
                _entityDefinitions.AddEntities(() => Scenes);
            }

            using (var reader = File.OpenText(Path.Combine(BasePath, "strings.txt"))) {
                Strings = new StringDefinitions(reader);
                _entityDefinitions.AddEntities(() => Strings);
            }

            using (var reader = File.OpenText(Path.Combine(BasePath, "tableau_materials.txt"))) {
                TableauMaterials = new TableauMaterialDefinitions(reader);
                _entityDefinitions.AddEntities(() => TableauMaterials);
            }

            using (var reader = File.OpenText(Path.Combine(BasePath, "troops.txt"))) {
                Troops = new TroopDefinitions(reader);
                _entityDefinitions.AddEntities(() => Troops);
            }

            using (var reader = File.OpenText(Path.Combine(BasePath, "variables.txt"))) {
                Variables = new VariableDefinitions(reader);
                _entityDefinitions.AddEntities(() => Variables);
            }
        }

        public EntityDefinitions<T> GetEntityDefinitions<T>()
            where T : EntityDefinition
            => (EntityDefinitions<T>)EntityDefinitions.GetEntities<T>();
    }
}
