namespace Greed.Models.HideoutData
{
    public class Regeneration :Clamper
    {
        public bool OfflineRegen { get; set; }
        public double HealthRegen { get; set => SetClamped(ref field, value, 0, 10); } = 1;
        public bool HideoutHealth { get; set; }
        public bool HideoutEnergy { get; set; }
        public bool HideoutHydration { get; set; }
        public double HydrationRegen { get; set => SetClamped(ref field, value, 0, 10); } = 1;
        public double EnergyRegen { get; set => SetClamped(ref field, value, 0, 10); } = 1;
    }
}
