using Greed.Models;
using Greed.Models.Raiding;
using SPTarkov.Common.Extensions;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Enums.RaidSettings;
using SPTarkov.Server.Core.Models.Spt.Config;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services;

namespace ServerValueModifier.Sections
{
    //Safe exit and Softcore located in Routers.ValidateOverrider
    internal class Raids(ISptLogger<SVM> logger, ConfigServer configServer, DatabaseService databaseService, SeasonalEventService seasonalEvent, MainClass.MainConfig svmconfig)
    {
        private readonly Globals globals = databaseService.GetGlobals();
        private readonly WeatherConfig Weather = configServer.GetConfig<WeatherConfig>();
        private readonly TraderConfig trader = configServer.GetConfig<TraderConfig>();
        private readonly LostOnDeathConfig midcore = configServer.GetConfig<LostOnDeathConfig>();
        private readonly SPTarkov.Server.Core.Models.Spt.Server.Locations locationsdb = databaseService.GetLocations();
        private readonly SPTarkov.Server.Core.Models.Eft.Common.Tables.LocationServices locationservices = databaseService.GetLocationServices();
        private readonly InRaidConfig inraid = configServer.GetConfig<InRaidConfig>();
        public void RaidsSection()
        {

                SeasonalEventConfig season = configServer.GetConfig<SeasonalEventConfig>();
                if (svmconfig.Raids.RaidEvents.Christmas)
                {
                    seasonalEvent.ForceSeasonalEvent(SeasonalEventType.Christmas);
                }

                if (svmconfig.Raids.RaidEvents.Halloween)
                {
                //globals.Configuration.SeasonActivity.InfectionHalloween.AddToExtensionData("RedThreatValue", 50);
                globals.Configuration.SeasonActivity.InfectionHalloween.DisplayUIEnabled = true;
                    seasonalEvent.ForceSeasonalEvent(SeasonalEventType.Halloween);
                }
            //Pre-raid menu settings
            if (svmconfig.Raids.RaidStartup.EnableRaidStartup)
            {
                switch (svmconfig.Raids.RaidStartup.AIAmount) //Should i make my own ENUM of sorts? TODO
                {
                    case 0:
                        inraid.RaidMenuSettings.AiAmount = "AsOnline";
                        break;
                    case 1:
                        inraid.RaidMenuSettings.AiAmount = "Low";
                        break;
                    case 2:
                        inraid.RaidMenuSettings.AiAmount = "Medium";
                        break;
                    case 3:
                        inraid.RaidMenuSettings.AiAmount = "High";
                        break;
                    case 4:
                        inraid.RaidMenuSettings.AiAmount = "Horde";
                        break;
                    case 5:
                        inraid.RaidMenuSettings.AiAmount = "NoBots";
                        break;
                }
                switch (svmconfig.Raids.RaidStartup.AIDifficulty)
                {
                    case 0:
                        inraid.RaidMenuSettings.AiDifficulty = "AsOnline";
                        break;
                    case 1:
                        inraid.RaidMenuSettings.AiDifficulty = "Easy";
                        break;
                    case 2:
                        inraid.RaidMenuSettings.AiDifficulty = "Medium";
                        break;
                    case 3:
                        inraid.RaidMenuSettings.AiDifficulty = "Hard";
                        break;
                    case 4:
                        inraid.RaidMenuSettings.AiDifficulty = "Impossible";
                        break;
                    case 5:
                        inraid.RaidMenuSettings.AiDifficulty = "Random";
                        break;
                }
                globals.Configuration.TimeBeforeDeploy = (double)svmconfig.Raids.RaidStartup.TimeBeforeDeployLocal;
                globals.Configuration.TimeBeforeDeployLocal = (double)svmconfig.Raids.RaidStartup.TimeBeforeDeployLocal;
                inraid.RaidMenuSettings.BossEnabled = svmconfig.Raids.RaidStartup.EnableBosses;
                inraid.RaidMenuSettings.ScavWars = svmconfig.Raids.RaidStartup.ScavWars;
                inraid.RaidMenuSettings.TaggedAndCursed = svmconfig.Raids.RaidStartup.TaggedAndCursed;

                //inraid.Save.Loot = Config.Raids.RaidStartup.SaveLoot; TODO
            }
            if (svmconfig.Raids.ForceSeason)
            {
                //TODO turn from int to string/enum
                //Might require to add the rest of seasons enums
                //Weather.OverrideSeason = Config.Raids.Season;
                //Weather.OverrideSeason = SPTarkov.Server.Core.Models.Enums.Season.SUMMER;
                //logger.Info(Weather.OverrideSeason.ToString());
                //Weather.OverrideSeason = (Season)Config.Raids.Season;
                //logger.Info(Weather.OverrideSeason.ToString());
                switch (svmconfig.Raids.Season)
                {
                    case 0: Weather.OverrideSeason = Season.SUMMER;
                        break;
                    case 1:
                        Weather.OverrideSeason = Season.AUTUMN;
                        break;
                    case 2:
                        Weather.OverrideSeason = Season.WINTER;
                        break;
                    case 3:
                        Weather.OverrideSeason = Season.SPRING;
                        break;
                    case 4:
                        Weather.OverrideSeason = Season.STORM;
                        break;
                }
            }
            //Enable/Disable Fence gifts 
            trader.Fence.CoopExtractGift.SendGift = !svmconfig.Raids.Exfils.FenceGift;
            //Save quest items on death
            midcore.QuestItems = !svmconfig.Raids.SaveQuestItems;
            locationsdb.Sandbox.Base.RequiredPlayerLevelMax = svmconfig.Raids.SandboxAccessLevel;
            Weather.Acceleration = svmconfig.Raids.Timeacceleration;

            if (svmconfig.Raids.NoRunThrough)
            {
                globals.Configuration.Exp.MatchEnd.SurvivedExperienceRequirement = 0;
                globals.Configuration.Exp.MatchEnd.SurvivedSecondsRequirement = 0;
            }
            if (svmconfig.Raids.Removelabkey)
            {
                locationsdb.Laboratory.Base.AccessKeys = [];
            }
                locationsdb.Laboratory.Base.Insurance = svmconfig.Raids.LabInsurance;
            if (svmconfig.Raids.ForceTransitStash)
            {
                foreach (var level in globals.Configuration.FenceSettings.Levels)
                {
                    level.Value.TransitGridSize.X = svmconfig.Raids.TransitWidth;
                    level.Value.TransitGridSize.Y = svmconfig.Raids.TransitHeight;
                }
            }
            if (svmconfig.Raids.EnableBTR)
            {
                globals.Configuration.BTRSettings.CleanUpPrice = svmconfig.Raids.BTRCoverPrice;
                globals.Configuration.BTRSettings.BasePriceTaxi = svmconfig.Raids.BTRTaxiPrice;
                globals.Configuration.BTRSettings.BearPriceMod = svmconfig.Raids.BearMult;
                globals.Configuration.BTRSettings.UsecPriceMod = svmconfig.Raids.UsecMult;
                globals.Configuration.BTRSettings.ScavPriceMod = svmconfig.Raids.ScavMult;
                locationservices.BtrServerSettings.ServerMapBTRSettings["Woods"].ChanceSpawn = svmconfig.Raids.BTRWoodsChance;
                locationservices.BtrServerSettings.ServerMapBTRSettings["Woods"].SpawnPeriod.X = svmconfig.Raids.BTRWoodsTimeMin;
                locationservices.BtrServerSettings.ServerMapBTRSettings["Woods"].SpawnPeriod.Y = svmconfig.Raids.BTRWoodsTimeMax;
                locationservices.BtrServerSettings.ServerMapBTRSettings["TarkovStreets"].ChanceSpawn = svmconfig.Raids.BTRStreetsChance;
                locationservices.BtrServerSettings.ServerMapBTRSettings["TarkovStreets"].SpawnPeriod.X = svmconfig.Raids.BTRStreetsTimeMin;
                locationservices.BtrServerSettings.ServerMapBTRSettings["TarkovStreets"].SpawnPeriod.Y = svmconfig.Raids.BTRStreetsTimeMax;
                {
                    foreach (var level in globals.Configuration.FenceSettings.Levels)
                    {
                        if (svmconfig.Raids.ForceBTRStash)
                        {
                            level.Value.DeliveryGridSize.X = svmconfig.Raids.BTRWidth;
                            level.Value.DeliveryGridSize.Y = svmconfig.Raids.BTRHeight;
                        }
                        if (svmconfig.Raids.ForceBTRFriendly)
                        {
                            level.Value.CanInteractWithBtr = true;
                        }
                    }
                }
            }
            if (svmconfig.Raids.Exfils.ArmorExtract)
            {
                var allowexfil = globals.Configuration.RequirementReferences.Alpinists.ToList();
                allowexfil.Splice(2, 1);
                globals.Configuration.RequirementReferences.Alpinists = allowexfil;
            }
            if (svmconfig.Raids.Exfils.GearExtract)
            {
                var allowexfil = globals.Configuration.RequirementReferences.Alpinists.ToList();
                allowexfil.Splice(0, 2);
                globals.Configuration.RequirementReferences.Alpinists = allowexfil;
            }
            foreach (Location names in locationsdb.GetDictionary().Values)
            {
                if (svmconfig.Raids.RaidTime != 0)
                {
                    names.Base.ExitAccessTime += svmconfig.Raids.RaidTime;
                    names.Base.EscapeTimeLimit += svmconfig.Raids.RaidTime;
                    names.Base.EscapeTimeLimitCoop += svmconfig.Raids.RaidTime;
                    names.Base.EscapeTimeLimitPVE += svmconfig.Raids.RaidTime;
                }
                var ExitNames = names.Base.Exits;
                foreach (var exits in ExitNames)
                {
                    if (exits.Name != "Exfil_Train" && exits.Name != "Saferoom Exfil")
                    {
                        if (svmconfig.Raids.Exfils.GearExtract && svmconfig.Raids.Exfils.ArmorExtract && exits.PassageRequirement == RequirementState.Reference)
                        {
                            FreeExit(exits);
                        }
                        if (svmconfig.Raids.Exfils.NoBackpack && exits.PassageRequirement == RequirementState.Empty)
                        {
                            FreeExit(exits);
                        }
                        if (exits.PassageRequirement == RequirementState.TransferItem && svmconfig.Raids.EnableCarCoop)
                        {
                            exits.ExfiltrationTime = svmconfig.Raids.Exfils.CarExtractTime;
                            exits.ExfiltrationTimePVE = svmconfig.Raids.Exfils.CarExtractTime;
                            AdjustExit(names.Base.Name, exits, "Car");
                        }
                        if (svmconfig.Raids.Exfils.CoopPaid && svmconfig.Raids.EnableCarCoop && exits.PassageRequirement == RequirementState.ScavCooperation)
                        {
                            AdjustExit(names.Base.Name, exits, "Coop");
                        }
                        if (svmconfig.Raids.Exfils.ExtendedExtracts)
                        {
                            AdjustExit(names.Base.Name, exits, "Side");
                        }
                        if (svmconfig.Raids.Exfils.FreeCoop && svmconfig.Raids.EnableCarCoop && exits.PassageRequirement == RequirementState.ScavCooperation)
                        {
                            FreeExit(exits);
                        }
                        if (svmconfig.Raids.Exfils.ChanceExtracts && exits.Chance is not null)
                        {
                            exits.Chance = 100;
                            exits.ChancePVE = 100;
                        }
                    }
                }
            }
        }
        public void FreeExit(Exit exit)
        {
            exit.PassageRequirement = RequirementState.None;
            exit.ExfiltrationType = ExfiltrationType.Individual;
            exit.Id = null;
            exit.Count = 0;
            exit.PlayersCount = 0;
            exit.RequirementTip = "Free Exit";
            exit.RequiredSlot = null;
        }
        public void PaidCoopDetails(Exit exit)
        {
            exit.PlayersCount = 0;
            exit.PlayersCountPVE = 0;
            exit.ExfiltrationType = ExfiltrationType.SharedTimer;
            exit.PassageRequirement = RequirementState.TransferItem;
            exit.Id = "5449016a4bdc2d6f028b456f";
        }
        public void AdjustExit(string names, Exit exits, string type)//This is horrible, but i'd handle all the operations within a single cycle, win scenario comparing to my JS code. Gotta refactor it later
        {
            switch (names)
            {
                case "Woods":
                    if (type == "Side")
                    {
                        exits.EntryPoints = "House,Old Station";
                    }
                    if (type == "Car")
                    {
                        if (svmconfig.Raids.Exfils.CarWoods != 0)
                        {
                            exits.Count = svmconfig.Raids.Exfils.CarWoods;
                        }
                        else FreeExit(exits);
                    }
                    if (type == "Coop")
                    {
                        if (svmconfig.Raids.Exfils.CoopPaidWoods != 0)
                        {
                            PaidCoopDetails(exits);
                            exits.Count = svmconfig.Raids.Exfils.CoopPaidWoods;
                        }
                        else FreeExit(exits);
                    }
                    break;
                case "Interchange":
                    if (type == "Side")
                    {
                        exits.EntryPoints = "MallSE,MallNW";
                    }
                    if (type == "Car")
                    {
                        if (svmconfig.Raids.Exfils.CarInterchange != 0)
                        {
                            exits.Count = svmconfig.Raids.Exfils.CarInterchange;
                        }
                        else FreeExit(exits);
                    }
                    if (type == "coop")
                    {
                        if (svmconfig.Raids.Exfils.CoopPaidInterchange != 0)
                        {
                            PaidCoopDetails(exits);
                            exits.Count = svmconfig.Raids.Exfils.CoopPaidInterchange;
                        }
                        else FreeExit(exits);
                    }
                    break;
                case "Customs":
                    if (type == "Side")
                    {
                        exits.EntryPoints = "Customs,Boiler Tanks";
                    }
                    if (type == "Car")
                    {
                        if (svmconfig.Raids.Exfils.CarCustoms != 0)
                        {
                            exits.Count = svmconfig.Raids.Exfils.CarCustoms;
                        }
                        else FreeExit(exits);
                    }
                    if (type == "Coop")
                    {
                        if (svmconfig.Raids.Exfils.CoopPaidCustoms != 0)
                        {
                            PaidCoopDetails(exits);
                            exits.Count = svmconfig.Raids.Exfils.CoopPaidCustoms;
                        }
                        else FreeExit(exits);
                    }
                    break;
                case "Streets of Tarkov":
                    if (type == "Side")
                    {
                        exits.EntryPoints = "E1_2,E6_1,E2_3,E3_4,E4_5,E5_6,E6_1";
                    }
                    if (type == "Car")
                    {
                        if (svmconfig.Raids.Exfils.CarStreets != 0)
                        {
                            exits.Count = svmconfig.Raids.Exfils.CarStreets;
                        }
                        else FreeExit(exits);
                    }
                    else
                    {
                        if (svmconfig.Raids.Exfils.CoopPaidStreets != 0)
                        {
                            PaidCoopDetails(exits);
                            exits.Count = svmconfig.Raids.Exfils.CoopPaidStreets;
                        }
                        else FreeExit(exits);
                    }
                    break;
                case "Lighthouse":
                    if (type == "Side")
                    {
                        exits.EntryPoints = "Tunnel,North";
                    }
                    if (type == "Car")
                    {
                        if (svmconfig.Raids.Exfils.CarLighthouse != 0)
                        {
                            exits.Count = svmconfig.Raids.Exfils.CarLighthouse;
                        }
                        else FreeExit(exits);
                    }
                    else
                    {
                        if (svmconfig.Raids.Exfils.CoopPaidLighthouse != 0)
                        {
                            PaidCoopDetails(exits);
                            exits.Count = svmconfig.Raids.Exfils.CoopPaidLighthouse;
                        }
                        else FreeExit(exits);
                    }
                    break;
                case "Shoreline":
                    if (type == "Side")
                    {
                        exits.EntryPoints = "Village,Riverside";
                    }
                    if (type == "Car")
                    {
                        if (svmconfig.Raids.Exfils.CarShoreline != 0)
                        {
                            exits.Count = svmconfig.Raids.Exfils.CarShoreline;
                        }
                        else FreeExit(exits);
                    }
                    else
                    {
                        if (svmconfig.Raids.Exfils.CoopPaidShoreline != 0)
                        {
                            PaidCoopDetails(exits);
                            exits.Count = svmconfig.Raids.Exfils.CoopPaidShoreline;
                        }
                        else FreeExit(exits);
                    }
                    break;
                case "Sandbox":
                    if (type == "Side")
                    {
                        exits.EntryPoints = "west,east";
                    }
                    if (type == "Car")
                    {
                        if (svmconfig.Raids.Exfils.CarSandbox != 0)
                        {
                            exits.Count = svmconfig.Raids.Exfils.CarSandbox;
                        }
                        else FreeExit(exits);
                    }
                    else
                    {
                        if (svmconfig.Raids.Exfils.CoopPaidSandbox != 0)
                        {
                            PaidCoopDetails(exits);
                            exits.Count = svmconfig.Raids.Exfils.CoopPaidSandbox;
                        }
                        else FreeExit(exits);
                    }
                    break;
                case "Reserve":
                    if (type == "Coop")
                    {
                        if (svmconfig.Raids.Exfils.CoopPaidReserve != 0)
                        {
                            PaidCoopDetails(exits);
                            exits.Count = svmconfig.Raids.Exfils.CoopPaidReserve;
                        }
                        else FreeExit(exits);
                    }//Only this statement happens, because reserve doesn't have car extract, it is needless but i left it just for sanity.
                    break;

            }
        }
    }
}
