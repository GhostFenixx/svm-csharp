namespace Greed.Models.HideoutData
{
    public class Regeneration
    {
        public bool OfflineRegen { get; set; }
        public double HealthRegen { get; set; } = 1;
        public bool HideoutHealth { get; set; }
        public bool HideoutEnergy { get; set; }
        public bool HideoutHydration { get; set; }
        public double HydrationRegen { get; set; } = 1;
        public double EnergyRegen { get; set; } = 1;
    }
}
