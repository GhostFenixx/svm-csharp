namespace Greed.Models.Questing
{
    public class DailyQuests
    {
        public int Types { get; set; }
        public int Reroll { get; set; }
        public LevelRanges LR1 { get; set; }
        public LevelRanges LR2 { get; set; }
        public LevelRanges LR3 { get; set; }
        public int Access { get; set; }
        public int QuestAmount { get; set; }
        public int Lifespan { get; set; }
        public string Levels { get; set; }
        public string Experience { get; set; }
        public string ItemsReward { get; set; }
        public string Reputation { get; set; }
        public string SkillPoint { get; set; }
        public string SkillChance { get; set; }
        public string Roubles { get; set; }
        public string GPcoins { get; set; }

    }
}
