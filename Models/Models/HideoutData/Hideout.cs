namespace Greed.Models.HideoutData
{
    public class Hideout : Clamper
    {
        public bool EnableStash { get; set; }
        public Stash Stash { get; set; }
        public Regeneration Regeneration { get; set; }
        public int WaterFilterTime { get; set => SetClamped(ref field, value, 5, 975); } = 325;
        public int BitcoinTime { get; set => SetClamped(ref field, value, 1, 7500); } = 2416;
        public int MaxBitcoins { get; set => SetClamped(ref field, value, 1, 15); } = 3;
        public int WaterFilterRate { get; set => SetClamped(ref field, value, 1, 240); } = 66;
        public int CultistMaxRewards { get; set => SetClamped(ref field, value, 1, 15); } = 5;
        public int MusclePainTime { get; set => SetClamped(ref field, value, 5, 7200); } = 1440;
        public double NoFuelMult { get; set => SetClamped(ref field, value, 0.001, 10); } = 1;
        public double FuelConsumptionRate { get; set => SetClamped(ref field, value, 0.001, 10); } = 1;
        public double ScavCasePrice { get; set => SetClamped(ref field, value, 0.001, 10); } = 1;
        public double ScavCaseTime { get; set => SetClamped(ref field, value, 0.001, 10); } = 1;
        public double HideoutConstMult { get; set => SetClamped(ref field, value, 0.001, 10); } = 1;
        public double HideoutProdMult { get; set => SetClamped(ref field, value, 0.001, 10); } = 1;
        public double GPUBoostRate { get; set => SetClamped(ref field, value, 0.001, 10); } = 1;
        public double CultistTime { get; set => SetClamped(ref field, value, 0.001, 10); } = 1;
        public double AirFilterRate { get; set => SetClamped(ref field, value, 0.001, 10); } = 1;

        public Hideout()
        {
            Stash = new Stash();
            Regeneration = new Regeneration();
        }
        public bool RemoveConstructionsRequirements { get; set; }
        public bool RemoveConstructionsFIRRequirements { get; set; }
        public bool RemoveCustomizationRequirements { get; set; }
        public bool RemoveArenaCrafts { get; set; }
        public bool RemoveSkillRequirements { get; set; }
        public bool RemoveTraderLevelRequirements { get; set; }
        public bool EnableHideout { get; set; }

    }
}
