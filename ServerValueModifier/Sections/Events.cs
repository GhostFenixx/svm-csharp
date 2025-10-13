using Greed.Models;
using SPTarkov.Common.Extensions;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Eft.Common;
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
    internal class Events(ISptLogger<SVM> logger, ConfigServer configServer, DatabaseService databaseService, MainClass.MainConfig svmconfig, ModHelper modhelper)
    {
        public void EventsSection()
        {
            //globals init
            Globals globals = databaseService.GetGlobals();
            var locs = databaseService.GetLocations();
            var locsconfig = configServer.GetConfig<LocationConfig>();
            string wavesfile = modhelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
            JsonNode loadName = JsonNode.Parse(File.ReadAllText( Path.Combine(wavesfile, "Misc", "Waves.json")));//This is for more complex setups with guards
            if (svmconfig.Raids.RaidEvents.AITypeOverride)
            {
                string aitype = "";
                WildSpawnType aispawntype = WildSpawnType.assault;//I hate to declare it with a variable but otherwise it throws an error, i'm missing something, TODO
                switch (svmconfig.Raids.RaidEvents.AIType)//Select type of AI we will be converting to
                {
                    case 0: aitype = "pmcbot"; aispawntype = WildSpawnType.pmcBot; break;
                    case 1: aitype = "exusec"; aispawntype = WildSpawnType.exUsec; break;
                    case 2: aitype = "sectantwarrior"; aispawntype = WildSpawnType.sectantWarrior; break;
                    case 3: aitype = "pmcBear"; aispawntype = WildSpawnType.pmcBEAR; break;
                    case 4: aitype = "pmcUsec"; aispawntype = WildSpawnType.pmcUSEC; break;
                }
                foreach (var loc in locs.GetDictionary().Values) // DB > Locations > base
                {
                    foreach (var wave in loc.Base.Waves)
                    {
                        wave.WildSpawnType = aispawntype;
                    }
                }
                foreach (var loc in locsconfig.CustomWaves.Boss) // Config > location
                {
                    foreach (var wave in loc.Value)
                    {
                        wave.BossName = aitype;
                        wave.BossEscortType = aitype;
                    }
                }
                foreach (var loc in locsconfig.CustomWaves.Normal)
                {
                    foreach (var wave in loc.Value)
                    {
                        wave.WildSpawnType = aispawntype;
                    }
                }
            }

            if (svmconfig.Raids.RaidEvents.KillaFactory)
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

                BossLocationSpawn kolontay = JsonSerializer.Deserialize<BossLocationSpawn>(loadName!["Kolontay"]!.ToString(), JsonUtil.JsonSerializerOptionsIndented)!;
                BossLocationSpawn kaban = JsonSerializer.Deserialize<BossLocationSpawn>(loadName!["Kaban"]!.ToString(), JsonUtil.JsonSerializerOptionsIndented)!;
                kolontay.BossZone = locs.RezervBase.Base.OpenZones;
                kaban.BossZone = locs.RezervBase.Base.OpenZones;
                locs.RezervBase.Base.BossLocationSpawn.Add(kolontay);
                locs.RezervBase.Base.BossLocationSpawn.Add(kaban);
            }

            if (svmconfig.Raids.RaidEvents.BossesOnHealthResort)
            {
                //JsonNode loadName = JsonNode.Parse(File.ReadAllText("Waves.json"));//TODO as well
                BossLocationSpawn goons = JsonSerializer.Deserialize<BossLocationSpawn>(loadName!["Kaban"]!.ToString(), JsonUtil.JsonSerializerOptionsIndented)!;
                goons.BossZone = "ZoneSanatorium1,ZoneSanatorium2";
                if (svmconfig.Raids.RaidEvents.HealthResortIncludeGuards)
                {
                    locs.Shoreline.Base.BossLocationSpawn.Add(CreateBasicBossWave("bossKilla", 100, "ZoneSanatorium1,ZoneSanatorium2", "followerBully", "0"));
                    locs.Shoreline.Base.BossLocationSpawn.Add(CreateBasicBossWave("bossTagilla", 100, "ZoneSanatorium1,ZoneSanatorium2", "followerBully", "0"));
                    locs.Shoreline.Base.BossLocationSpawn.Add(CreateBasicBossWave("bossSanitar", 100, "ZoneSanatorium1,ZoneSanatorium2", "followerSanitar", "2"));
                    locs.Shoreline.Base.BossLocationSpawn.Add(CreateBasicBossWave("bossKojaniy", 100, "ZoneSanatorium1,ZoneSanatorium2", "followerKojaniy", "2"));
                    locs.Shoreline.Base.BossLocationSpawn.Add(CreateBasicBossWave("bossBully", 100, "ZoneSanatorium1,ZoneSanatorium2", "followerBully", "4"));
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
                    locs.Shoreline.Base.BossLocationSpawn.Add(goons);
                }
            }
            if (svmconfig.Raids.RaidEvents.BossesOnCustoms)
            {
                locs.Bigmap.Base.BossLocationSpawn.Add(CreateBasicBossWave("bossKilla", 100, "ZoneOldAZS", "followerbully", "0"));
                locs.Bigmap.Base.BossLocationSpawn.Add(CreateBasicBossWave("bossKojaniy", 100, "ZoneFactoryCenter", "followerKojaniy", "2"));
                locs.Bigmap.Base.BossLocationSpawn.Add(CreateBasicBossWave("bossSanitar", 100, "ZoneGasStation", "followerSanitar", "2"));
                locs.Bigmap.Base.BossLocationSpawn.Add(CreateBasicBossWave("bossTagilla", 100, "ZoneOldAZS", "followerbully", "0"));
            }
                if (svmconfig.Raids.RaidEvents.CultistBosses)
            {
                BossLocationSpawn cultists = JsonSerializer.Deserialize<BossLocationSpawn>(loadName!["Cultists"]!.ToString(), JsonUtil.JsonSerializerOptionsIndented)!;
                cultists.BossZone = locs.Bigmap.Base.OpenZones;
                cultists.BossChance = svmconfig.Raids.RaidEvents.CultistBossesChance;
                locs.Bigmap.Base.BossLocationSpawn.Add(cultists);//Maybe i should short that, another TODO, heh.
                cultists.BossZone = locs.Shoreline.Base.OpenZones;
                locs.Shoreline.Base.BossLocationSpawn.Add(cultists);
                cultists.BossZone = locs.Woods.Base.OpenZones;
                locs.Woods.Base.BossLocationSpawn.Add(cultists);
                cultists.BossZone = locs.Lighthouse.Base.OpenZones;
                locs.Lighthouse.Base.BossLocationSpawn.Add(cultists);
            }
            if (svmconfig.Raids.RaidEvents.GoonsFactory)
            {
                BossLocationSpawn goons = JsonSerializer.Deserialize<BossLocationSpawn>(loadName!["Goons"]!.ToString(), JsonUtil.JsonSerializerOptionsIndented)!;
                goons.BossZone = "BotZone";
                goons.BossChance = svmconfig.Raids.RaidEvents.GoonsFactoryChance;
                locs.Factory4Day.Base.BossLocationSpawn.Add(goons);
                locs.Factory4Night.Base.BossLocationSpawn.Add(goons);
            }
            if (svmconfig.Raids.RaidEvents.GlukharLabs)
            {
                BossLocationSpawn glukhar = JsonSerializer.Deserialize<BossLocationSpawn>(loadName!["Glukhar"]!.ToString(), JsonUtil.JsonSerializerOptionsIndented)!;
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
