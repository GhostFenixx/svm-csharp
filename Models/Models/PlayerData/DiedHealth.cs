namespace Greed.Models.PlayerData
{
    public class DiedHealth
    {
        public bool Saveeffects { get; set; } = true;
        public bool Savehealth { get; set; } = true;
        public double Health_blacked { get; set; } = 0.1;
        public double Health_death { get; set; } = 0.3;
    }
}
