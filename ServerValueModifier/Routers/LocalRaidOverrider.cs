using Greed.Models;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Callbacks;
using SPTarkov.Server.Core.Controllers;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Eft.Match;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Services;
using SPTarkov.Server.Core.Utils;

namespace ServerValueModifier.Routers
{
    [Injectable]
    public class LocalRaidOverrider(ModHelper modhelper, ProfileHelper pf, HttpResponseUtil HttpResponseUtil, MatchController MatchController, DatabaseService DatabaseService) : MatchCallbacks(HttpResponseUtil, MatchController, DatabaseService)
    {
        public override ValueTask<string> StartLocalRaid(string url, StartLocalRaidRequestData info, MongoId sessionID)
        {
            var locs = DatabaseService.GetLocations();
            MainClass.MainConfig cf = new SVMConfig(modhelper).CallConfig();
            Random rnd = new Random();
            if (cf.PMC.AItoPMC.AIConverterEnable)
            {
                foreach (var loc in locs.GetDictionary().Values)
                {
                    loc.Base.NewSpawn = false;
                    loc.Base.OfflineNewSpawn = false;
                    foreach (var wave in loc.Base.BossLocationSpawn)//This is so wrong, not the code, but wave system.
                    {
                        int chance = rnd.Next(1, 101);
                        if (wave.BossName.Equals("pmcBEAR") || wave.BossName.Equals("pmcUSEC")) // && (chance > (100 - svmcfg.PMC.AItoPMC.PMCToScav))
                        {
                            wave.BossName = "assault";
                            wave.BossEscortType = "assault";
                        }
                    }
                    foreach (var wave in loc.Base.Waves)
                    {
                        int chance = rnd.Next(1, 101);
                        if ((wave.WildSpawnType == WildSpawnType.assault || wave.WildSpawnType == WildSpawnType.assaultGroup) && chance > (100 - cf.PMC.AItoPMC.ScavToPMC))
                        {
                            WildSpawnType result = rnd.Next(2) > 0 ? WildSpawnType.pmcBEAR : WildSpawnType.pmcUSEC;
                            wave.WildSpawnType = result;
                        }
                    }
                }
            }
            return new ValueTask<string>(HttpResponseUtil.GetBody(MatchController.StartLocalRaid(sessionID, info)));
        }
        public override ValueTask<string> EndLocalRaid(string url, EndLocalRaidRequestData info, MongoId sessionID) //LocationLifeCycle
        {
            try
            {
                MainClass.MainConfig cf = new SVMConfig(modhelper).CallConfig();
                Random rnd = new Random();
                var locs = DatabaseService.GetLocations();
                if (cf.PMC.AItoPMC.AIConverterEnable)
                {
                    foreach (var loc in locs.GetDictionary().Values)
                    {
                        loc.Base.NewSpawn = false;
                        loc.Base.OfflineNewSpawn = false;
                        foreach (var wave in loc.Base.BossLocationSpawn)//This is so wrong, not the code, but wave system.
                        {
                            int chance = rnd.Next(1, 101);
                            if (wave.BossName.Equals("pmcBEAR") || wave.BossName.Equals("pmcUSEC")) // && (chance > (100 - svmcfg.PMC.AItoPMC.PMCToScav))
                            {
                                wave.BossName = "assault";
                                wave.BossEscortType = "assault";
                            }
                        }
                        foreach (var wave in loc.Base.Waves)
                        {
                            int chance = rnd.Next(1, 101);
                            if ((wave.WildSpawnType == WildSpawnType.assault || wave.WildSpawnType == WildSpawnType.assaultGroup) && chance > (100 - cf.PMC.AItoPMC.ScavToPMC))
                            {
                                WildSpawnType result = rnd.Next(2) > 0 ? WildSpawnType.pmcBEAR : WildSpawnType.pmcUSEC;
                                wave.WildSpawnType = result;
                            }
                        }
                    }
                }
                if (cf.Raids.RaidStartup.SaveLoot)
                {

                }
                else
                {
                    if (info.Results.Result == ExitStatus.LEFT && info.Results.Profile.Info.Side != "Savage" && cf.Raids.SafeExit & cf.Raids.EnableRaids)
                    {
                        info.Results.Result = ExitStatus.RUNNER;
                    }
                    if (info.Results.Result != ExitStatus.SURVIVED && info.Results.Result != ExitStatus.TRANSIT && info.Results.Profile.Info.Side != "Savage" && cf.Raids.SaveGearAfterDeath && cf.Raids.EnableRaids)
                    {
                        info.Results.Result = ExitStatus.RUNNER;
                    }
                    MatchController.EndLocalRaid(sessionID, info);
                    if (cf.Scav.EnableScav)
                    {
                        if (info.Results.Result != ExitStatus.SURVIVED && info.Results.Result != ExitStatus.TRANSIT && info.Results.Result != ExitStatus.RUNNER && info.Results.Profile.Info.Side == "Savage")//Cursed, i hate it.
                        {
                            if (cf.Scav.EnableScavHealth || cf.Scav.EnableStats)
                            {
                                PmcData? scavdata = pf.GetScavProfile(sessionID);
                                if (cf.Scav.EnableStats)
                                {
                                    scavdata.Health.Energy.Maximum = cf.Scav.ScavStats.MaxEnergy;
                                    scavdata.Health.Hydration.Maximum = cf.Scav.ScavStats.MaxHydration;
                                }
                                if (cf.Scav.ScavCustomPockets)
                                {
                                    foreach (var item in scavdata.Inventory.Items)
                                    {
                                        if (item.SlotId == "Pockets")
                                        {
                                            item.Template = "a8edfb0bce53d103d3f6219b";
                                        }
                                    }
                                }
                                if (cf.Scav.EnableScavHealth)
                                {
                                    Dictionary<string, SPTarkov.Server.Core.Models.Eft.Common.Tables.BodyPartHealth>? health = scavdata.Health.BodyParts;
                                    HealthEdit(health, cf.Scav.Health, "Current"); //Since scav should be at full health at every generation.
                                    HealthEdit(health, cf.Scav.Health, "Maximum");
                                }
                            }
                        }
                    }
                }
                return new ValueTask<string>(HttpResponseUtil.NullResponse());
            }
            catch (Exception ex)
            {
                return new ValueTask<string>(HttpResponseUtil.NullResponse());
            }
        }

        public static void HealthEdit(Dictionary<string, BodyPartHealth>? Data, Greed.Models.PlayerData.Health values, string type)
        {
            if (type == "Current")
            {
                Data["Head"].Health.Current = values.Head;
                Data["Chest"].Health.Current = values.Chest;
                Data["Stomach"].Health.Current = values.Stomach;
                Data["LeftArm"].Health.Current = values.LeftArm;
                Data["LeftLeg"].Health.Current = values.LeftLeg;
                Data["RightArm"].Health.Current = values.RightArm;
                Data["RightLeg"].Health.Current = values.RightLeg;
            }
            if (type == "Maximum") //Horrible, TODO reflection with [Current/Maximum]
            {
                Data["Head"].Health.Maximum = values.Head;
                Data["Chest"].Health.Maximum = values.Chest;
                Data["Stomach"].Health.Maximum = values.Stomach;
                Data["LeftArm"].Health.Maximum = values.LeftArm;
                Data["LeftLeg"].Health.Maximum = values.LeftLeg;
                Data["RightArm"].Health.Maximum = values.RightArm;
                Data["RightLeg"].Health.Maximum = values.RightLeg;
            }
        }
    }
}
