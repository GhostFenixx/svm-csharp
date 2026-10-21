using Greed.Models.PlayerData;
namespace Greed.Models.HideoutData
{

    public class Prestige
    {
        public bool EnablePrestige { get; set; }
        public bool PrestigeCollector { get; set; }
        public bool PrestigeNewBeginnings { get; set; }
        public bool PrestigeAreas { get; set; }
        //public int PrestigeLevel { get; set; } = 55;
        //public int PrestigeStrength { get; set; } = 20;
        //public int PrestigeEndurance { get; set; } = 20;
        //public int PrestigeCharisma { get; set; } = 15;
        //public int PrestigeCurrency { get; set; } = 20000000;
        public PrestigeLevel FirstPrestige { get; set; }
        public PrestigeLevel SecondPrestige { get; set; }
        public PrestigeLevel ThirdPrestige { get; set; }
        public PrestigeLevel FourthPrestige { get; set; }
        public PrestigeLevel FifthPrestige { get; set; }
        public PrestigeLevel SixthPrestige { get; set; }
        public Prestige()
        {
            FirstPrestige = new PrestigeLevel()
            {
                Height = 3,
                Skills = 5,
                Mastery = 5,
                Filter = false
            };
            SecondPrestige = new PrestigeLevel()
            {
                Height = 4,
                Skills = 10,
                Mastery = 10,
                Filter = false
            };
            ThirdPrestige = new PrestigeLevel()
            {
                Height = 5,
                Skills = 15,
                Mastery = 15,
                Filter = false
            };
            FourthPrestige = new PrestigeLevel()
            {
                Height = 6,
                Skills = 20,
                Mastery = 20,
                Filter = false
            };
            FifthPrestige = new PrestigeLevel()
            {
                Height = 6,
                Skills = 20,
                Mastery = 20,
                Filter = false
            };
            SixthPrestige = new PrestigeLevel()
            {
                Height = 6,
                Skills = 20,
                Mastery = 20,
                Filter = false
            };
        }
    }
    public class PrestigeLevel
    {
        public int Level { get; set; }
        public int Strength { get; set; }
        public int Endurance { get; set; }
        public int Charisma { get; set; }
        public int Currency { get; set; }
        public bool Areas { get; set; }
        public int Height { get; set; }
        public bool Filter { get; set; }
        public int Skills { get; set; }
        public int Mastery { get; set; }
    }
}