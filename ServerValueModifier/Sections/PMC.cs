using Greed.Models;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Spt.Config;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services;

namespace ServerValueModifier.Sections
{
    internal class PMC(ISptLogger<SVM> logger, ConfigServer configServer, DatabaseService databaseService, MainClass.MainConfig svmcfg)
    {
        public void PMCSection()
        {
            var items = databaseService.GetItems();
            var pmc = configServer.GetConfig<PmcConfig>();
            var bot = configServer.GetConfig<BotConfig>();
            var bottypes = databaseService.GetBots();
            pmc.IsUsec = svmcfg.PMC.PMCRatio;
            pmc.BotRelativeLevelDelta.Min = svmcfg.PMC.LevelDownMargin;
            pmc.BotRelativeLevelDelta.Max = svmcfg.PMC.LevelUpMargin;
            var locs = databaseService.GetLocations();


            if (svmcfg.PMC.ChancesEnable)
            {
                pmc.HostilitySettings["pmcbear"].UsecEnemyChance = svmcfg.PMC.PMCChance.HostilePMC;
                pmc.HostilitySettings["pmcusec"].UsecEnemyChance = svmcfg.PMC.PMCChance.HostileSamePMC;
                pmc.HostilitySettings["pmcbear"].BearEnemyChance = svmcfg.PMC.PMCChance.HostileSamePMC;
                pmc.HostilitySettings["pmcusec"].BearEnemyChance = svmcfg.PMC.PMCChance.HostilePMC;//hehe, palindromes
                string[] applicabletypes = ["pmcusec", "pmcbear", "bear", "usec"];
                for (int i = 0 ; i < applicabletypes.Length; i++)
                {
                    foreach (var item in bottypes.Types[applicabletypes[i]].BotDifficulty)
                    {
                        item.Value.Mind.DefaultBearBehaviour = SPTarkov.Server.Core.Models.Eft.Bot.GlobalSettings.BotGlobalsMindSettings.EWarnBehaviour.ChancedEnemies;
                        item.Value.Mind.DefaultUsecBehaviour = SPTarkov.Server.Core.Models.Eft.Bot.GlobalSettings.BotGlobalsMindSettings.EWarnBehaviour.ChancedEnemies;
                    }
                }
                pmc.LooseWeaponInBackpackChancePercent = svmcfg.PMC.PMCChance.PMCLooseWep;
                pmc.WeaponHasEnhancementChancePercent = svmcfg.PMC.PMCChance.PMCWepEnhance;

                //pmc.AddPrefixToSameNamePMCAsPlayerChance = svmcfg.PMC.PMCChance.PMCNamePrefix; TODO
                pmc.AllPMCsHavePlayerNameWithRandomPrefixChance = svmcfg.PMC.PMCChance.PMCAllNamePrefix;
            }

            if (svmcfg.PMC.LootableMelee)//Honestly it should migrate to Items  
            {
                foreach (TemplateItem basetemplate in items.Values)
                {
                    if (basetemplate.Parent == "5447e1d04bdc2dff2f8b4567" && basetemplate.Id != "6087e570b998180e9f76dc24")
                    {
                        basetemplate.Properties.UnlootableFromSide = [];
                    }
                }
            }



            if (svmcfg.PMC.DisableLowLevelPMC)
            {
                try
                {
                    bot.Equipment["pmc"].Randomisation[0].LevelRange.Min = 1;
                    bot.Equipment["pmc"].Randomisation[0].LevelRange.Max = 14;//svm.cfg.PMC.LvlRange1
                    bot.Equipment["pmc"].Randomisation[1].LevelRange.Min = 15;// svm.cfg.PMC.LvlRange1 + 1
                    bot.Equipment["pmc"].Randomisation[1].LevelRange.Max = 22;//svm.cfg.PMC.LvlRange2
                    bot.Equipment["pmc"].Randomisation[2].LevelRange.Min = 23;// svm.cfg.PMC.LvlRange2 + 1
                    bot.Equipment["pmc"].Randomisation[2].LevelRange.Max = 45;//svm.cfg.PMC.LvlRange3 
                    bot.Equipment["pmc"].Randomisation[2].LevelRange.Min = 46;// svm.cfg.PMC.LvlRange3 + 1
                    //No point declaring Max of Level range 4 since it goes to 100, gotta limit level ranges to 75 to avoid hitting the ceiling.
                    //So they won't overlap this way, 3 fields for user control and being catched if certain mods deletes them.
                }
                catch
                {
                    logger.Warning("[SVM] AI PMC - Level Ranges missing, probably another mod in action that reworks them, ignoring changes");
                }
            }
            //bot.Equipment["pmc"].Randomisation[1].LevelRange.Min = 1; According to Acid - they shouldn't overlap, therefore i'll just remove first level range and leave it be.
            //bot.Equipment["pmc"].Randomisation[2].LevelRange.Min = 1; Probably will just allow user to adjust every range eventually.
            //bot.Equipment["pmc"].Randomisation[3].LevelRange.Min = 1;
            if (svmcfg.PMC.NamesEnable)
            {
                if (svmcfg.PMC.NameOverride)
                {
                    string[] names = svmcfg.PMC.PMCNameList.Split("\r\n");
                    foreach (string name in names)
                    {
                        bottypes.Types["pmcusec"].FirstNames.Add(name);
                        bottypes.Types["usec"].FirstNames.Add(name);
                        bottypes.Types["pmcbear"].FirstNames.Add(name);
                        bottypes.Types["bear"].FirstNames.Add(name);
                    }
                }
            }
        }
    }
}
