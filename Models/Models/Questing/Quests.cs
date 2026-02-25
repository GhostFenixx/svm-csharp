namespace Greed.Models.Questing
{
    public class Quests
    {
        public bool EnableQuestsMisc { get; set; }
        public double QuestCostMult { get; set; } = 1;
        public bool QuestRepToZero { get; set; }
        public DailyQuests DailyQuests { get; set; }
        public DailyQuests WeeklyQuests { get; set; }

        public bool EnableQuests { get; set; }
        public DailyQuests ScavQuests { get; set; }
        public Quests()
        {
            ScavQuests = new DailyQuests()
            {
                LR1 = new()
                {
                    MinKills = 1,
                    MaxKills = 3,
                    MinItems = 2,
                    MaxItems = 5,
                    MinExtracts = 1,
                    MaxExtracts = 3,
                    MinSpecExits = 1,
                    MaxSpecExits = 3,
                },
                LR2 = new()
                {
                    MinKills = 3,
                    MaxKills = 9,
                    MinItems = 2,
                    MaxItems = 5,
                    MinExtracts = 2,
                    MaxExtracts = 7,
                    MinSpecExits = 1,
                    MaxSpecExits = 3,
                },
                LR3 = new()
                {
                    MinItems = 4,
                    MaxItems = 6,
                    MinExtracts = 3,
                    MaxExtracts = 15,
                    MinSpecExits = 2,
                    MaxSpecExits = 4,
                },
                Access = 1,
                QuestAmount = 1,
                Lifespan = 1440,
                Types = 6,
                Reroll = 2,
                Levels = "1,10,20,30,40,50,60",
                Experience = "0,0,0,0,0,0,0",
                Reputation = "0.02,0.02,0.03,0.03,0.04,0.04,0.05",
                ItemsReward = "2,3,3,3,3,4,4",
                Roubles = "11000,20000,32000,45000,58000,70000,82000",
                GPcoins = "1,1,2,2,4,4,5",
                SkillChance = "0,0,0,0,0,0,0",
                SkillPoint = "10,15,20,25,30,35,40"
            };
            DailyQuests = new DailyQuests()
            {
                LR1 = new()
                {
                    MinKills = 2,
                    MaxKills = 4,
                    MinItems = 1,
                    MaxItems = 4,
                    MinExtracts = 1,
                    MaxExtracts = 3,
                    MinSpecExits = 1,
                    MaxSpecExits = 2,
                },
                LR2 = new()
                {
                    MinKills = 5,
                    MaxKills = 15,
                    MinItems = 2,
                    MaxItems = 4,
                    MinExtracts = 2,
                    MaxExtracts = 7,
                    MinSpecExits = 1,
                    MaxSpecExits = 3,
                },
                LR3 = new()
                {
                    MinKills = 5,
                    MaxKills = 20,
                    MinItems = 3,
                    MaxItems = 6,
                    MinExtracts = 3,
                    MaxExtracts = 15,
                    MinSpecExits = 2,
                    MaxSpecExits = 4,
                },
                Access = 5,
                QuestAmount = 3,
                Lifespan = 1440,
                Types = 6,
                Reroll = 2,
                Levels = "1,10,20,30,40,50,60",
                Experience = "1000,2000,8000,13000,19000,24000,30000",
                Reputation = "0.01,0.02,0.03,0.03,0.03,0.03,0.03",
                ItemsReward = "2,3,4,5,5,5,5",
                Roubles = "11000,20000,32000,45000,58000,70000,82000",
                GPcoins = "3,3,6,6,8,8,10",
                SkillChance = "0,1,5,10,10,15,15",
                SkillPoint = "10,15,20,25,30,35,40"
            };
            WeeklyQuests = new DailyQuests()
            {
                LR1 = new()
                {
                    MinKills = 8,
                    MaxKills = 20,
                    MinItems = 4,
                    MaxItems = 6,
                    MinExtracts = 3,
                    MaxExtracts = 5,
                    MinSpecExits = 1,
                    MaxSpecExits = 4,
                },
                LR2 = new()
                {
                    MinKills = 15,
                    MaxKills = 40,
                    MinItems = 4,
                    MaxItems = 8,
                    MinExtracts = 4,
                    MaxExtracts = 8,
                    MinSpecExits = 2,
                    MaxSpecExits = 5,
                },
                LR3 = new()
                {
                    MinKills = 20,
                    MaxKills = 40,
                    MinItems = 6,
                    MaxItems = 12,
                    MinExtracts = 5,
                    MaxExtracts = 15,
                    MinSpecExits = 3,
                    MaxSpecExits = 6,
                },
                Access = 15,
                QuestAmount = 1,
                Lifespan = 10080,
                Types = 6,
                Reroll = 0,
                Levels = "1,10,20,30,40,50,60",
                Experience = "7500,18000,30000,80000,210000,260000,350000",
                Reputation = "0.02,0.03,0.04,0.04,0.05,0.06,0.07",
                ItemsReward = "3,4,5,5,5,5,5",
                Roubles = "20000,50000,175000,350000,540000,710000,880000",
                GPcoins = "10,10,16,16,20,30,35",
                SkillChance = "0,5,10,15,20,20,20",
                SkillPoint = "25,35,45,50,55,60,65"
            };
        }
    }
}
