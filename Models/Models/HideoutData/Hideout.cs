using System.Runtime.ExceptionServices;

namespace Greed.Models.HideoutData
{
    public class Hideout
    {
        public bool EnableStash { get; set; }
        public Stash Stash { get; set; }
        public Regeneration Regeneration { get; set; }
        public int WaterFilterTime { get; set; } = 325;
        public int BitcoinTime { get; set; } = 2416;
        public int MaxBitcoins { get; set; } = 3;
        public double NoFuelMult { get; set; } = 1;
        public double ScavCasePrice { get; set; } = 1;
        public double ScavCaseTime { get; set; } = 1;
        public double HideoutConstMult { get; set; } = 1;
        public double HideoutProdMult { get; set; } = 1;
        public int WaterFilterRate { get; set; } = 66;
        public double GPUBoostRate { get; set; } = 1;
        public double AirFilterRate { get; set; } = 1;
        public double CultistTime { get; set; } = 1;
        public int CultistMaxRewards { get; set; } = 5;
        public bool RemoveConstructionsRequirements { get; set; }
        public bool RemoveConstructionsFIRRequirements { get; set; }
        public bool RemoveSkillRequirements { get; set; }
        public bool RemoveTraderLevelRequirements { get; set; }
        public bool EnableHideout { get; set; }
        public double FuelConsumptionRate { get; set; } = 1;
        public Prestige FirstPrestige { get; set; }
        public Prestige SecondPrestige { get; set; }
        public Prestige ThirdPrestige { get; set; }
        public Prestige FourthPrestige { get; set; }
        public Hideout()
        {
            Stash = new Stash();
            Regeneration = new Regeneration();
            FirstPrestige = new Prestige()
            {
                Height = 3,
                Skills = 5,
                Mastery = 5,
                Filter = false
            };
            SecondPrestige = new Prestige()
            {
                Height = 4,
                Skills = 10,
                Mastery = 10,
                Filter = false
            };
            ThirdPrestige = new Prestige()
            {
                Height = 5,
                Skills = 15,
                Mastery = 15,
                Filter = false
            };
            FourthPrestige = new Prestige()
            {
                Height = 6,
                Skills = 20,
                Mastery = 20,
                Filter = false
            };
        }
    }
}
