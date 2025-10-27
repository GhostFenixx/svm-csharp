using Greed.Models;
using Greed.Models.Questing;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Spt.Config;
using SPTarkov.Server.Core.Models.Spt.Repeatable;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;

namespace ServerValueModifier.Sections
{
    internal class Quests(ISptLogger<SVM> logger, ConfigServer configServer, DatabaseService databaseService, MainClass.MainConfig svmconfig)
    {
        public void QuestSection()
        {
            CultureInfo customCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = customCulture;
            //For ease of use
            Greed.Models.Questing.DailyQuests daily = svmconfig.Quests.DailyQuests;
            Greed.Models.Questing.DailyQuests weekly = svmconfig.Quests.WeeklyQuests;
            Greed.Models.Questing.DailyQuests scavdaily = svmconfig.Quests.ScavQuests;

            SPTarkov.Server.Core.Models.Spt.Templates.Templates questdb = databaseService.GetTemplates();
            SPTarkov.Server.Core.Models.Eft.Common.Tables.RepeatableTemplates? questtemplate = questdb.RepeatableQuests.Templates;

            if (svmconfig.Quests.EnableQuestsMisc)//Awful, TODO rewrite into cycle somehow.
            {
                questtemplate.Elimination.ChangeCost[0].Count = (int)(questtemplate.Elimination.ChangeCost[0].Count * svmconfig.Quests.QuestCostMult);
                questtemplate.Completion.ChangeCost[0].Count = (int)(questtemplate.Completion.ChangeCost[0].Count * svmconfig.Quests.QuestCostMult);
                questtemplate.Exploration.ChangeCost[0].Count = (int)(questtemplate.Exploration.ChangeCost[0].Count * svmconfig.Quests.QuestCostMult);
                questtemplate.Pickup.ChangeCost[0].Count = (int)(questtemplate.Pickup.ChangeCost[0].Count * svmconfig.Quests.QuestCostMult);
                if (svmconfig.Quests.QuestRepToZero)
                {
                    questtemplate.Elimination.ChangeStandingCost = 0;
                    questtemplate.Completion.ChangeStandingCost = 0;
                    questtemplate.Exploration.ChangeStandingCost = 0;
                    questtemplate.Pickup.ChangeStandingCost = 0; //Useless probably
                }
            }
            QuestDetails(daily, 0);
            QuestDetails(weekly, 1);
            QuestDetails(scavdaily, 2);

            QuestRewards(daily, 0);
            QuestRewards(weekly, 1);
            QuestRewards(scavdaily, 2);
        }
        public void QuestRewards(DailyQuests type, int digit)
        {
            var quest = configServer.GetConfig<QuestConfig>();
            string[] levels = type.Levels.Split(',');
            string[] exp = type.Experience.Split(",");
            string[] rep = type.Reputation.Split(",");
            string[] itemsreward = type.ItemsReward.Split(",");
            string[] roubles = type.Roubles.Split(",");
            string[] gpcoins = type.GPcoins.Split(",");
            string[] skillpoints = type.SkillPoint.Split(",");
            string[] skillchance = type.SkillChance.Split(",");

            if ((levels.Length == exp.Length) && (levels.Length == rep.Length) && (levels.Length == itemsreward.Length) && (levels.Length == roubles.Length) && (levels.Length == gpcoins.Length) && (levels.Length == skillchance.Length) && (levels.Length == skillpoints.Length))
            {
                quest.RepeatableQuests[digit].RewardScaling.Levels = StrtoDConvert(levels);
                quest.RepeatableQuests[digit].RewardScaling.Experience = StrtoDConvert(exp);
                quest.RepeatableQuests[digit].RewardScaling.Reputation = StrtoDConvert(rep);
                quest.RepeatableQuests[digit].RewardScaling.Items = StrtoDConvert(itemsreward);
                quest.RepeatableQuests[digit].RewardScaling.Roubles = StrtoDConvert(roubles);
                quest.RepeatableQuests[digit].RewardScaling.GpCoins = StrtoDConvert(gpcoins);
                quest.RepeatableQuests[digit].RewardScaling.SkillPointReward = StrtoDConvert(skillpoints);
                quest.RepeatableQuests[digit].RewardScaling.SkillRewardChance = StrtoDConvert(skillchance);
            }
        }
        public List<double> StrtoDConvert(string[] element)
        {
            return [.. Array.ConvertAll<string, double>(element, double.Parse)];
        }
        public void QuestDetails(DailyQuests type, int digit)
        {
            var quest = configServer.GetConfig<QuestConfig>();
            string[] arrays = ["Elimination", "Completion", "Exploration"]; 
            quest.RepeatableQuests[digit].ResetTime = (long)type.Lifespan * 60;
            quest.RepeatableQuests[digit].NumQuests = type.QuestAmount;
            quest.RepeatableQuests[digit].FreeChanges = type.Reroll;
            quest.RepeatableQuests[digit].FreeChangesAvailable = type.Reroll;
            quest.RepeatableQuests[digit].MinPlayerLevel = type.Access;

            if (svmconfig.Quests.QuestRepToZero && svmconfig.Quests.EnableQuestsMisc)
            {
                quest.RepeatableQuests[digit].StandingChangeCost = [0];
            }
            //TODO ASAP - optimise, add missing fields, run through LRs lists
            //I tried to make it as small as possible ;-;
            LevelRanges[] ranges = { type.LR1, type.LR2, type.LR3 };
            for (int i = 0; i < quest.RepeatableQuests[digit].QuestConfig.ExplorationConfig.Count && i < ranges.Length; i++)
            {
                quest.RepeatableQuests[digit].QuestConfig.ExplorationConfig[i].MinimumExtracts = ranges[i].MinExtracts;
                quest.RepeatableQuests[digit].QuestConfig.ExplorationConfig[i].MaximumExtracts = ranges[i].MaxExtracts;
                quest.RepeatableQuests[digit].QuestConfig.ExplorationConfig[i].MinimumExtractsWithSpecificExit = ranges[i].MinSpecExits;
                quest.RepeatableQuests[digit].QuestConfig.ExplorationConfig[i].MaximumExtractsWithSpecificExit = ranges[i].MaxSpecExits;
            }
            for (int i = 0; i < quest.RepeatableQuests[digit].QuestConfig.CompletionConfig.Count && i < ranges.Length; i++)
            {
                quest.RepeatableQuests[digit].QuestConfig.CompletionConfig[i].RequestedItemCount.Min = ranges[i].MinItems;
                quest.RepeatableQuests[digit].QuestConfig.CompletionConfig[i].RequestedItemCount.Max = ranges[i].MaxItems;
            }
            for (int i = 0; i < quest.RepeatableQuests[digit].QuestConfig.Elimination.Count && i < ranges.Length; i++)
            {
                quest.RepeatableQuests[digit].QuestConfig.Elimination[i].MinKills = ranges[i].MinKills;
                quest.RepeatableQuests[digit].QuestConfig.Elimination[i].MaxKills = ranges[i].MaxKills;
            }
            //switch (type.Types)//I could just call exact possible types from enum or smth, hmm, Scrapped to better times.
            //{
            //    case 0:
            //        //quest.RepeatableQuests[digit].Types.ForEach( type => logger.Warning(type.ToString()));
            //        //quest.RepeatableQuests[digit].Types.Remove(arrays[1]);
            //        //quest.RepeatableQuests[digit].Types.Remove(arrays[2]);
            //        if (quest.RepeatableQuests[digit].Name == "Daily") quest.RepeatableQuests[digit].Name = "SVM_Daily";
            //        quest.RepeatableQuests[digit].TraderWhitelist.ForEach(tradtype =>
            //        { 
            //        tradtype.QuestTypes.Remove(arrays[1]);
            //        tradtype.QuestTypes.Remove(arrays[2]);
            //        });
            //        break;
            //    case 1:
            //        //quest.RepeatableQuests[digit].Types.Remove(arrays[0]);
            //        //quest.RepeatableQuests[digit].Types.Remove(arrays[2]);
            //        if (quest.RepeatableQuests[digit].Name == "Daily") quest.RepeatableQuests[digit].Name = "SVM_Daily";
            //        quest.RepeatableQuests[digit].TraderWhitelist.ForEach(tradtype =>
            //        {
            //            tradtype.QuestTypes.Remove(arrays[0]);
            //            tradtype.QuestTypes.Remove(arrays[2]);
            //        });
            //        break;
            //    case 2:
            //        //quest.RepeatableQuests[digit].Types.Remove(arrays[0]);
            //        //quest.RepeatableQuests[digit].Types.Remove(arrays[1]);
            //        if (quest.RepeatableQuests[digit].Name == "Daily") quest.RepeatableQuests[digit].Name = "SVM_Daily";
            //        quest.RepeatableQuests[digit].TraderWhitelist.ForEach(tradtype =>
            //        {
            //            tradtype.QuestTypes.Remove(arrays[0]);
            //            tradtype.QuestTypes.Remove(arrays[1]);
            //        });
            //        break;
            //    case 3:
            //        //quest.RepeatableQuests[digit].Types.Remove(arrays[2]);
            //        if (quest.RepeatableQuests[digit].Name == "Daily") quest.RepeatableQuests[digit].Name = "SVM_Daily";
            //        quest.RepeatableQuests[digit].TraderWhitelist.ForEach(tradtype =>
            //        {
            //            tradtype.QuestTypes.Remove(arrays[2]);
            //        });
            //        break;
            //    case 4:
            //        //quest.RepeatableQuests[digit].Types.Remove(arrays[0]);
            //        if (quest.RepeatableQuests[digit].Name == "Daily") quest.RepeatableQuests[digit].Name = "SVM_Daily";
            //        quest.RepeatableQuests[digit].TraderWhitelist.ForEach(tradtype =>
            //        {
            //            tradtype.QuestTypes.Remove(arrays[0]);
            //        });
            //        break;
            //    case 5:
            //        //quest.RepeatableQuests[digit].Types.Remove(arrays[1]);
            //        if (quest.RepeatableQuests[digit].Name == "Daily") quest.RepeatableQuests[digit].Name = "SVM_Daily";
            //        quest.RepeatableQuests[digit].TraderWhitelist.ForEach(tradtype =>
            //        {
            //            tradtype.QuestTypes.Remove(arrays[1]);
            //        });
            //        break;
            //    case 6:
            //        break;
            //}

        }
    }
}
