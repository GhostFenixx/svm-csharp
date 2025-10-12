namespace Greed.Models.Looting
{
    public class Loot
    {
        public Airdrops Airdrops { get; set; }
        public bool EnableLoot { get; set; }
        public Locations Locations { get; set; }

        public Loot()
        {
            Airdrops = new Airdrops();
            Locations = new Locations();
        }
    }
}
