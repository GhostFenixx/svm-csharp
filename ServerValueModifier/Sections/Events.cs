using Greed.Models;
using SPTarkov.Common.Extensions;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Spt.Config;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services;
using SPTarkov.Server.Core.Utils;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using static System.Net.Mime.MediaTypeNames;

namespace ServerValueModifier.Sections
{
    internal class Events(ISptLogger<SVM> logger, ConfigServer configServer, DatabaseService databaseService, MainClass.MainConfig svmconfig, SeasonalEventService seasonalEvent, ModHelper modhelper)
    {
        public void EventsSection()
        {
            //globals init
            Globals globals = databaseService.GetGlobals();
            var locs = databaseService.GetLocations();
            var locsconfig = configServer.GetConfig<LocationConfig>();
            var botconfig = configServer.GetConfig<BotConfig>();
            string wavesfile = modhelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
            JsonNode loadName = JsonNode.Parse(File.ReadAllText( Path.Combine(wavesfile, "Misc", "Waves.json")));//This is for more complex setups with guards
            BossLocationSpawn kaban = JsonSerializer.Deserialize<BossLocationSpawn>(loadName!["Kaban"]!.ToString(), JsonUtil.JsonSerializerOptionsIndented)!;
            BossLocationSpawn kolontay  = JsonSerializer.Deserialize<BossLocationSpawn>(loadName!["Kolontay"]!.ToString(), JsonUtil.JsonSerializerOptionsIndented)!;
            BossLocationSpawn goons = JsonSerializer.Deserialize<BossLocationSpawn>(loadName!["Goons"]!.ToString(), JsonUtil.JsonSerializerOptionsIndented)!;
            BossLocationSpawn glukhar = JsonSerializer.Deserialize<BossLocationSpawn>(loadName!["Glukhar"]!.ToString(), JsonUtil.JsonSerializerOptionsIndented)!;

            SeasonalEventConfig season = configServer.GetConfig<SeasonalEventConfig>();
            if (svmconfig.Raids.RaidEvents.Christmas && !svmconfig.Raids.RaidEvents.DisableEvents)//Avoid forcing events if we want to avoid detecting them as well
            {
                seasonalEvent.ForceSeasonalEvent(SeasonalEventType.Christmas);
            }
            if (svmconfig.Raids.RaidEvents.Halloween && !svmconfig.Raids.RaidEvents.DisableEvents)
            {
                seasonalEvent.ForceSeasonalEvent(SeasonalEventType.Halloween);
            }

            if (svmconfig.Raids.RaidEvents.KillaFactory)//Easier to create a standalone wave than finding existing one and editing follower, also handy if we want it to be a chance
            {
                locs.Factory4Day.Base.BossLocationSpawn.Add(CreateBasicBossWave("bossKilla", svmconfig.Raids.RaidEvents.KillaFactoryChance, locs.Factory4Day.Base.OpenZones, "followerBully", "0"));
                locs.Factory4Night.Base.BossLocationSpawn.Add(CreateBasicBossWave("bossKilla", svmconfig.Raids.RaidEvents.KillaFactoryChance, locs.Factory4Day.Base.OpenZones, "followerBully", "0"));
            }

            if (svmconfig.Raids.RaidEvents.TagillaInterchange)
            {
                foreach (var bosses in locs.Interchange.Base.BossLocationSpawn)
                {
                    if (bosses.BossName == "bossKilla")
                    {
                        bosses.BossEscortAmount = "1";
                        bosses.BossEscortType = "bossTagilla";
                    }
                }
            }

            if (svmconfig.Raids.RaidEvents.BossesOnReserve)
            {
                locs.RezervBase.Base.BossLocationSpawn.Add(CreateBasicBossWave("bossKilla", 100, locs.RezervBase.Base.OpenZones, "followerBully", "0"));
                locs.RezervBase.Base.BossLocationSpawn.Add(CreateBasicBossWave("bossTagilla", 100, locs.RezervBase.Base.OpenZones, "followerBully", "0"));
                locs.RezervBase.Base.BossLocationSpawn.Add(CreateBasicBossWave("bossSanitar", 100, locs.RezervBase.Base.OpenZones, "followerSanitar", "2"));
                locs.RezervBase.Base.BossLocationSpawn.Add(CreateBasicBossWave("bossKojaniy", 100, locs.RezervBase.Base.OpenZones, "followerKojaniy", "2"));
                locs.RezervBase.Base.BossLocationSpawn.Add(CreateBasicBossWave("bossBully", 100, locs.RezervBase.Base.OpenZones, "followerBully", "4"));
                //JsonNode loadName = JsonNode.Parse(File.ReadAllText("Waves.json"));//Hello Archangel, TODO full refactor so lacy's eyes won't burn seeing json usage. 
                if (svmconfig.Raids.RaidEvents.IncludeStreetBosses)
                {
                    //BossLocationSpawn kolontay = JsonSerializer.Deserialize<BossLocationSpawn>(loadName!["Kolontay"]!.ToString(), JsonUtil.JsonSerializerOptionsIndented)!;
                    //BossLocationSpawn kaban = JsonSerializer.Deserialize<BossLocationSpawn>(loadName!["Kaban"]!.ToString(), JsonUtil.JsonSerializerOptionsIndented)!;
                    kolontay.BossZone = locs.RezervBase.Base.OpenZones;
                    kaban.BossZone = locs.RezervBase.Base.OpenZones;
                    locs.RezervBase.Base.BossLocationSpawn.Add(kolontay);
                    locs.RezervBase.Base.BossLocationSpawn.Add(kaban);
                }
            }

            if (svmconfig.Raids.RaidEvents.BossesOnHealthResort)
            {
                //JsonNode loadName = JsonNode.Parse(File.ReadAllText("Waves.json"));//TODO as well
                goons.BossZone = "ZoneSanatorium1,ZoneSanatorium2";
                kaban.BossZone = "ZoneSanatorium1,ZoneSanatorium2";
                kolontay.BossZone = "ZoneSanatorium1,ZoneSanatorium2";
                if (svmconfig.Raids.RaidEvents.HealthResortIncludeGuards)
                {
                    locs.Shoreline.Base.BossLocationSpawn.Add(CreateBasicBossWave("bossKilla", 100, "ZoneSanatorium1,ZoneSanatorium2", "followerBully", "0"));
                    locs.Shoreline.Base.BossLocationSpawn.Add(CreateBasicBossWave("bossTagilla", 100, "ZoneSanatorium1,ZoneSanatorium2", "followerBully", "0"));
                    locs.Shoreline.Base.BossLocationSpawn.Add(CreateBasicBossWave("bossSanitar", 100, "ZoneSanatorium1,ZoneSanatorium2", "followerSanitar", "2"));
                    locs.Shoreline.Base.BossLocationSpawn.Add(CreateBasicBossWave("bossKojaniy", 100, "ZoneSanatorium1,ZoneSanatorium2", "followerKojaniy", "2"));
                    locs.Shoreline.Base.BossLocationSpawn.Add(CreateBasicBossWave("bossBully", 100, "ZoneSanatorium1,ZoneSanatorium2", "followerBully", "4"));
                    locs.Shoreline.Base.BossLocationSpawn.Add(goons);
                    locs.Shoreline.Base.BossLocationSpawn.Add(kaban);
                    locs.Shoreline.Base.BossLocationSpawn.Add(kolontay);
                    locs.Shoreline.Base.BossLocationSpawn.Add(glukhar);
                }
                else
                {
                    locs.Shoreline.Base.BossLocationSpawn.Add(CreateBasicBossWave("bossKilla", 100, "ZoneSanatorium1,ZoneSanatorium2", "followerBully", "0"));
                    locs.Shoreline.Base.BossLocationSpawn.Add(CreateBasicBossWave("bossTagilla", 100, "ZoneSanatorium1,ZoneSanatorium2", "followerBully", "0"));
                    locs.Shoreline.Base.BossLocationSpawn.Add(CreateBasicBossWave("bossSanitar", 100, "ZoneSanatorium1,ZoneSanatorium2", "followerSanitar", "0"));
                    locs.Shoreline.Base.BossLocationSpawn.Add(CreateBasicBossWave("bossKojaniy", 100, "ZoneSanatorium1,ZoneSanatorium2", "followerKojaniy", "0"));
                    locs.Shoreline.Base.BossLocationSpawn.Add(CreateBasicBossWave("bossSanitar", 100, "ZoneSanatorium1,ZoneSanatorium2", "followerSanitar", "0"));
                    locs.Shoreline.Base.BossLocationSpawn.Add(CreateBasicBossWave("bossBoar", 100, "ZoneSanatorium1,ZoneSanatorium2", "followerKojaniy", "0"));
                    locs.Shoreline.Base.BossLocationSpawn.Add(CreateBasicBossWave("bossKolontay", 100, "ZoneSanatorium1,ZoneSanatorium2", "followerSanitar", "0"));
                    locs.Shoreline.Base.BossLocationSpawn.Add(CreateBasicBossWave("bossGluhar", 100, "ZoneSanatorium1,ZoneSanatorium2", "followerGluharSecurity", "0"));
                    locs.Shoreline.Base.BossLocationSpawn.Add(goons);
                }
            }
            if (svmconfig.Raids.RaidEvents.BossesOnCustoms)
            {
                locs.Bigmap.Base.BossLocationSpawn.Add(CreateBasicBossWave("bossKilla", 100, "ZoneOldAZS", "followerBully", "0"));
                locs.Bigmap.Base.BossLocationSpawn.Add(CreateBasicBossWave("bossKojaniy", 100, "ZoneFactoryCenter", "followerKojaniy", "2"));
                locs.Bigmap.Base.BossLocationSpawn.Add(CreateBasicBossWave("bossSanitar", 100, "ZoneGasStation", "followerSanitar", "2"));
                locs.Bigmap.Base.BossLocationSpawn.Add(CreateBasicBossWave("bossTagilla", 100, "ZoneOldAZS", "followerBully", "0"));
            }
                if (svmconfig.Raids.RaidEvents.CultistBosses)
            {
                BossLocationSpawn cultists = JsonSerializer.Deserialize<BossLocationSpawn>(loadName!["Cultists"]!.ToString(), JsonUtil.JsonSerializerOptionsIndented)!;
                cultists.BossZone = locs.Bigmap.Base.OpenZones;
                cultists.BossChance = svmconfig.Raids.RaidEvents.CultistBossesChance;
                locs.Bigmap.Base.BossLocationSpawn.Add(cultists);
                cultists.BossZone = locs.Shoreline.Base.OpenZones;
                locs.Shoreline.Base.BossLocationSpawn.Add(cultists);
                cultists.BossZone = locs.Woods.Base.OpenZones;
                locs.Woods.Base.BossLocationSpawn.Add(cultists);
                cultists.BossZone = locs.Lighthouse.Base.OpenZones;
                locs.Lighthouse.Base.BossLocationSpawn.Add(cultists);
            }
            if (svmconfig.Raids.RaidEvents.GoonsFactory)
            {
                goons.BossZone = "BotZone";
                goons.BossChance = svmconfig.Raids.RaidEvents.GoonsFactoryChance;
                locs.Factory4Day.Base.BossLocationSpawn.Add(goons);
                locs.Factory4Night.Base.BossLocationSpawn.Add(goons);
            }
            if (svmconfig.Raids.RaidEvents.GlukharLabs)
            {
                glukhar.BossZone = "BotZoneFloor1,BotZoneFloor2";
                locs.Laboratory.Base.BossLocationSpawn.Add(glukhar);
            }
            foreach (Location loc in locs.GetDictionary().Values)
            {
                foreach (var bosses in loc.Base.BossLocationSpawn)
                {
                    if (bosses.BossName == "arenaFighterEvent" && loc.Base.Name == "bigmap")
                    {
                        bosses.BossChance = svmconfig.Raids.RaidEvents.HoundsCustoms;
                    }
                    if (bosses.BossName == "arenaFighterEvent" && loc.Base.Name == "woods")
                    {
                        bosses.BossChance = svmconfig.Raids.RaidEvents.HoundsWoods;
                    }
                    if (bosses.BossName == "peacemaker" && loc.Base.Name == "shoreline")
                    {
                        bosses.BossChance = svmconfig.Raids.RaidEvents.PeaceFighters;
                    }
                    if (bosses.BossName == "skier" && loc.Base.Name == "bigmap")
                    {
                        bosses.BossChance = svmconfig.Raids.RaidEvents.SkierFighters;
                    }
                }
            }
        }
        public BossLocationSpawn CreateBasicBossWave(string bossname, double bosschance, string zones, string escorttype, string escortamount)
        {
            return new BossLocationSpawn
            {
                BossName = bossname,
                BossChance = bosschance,
                BossZone = zones,
                IsBossPlayer = false,
                BossDifficulty = "normal",
                BossEscortType = escorttype,
                BossEscortDifficulty = "normal",
                BossEscortAmount = escortamount,
                Time = -1
            };
        }
    }
}
