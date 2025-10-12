namespace Greed.Models.Looting
{
    public class Locations
    {
        public LootOnLocations Streets { get; set; }
        public LootOnLocations Sandbox { get; set; }
        public LootOnLocations SandboxHard { get; set; }
        public LootOnLocations Lighthouse { get; set; }
        public LootOnLocations Bigmap { get; set; }
        public LootOnLocations Interchange { get; set; }
        public LootOnLocations FactoryDay { get; set; }
        public LootOnLocations Laboratory { get; set; }
        public LootOnLocations Shoreline { get; set; }
        public LootOnLocations Reserve { get; set; }
        public LootOnLocations Woods { get; set; }
        public LootOnLocations FactoryNight { get; set; }
        public bool AllContainers { get; set; }
        public Locations()
        {
            Streets = new LootOnLocations()
            {
                Loose = 3,
                Container = 1
            };
            Sandbox = new LootOnLocations()
            {
                Loose = 2.8,
                Container = 1
            };
            SandboxHard = new LootOnLocations()
            {
                Loose = 2.8,
                Container = 1
            };
            Lighthouse = new LootOnLocations()
            {
                Loose = 2.8,
                Container = 1
            };
            Bigmap = new LootOnLocations()
            {
                Loose = 2.5,
                Container = 1
            };
            Interchange = new LootOnLocations()
            {
                Loose = 2.8,
                Container = 1
            };
            FactoryDay = new LootOnLocations()
            {
                Loose = 3.5,
                Container = 1
            };
            Laboratory = new LootOnLocations()
            {
                Loose = 2.8,
                Container = 1
            };
            Shoreline = new LootOnLocations()
            {
                Loose = 3.7,
                Container = 1
            };
            Reserve = new LootOnLocations()
            {
                Loose = 2.9,
                Container = 1
            };
            Woods = new LootOnLocations()
            {
                Loose = 1.9,
                Container = 1
            };
            FactoryNight = new LootOnLocations()
            {
                Loose = 3.5,
                Container = 1
            };
        }
    }
}
