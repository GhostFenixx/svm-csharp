using Greed.Models;
using Greed.Models.Flea;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Eft.Weather;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Spt.Config;
using SPTarkov.Server.Core.Models.Spt.Repeatable;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services;
using SPTarkov.Server.Core.Utils.Cloners;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml;
using static System.Net.WebRequestMethods;
using TraderID = SPTarkov.Server.Core.Models.Enums.Traders;

namespace ServerValueModifier
{
    [Injectable(TypePriority = OnLoadOrder.PreSptModLoader + 5)]
    public class SVMPreLoad(ISptLogger<SVM> logger, ModHelper modhelper, ConfigServer configServer) : IOnLoad
    {
        public Task OnLoad()
        {
            try
            {
                //Load Preset
                MainClass.MainConfig svmcfg = new SVMConfig(modhelper).CallConfig();
                //A list of features that should run before `PerformPostDbLoadActions`
                //Will make into a separate section later.
                //bots section

                if (svmcfg.Bots.EnableBots)
                {
                    BotConfig bots = configServer.GetConfig<BotConfig>();
                    bots.WeeklyBoss.Enabled = !svmcfg.Bots.AIChance.DisableWeeklyBoss;
                    if (svmcfg.Raids.RaidEvents.AITypeOverride)
                    {
                        switch (svmcfg.Raids.RaidEvents.AIType) //2.0.1 change - using SPT functionality now.
                        {
                            case 0: bots.ReplaceScavWith = WildSpawnType.pmcBot; break;
                            case 1: bots.ReplaceScavWith = WildSpawnType.exUsec; break;
                            case 2: bots.ReplaceScavWith = WildSpawnType.sectantWarrior; break;
                            case 3: bots.ReplaceScavWith = WildSpawnType.pmcBEAR; break;
                            case 4: bots.ReplaceScavWith = WildSpawnType.pmcUSEC; break;
                        }
                    }
                }
                //Raids/ Events section
                if (svmcfg.Raids.EnableRaids)
                {
                    WeatherConfig weatherconfig = configServer.GetConfig<WeatherConfig>();
                    weatherconfig.Acceleration = svmcfg.Raids.Timeacceleration;
                    if (svmcfg.Raids.ForceSeason)
                    {
                        switch (svmcfg.Raids.Season)
                        {
                            case 0:
                                weatherconfig.OverrideSeason = Season.SUMMER;
                                break;
                            case 1:
                                weatherconfig.OverrideSeason = Season.AUTUMN;
                                break;
                            case 2:
                                weatherconfig.OverrideSeason = Season.WINTER;
                                break;
                            case 3:
                                weatherconfig.OverrideSeason = Season.SPRING;
                                break;
                            case 4:
                                weatherconfig.OverrideSeason = Season.STORM;
                                break;
                        }
                    }

                    var eventconfig = configServer.GetConfig<SeasonalEventConfig>();
                    var questconfig = configServer.GetConfig<QuestConfig>();
                    questconfig.ShowNonSeasonalEventQuests = svmcfg.Raids.RaidEvents.NonSeasonalQuests;
                    foreach (var eventname in eventconfig.Events)//Very redundant tbh
                    {
                        if (eventname.Name == "halloween" && svmcfg.Raids.RaidEvents.DisableZombies)
                        {
                            eventname.Settings.ZombieSettings.Enabled = !svmcfg.Raids.RaidEvents.DisableZombies;
                        }
                    }
                    eventconfig.EnableSeasonalEventDetection = !svmcfg.Raids.RaidEvents.DisableEvents;
                    if (svmcfg.Raids.RaidEvents.DisableHalloweenAIFriendly)
                    {
                        foreach (var bottype in eventconfig.HostilitySettingsForEvent["zombies"]["default"])
                        {
                            if (bottype.BotRole == "pmcBEAR")
                            {
                                bottype.SavagePlayerBehaviour = "AlwaysEnemies";
                                bottype.Neutral.Remove("pmcUSEC");
                                bottype.AlwaysEnemies.Add("pmcUSEC");
                            }
                            else if (bottype.BotRole == "pmcUSEC")
                            {
                                bottype.SavagePlayerBehaviour = "AlwaysEnemies";
                                bottype.Neutral.Remove("pmcBEAR");
                                bottype.AlwaysEnemies.Add("pmcBEAR");
                            }
                            else
                            {
                                bottype.BearPlayerBehaviour = "AlwaysEnemies";
                                bottype.UsecPlayerBehaviour = "AlwaysEnemies";
                            }
                        }
                    }
                }
                //Flea section
                if (svmcfg.Fleamarket.EnablePlayerOffers && svmcfg.Fleamarket.EnableFleamarket)
                {
                    var fleaconfig = configServer.GetConfig<RagfairConfig>();
                    fleaconfig.Dynamic.Blacklist.EnableBsgList = !svmcfg.Fleamarket.DisableBSGList;
                }
                //

            }
            catch (FileNotFoundException)
            {
            }
            catch (Exception ex)
            {
                logger.Error("[SVM] Pre-DB Initialization failed due to unknown error: " + ex);
            }
            return Task.CompletedTask;
        }
    }
    [Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 5)]
    public class SVM(ISptLogger<SVM> logger, ModHelper modhelper, ConfigServer configServer, SeasonalEventService seasonalEvent, DatabaseService databaseService, LocaleService localeService, ICloner _cloner) : IOnLoad
    {

        public static string ffolder; // TODO - Wait until changes are done about GetExecutingAssembly, create a new method to point to mod folder
        public Task OnLoad()
        {

            string folder = modhelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
            ffolder = folder;
            try
            {
                //Load Preset
                MainClass.MainConfig svmcfg = new SVMConfig(modhelper).CallConfig();
                //Predefining modded items to avoid profile "corruption" if certain fields were removed
                Dictionary<SPTarkov.Server.Core.Models.Common.MongoId, TemplateItem> items = databaseService.GetItems();
                //Creating Pockets here to exist whether section ON or OFF so we won't have an issue when modded item not found if section is off
                TemplateItem custompocket = _cloner.Clone(items["627a4e6b255f7527fb05a0f6"]);
                custompocket.Id = "a8edfb0bce53d103d3f62b9b";
                items["a8edfb0bce53d103d3f62b9b"] = custompocket;
                TemplateItem scavcustompocket = _cloner.Clone(items["557ffd194bdc2d28148b457f"]);
                scavcustompocket.Id = "a8edfb0bce53d103d3f6219b";
                items["a8edfb0bce53d103d3f6219b"] = scavcustompocket;
                //Creating Ragman offers for SCAV apparels
                DeclareModdedAssort(svmcfg);
                //Initialisation
                Sections.Items itemLoad = new(logger, configServer, databaseService, svmcfg);
                Sections.Hideout hideoutLoad = new(logger, configServer, databaseService, _cloner, svmcfg);
                Sections.Traders tradersLoad = new(logger, configServer, databaseService, svmcfg);
                Sections.Services servicesLoad = new(logger, configServer, databaseService, _cloner, localeService, svmcfg);
                Sections.Loot lootLoad = new(logger, configServer, databaseService, svmcfg);
                Sections.Bots botload = new(logger, configServer, databaseService, svmcfg);
                Sections.Player playerLoad = new(logger, configServer, databaseService, svmcfg);
                Sections.Raids raidsload = new(logger, configServer, databaseService, svmcfg);
                Sections.Fleamarket fleamarketLoad = new(logger, configServer, databaseService, svmcfg);
                Sections.Quests questsLoad = new(logger, configServer, databaseService, svmcfg);
                Sections.CSM csmLoad = new(logger, configServer, databaseService, svmcfg, _cloner, custompocket);
                Sections.Events eventsLoad = new(logger, configServer, databaseService, svmcfg, seasonalEvent, modhelper);
                Sections.Scav scavload = new(logger, configServer, databaseService, svmcfg, _cloner, scavcustompocket);
                Sections.PMC pmcload = new(logger, configServer, databaseService, svmcfg);

                if (svmcfg.Items.EnableItems) itemLoad.ItemsSection();
                if (svmcfg.Hideout.EnableHideout) hideoutLoad.HideoutSection();
                if (svmcfg.Traders.EnableTraders) tradersLoad.TradersSection();
                if (svmcfg.Services.EnableServices) servicesLoad.ServicesSection();
                if (svmcfg.Loot.EnableLoot) lootLoad.LootSection();
                if (svmcfg.Fleamarket.EnableFleamarket) fleamarketLoad.FleamarketSection();
                if (svmcfg.Player.EnablePlayer) playerLoad.PlayerSection();
                if (svmcfg.Raids.EnableRaids)
                {
                    raidsload.RaidsSection();
                    eventsLoad.EventsSection();
                }
                if (svmcfg.Quests.EnableQuests) questsLoad.QuestSection();
                if (svmcfg.Bots.EnableBots) botload.BotsSection();
                if (svmcfg.CSM.EnableCSM) csmLoad.CSMSection();
                if (svmcfg.Scav.EnableScav) scavload.ScavSection();
                if (svmcfg.PMC.EnablePMC) pmcload.PMCSection();

                string[] funnitext = System.IO.File.ReadAllText(System.IO.Path.Combine(ffolder, "Misc", "MOTD.txt")).Split("\n");
                Random rnd = new Random();
                logger.LogWithColor("[SVM] Initialization complete. " + funnitext[rnd.Next(0, funnitext.Length)], SPTarkov.Server.Core.Models.Logging.LogTextColor.Blue);
                logger.LogWithColor("[SVM] Preset - " + new SVMConfig(modhelper).ServerMessage()!["CurrentlySelectedPreset"] + " - successfully loaded", SPTarkov.Server.Core.Models.Logging.LogTextColor.Blue);
            }
            catch (FileNotFoundException)
            {
                logger.Error("[SVM] Initialization cancelled: Preset or/and Loader is not found or null\nBe sure you clicked Save and Apply in Greed.exe\nMod is disabled");
            }
            catch (Exception ex)
            {
                logger.Error("[SVM] Initialization failed. Check the error: " + ex.Message.ToString());
            }
            return Task.CompletedTask;
        }
        //I hate the fact it's here, but it's best for users, to not see `can't run the profile - it's corrupted`.
        public void DeclareModdedAssort(MainClass.MainConfig svmcfg)
        {
            //Crudely added, TODO
            MongoId[] bodies = { "66546f823b51a4d21e0d17d7", "5e4bb3ee86f77406975c934e", "5fd22d311add82653b5a704c", "618d1af10a5a59657e5f56f3", "5cc2e59214c02e000f16684e", "5e9da1d086f774054a667134", "619238266c614e6d15792bca", "5fd1eb3fbe3b7107d66cb645", "5cde9f337d6c8b0474535da8", //Scav apparel - Not finished, TODO
                "67ac86f8a6749cd1690ae1dd","67ac870e5d717b44c00a0c94","67ac7eeba6749cd1690ae1d7", "67ac7f135f7251f49d0e9b0b","67ac7f295d717b44c00a0c8c","67ac7f3ca6749cd1690ae1d9","67ac7f52e4d14ccd6005de86"}; // BP outfits
            MongoId[] feets = { "6193be546e5968395b260157", "61922589bb2cea6f7a22d964", "637df25a1e688345e1097bd4", "5f5e410c6bdad616ad46d60b", "5f5e41576760b4138443b344", "5f5e41366760b4138443b343", "5df8a15186f77412640e2e6a", "5cc2e5d014c02e15d53d9c03", "5cde9fb87d6c8b0474535da9",
            "67ab1543755a9402da0011a2", "67ab1556508ee9b6440e9c62", "67ab1570fe82855dcc0f2aec","67ab0b06755a9402da001199", "67ab0b16755a9402da00119b", "67ab0b27755a9402da00119d" };
            MongoId[] hands = { "6654957791883f6c9f0f1f50", "5e4bb49586f77406a313ec5a", "5fd7901bdd870108a754c0e6", "6197aca964ae5436d76c1f98", "5cc2e68f14c02e28b47de290", "5e9da2dd86f774054e7d0f63", "618cf9540a5a59657e5f5677", "5fd78fe9e3bfcf6cab4c9f54", "5cde9fff7d6c8b647a3769b1",
                "67ac8728e4d14ccd6005de88" , "67ac873d5d717b44c00a0c96", "67ac81ab5f7251f49d0e9b0d", "67ac81fd5d717b44c00a0c8e", "67ac8233a6749cd1690ae1db", "67ac82465f7251f49d0e9b0f", "67ac825b5d717b44c00a0c90"}; // BP hands
            string upuid = "66a259bc0080c7eb925db";
            string lowuid = "66a259bc0080c7eb925dc";//Why this way? To keep it consistent between server restarts
            int p = 100;//Somewhere defo crying one little lacy
            for (int i = 0; i < bodies.Length; i++)
            {
                CreateUpperSuitAndAddIntoAssort(upuid + (p + 1), bodies[i],
                                    hands[i], upuid + (p + 2), upuid + (p + 3), svmcfg);
                p = p + 3;
            }
            for (int i = 0; i < feets.Length; i++)
            {
                CreateLowerSuitAndAddIntoAssort(lowuid + (p + 1), feets[i],
                                    lowuid + (p + 2), svmcfg);
                p = p + 3;
            }
        }

        public void CreateUpperSuitAndAddIntoAssort(MongoId upKitID, MongoId body, MongoId hands, MongoId lowKitID, MongoId tradeUpperUID, MainClass.MainConfig cfg)
        {
            var suits = databaseService.GetCustomization();//Maybe i shouldn't call them here, TODO
            var traders = databaseService.GetTraders();
            var locales = databaseService.GetTables().Locales.Global;
            if (traders[TraderID.FENCE].Suits is null) //Horrible temporary solution, will make it outside cycle. later, TODO
            {
                traders[TraderID.FENCE].Base.CustomizationSeller = true;
                traders[TraderID.FENCE].Suits = new();
            }
            Suit upperassort = _cloner.Clone(traders[TraderID.RAGMAN].Suits[1]);//Example we're using - Contractor BEAR Upper
            CustomizationItem? uppercustom = _cloner.Clone(suits["5d1f60ae86f7744bcc04998b"]);
            uppercustom.Id = upKitID;
            uppercustom.Properties.Body = body;
            uppercustom.Properties.Hands = hands;
            upperassort.Id = tradeUpperUID;
            upperassort.SuiteId = upKitID;
            if (cfg.Services.ClothesAnySide)
            {
                traders[TraderID.FENCE].Suits.Add(upperassort);
            }
            suits[upKitID] = uppercustom;
            //MongoId originallocale = 
            if (databaseService.GetLocales().Global.TryGetValue("en", out var lazyloadedValue))
            {
                lazyloadedValue.AddTransformer(lazyloadedLocaleData =>
                {
                    if (ApparelNames.LOCALES.TryGetValue(upKitID, out var locale))
                    {
                        lazyloadedLocaleData.Add(upKitID + " Name", locale.Name);
                        lazyloadedLocaleData.Add(upKitID + " ShortName", locale.ShortName);
                        lazyloadedLocaleData.Add(upKitID + " Description", locale.Description);
                    }
                    else
                    {
                        lazyloadedLocaleData.Add(upKitID + " Name", "Scav Apparel");
                        lazyloadedLocaleData.Add(upKitID + " ShortName", "Scav");
                        lazyloadedLocaleData.Add(upKitID + " Description", "SVM's Special");
                    }
                    return lazyloadedLocaleData;
                });
            }
        }
        public void CreateLowerSuitAndAddIntoAssort(MongoId lowKitID, MongoId feet, MongoId tradeLowerUID, MainClass.MainConfig cfg)
        {
            var suits = databaseService.GetCustomization();//Maybe i shouldn't call them here, TODO
            var traders = databaseService.GetTraders();
            var locales = databaseService.GetTables().Locales.Global;

            Suit lowerassort = _cloner.Clone(traders[TraderID.RAGMAN].Suits[1]);//
            CustomizationItem? lowercustom = _cloner.Clone(suits["66043e047502eca33a08cad6"]);
            lowercustom.Id = lowKitID;
            lowercustom.Properties.Feet = feet;
            lowerassort.Id = tradeLowerUID;
            lowerassort.SuiteId = lowKitID;
            if (cfg.Services.ClothesAnySide)
            {
                traders[TraderID.FENCE].Suits.Add(lowerassort);
            }
            suits[lowKitID] = lowercustom;

            if (databaseService.GetLocales().Global.TryGetValue("en", out var lazyloadedValue))
            {
                lazyloadedValue.AddTransformer(lazyloadedLocaleData =>
                {
                    if (ApparelNames.LOCALES.TryGetValue(lowKitID, out var locale))
                    {
                        lazyloadedLocaleData.Add(lowKitID + " Name", locale.Name);
                        lazyloadedLocaleData.Add(lowKitID + " ShortName", locale.ShortName);
                        lazyloadedLocaleData.Add(lowKitID + " Description", locale.Description);
                    }
                    else
                    {
                        lazyloadedLocaleData.Add(lowKitID + " Name", "Scav Apparel");
                        lazyloadedLocaleData.Add(lowKitID + " ShortName", "Scav");
                        lazyloadedLocaleData.Add(lowKitID + " Description", "SVM's Special");
                    }

                    return lazyloadedLocaleData;
                });
            }
        }

        private class Loader
        {
            public string CurrentlySelectedPreset { get; set; }
        }
    }
    [Injectable(TypePriority = OnLoadOrder.PostSptModLoader + 100)]
    public class SVMPostLoad(ISptLogger<SVM> logger, ConfigServer configServer, DatabaseService databaseService, ModHelper modhelper) : IOnLoad
    {
        public Task OnLoad() //Separation of custom section for sake to load last in attempt to work with any possible values (including modded ones) after all changes.
        {
            try
            {
                //Load Preset
                MainClass.MainConfig svmcfg = new SVMConfig(modhelper).CallConfig();
                Sections.Advanced advLoad = new(logger, configServer, databaseService, svmcfg);
                if (svmcfg.Custom.EnableCustom) advLoad.ItemChangerSection();
                return Task.CompletedTask;
            }
            catch // Currently no logging on this since we have them in the method itself? debug/tests required
            {
                return Task.CompletedTask;
            }
        }
    }
}
