using Greed.Models;
using SPTarkov.Common.Extensions;
using SPTarkov.DI.Annotations;
using SPTarkov.Reflection.Patching;
using SPTarkov.Server.Core.Callbacks;
using SPTarkov.Server.Core.Controllers;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Helpers.Bot;
using SPTarkov.Server.Core.Helpers.Profile;
using SPTarkov.Server.Core.Helpers.Server;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Eft.Match;
using SPTarkov.Server.Core.Models.Eft.Profile;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Spt.Config;
using SPTarkov.Server.Core.Models.Spt.Tables;
using SPTarkov.Server.Core.Services;
using SPTarkov.Server.Core.Services.Bot;
using SPTarkov.Server.Core.Services.InRaid;
using SPTarkov.Server.Core.Services.Profile;
using SPTarkov.Server.Core.Utils;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace ServerValueModifier.HarmonyOverrides
{
    [Injectable]

    public class LocalRaidPatch : AbstractPatch
    {
        private static ProfileHelper _profileHelper;
        private static ModHelper _modHelper;
        private static HttpResponseUtil _httpResponseUtil;
        private static MatchController _matchController;
        public LocalRaidPatch(ProfileHelper profileHelper, ModHelper modHelper, HttpResponseUtil httpResponseUtil)
        {
            _profileHelper = profileHelper;
            _modHelper = modHelper;
            _httpResponseUtil = httpResponseUtil;
        }
        protected override MethodBase GetTargetMethod()
        {
            return typeof(MatchCallbacks).GetMethod(nameof(MatchCallbacks.EndLocalRaidAsync));
        }
        [PatchPrefix]
        public static bool Prefix(ref ValueTask<string> __result, MongoId sessionID, EndLocalRaidRequestData info, CancellationToken cancellationToken)
        {
            try//try-catch in case no config
            {
                MainClass.MainConfig cf = new SVMConfig(_modHelper).CallConfig();
                if (cf.Raids.RaidStartup.SaveLoot && cf.Raids.EnableRaids && cf.Raids.RaidStartup.EnableRaidStartup)
                {

                    __result = new ValueTask<string>(_httpResponseUtil.NullResponse());
                    //_httpResponseUtil.NullResponse();   //Bad writing, still - if Section and subsection is on AND practice mode(saveloot) is on - ignore any changes to raid, including scav raids.
                    return false;
                }
                else
                {
                    if (info.Results.Profile.Info.Side != "Savage" && info.Results.Result != ExitStatus.TRANSIT && cf.Raids.EnableRaids)
                    {
                        DefineRaidStatus(cf, info);
                        if (info.Results.Result == ExitStatus.TRANSIT)
                        {
                            __result = new ValueTask<string>(_httpResponseUtil.NullResponse());
                            return false;// So, this is funny case, since my field is int converting to ExitStatus, my 'ignore raid' state hits same number as Transit,
                             // therefore knowing that initially it can't roll transit exit on entry i utilise it to ignore raid altogether
                             // Very doohickey.
                        }
                        else
                        {
                            return true;
                        }
                    }
                    if (cf.Scav.EnableScav)
                    {
                        if (info.Results.Result != ExitStatus.SURVIVED && info.Results.Result != ExitStatus.TRANSIT && info.Results.Result != ExitStatus.RUNNER && info.Results.Profile.Info.Side == "Savage")//Cursed, i hate it.
                        {
                            if (cf.Scav.EnableScavHealth || cf.Scav.EnableStats)
                            {
                                PmcData? scavdata = _profileHelper.GetScavProfile(sessionID);
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
                    return true;
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine("ERROR ERROR: " + ex);
                return true;
            }
        }
        public static void DefineRaidStatus(MainClass.MainConfig svmconfig, EndLocalRaidRequestData info)
        {
            switch (info.Results.Result)
            {
                case ExitStatus.SURVIVED:
                    info.Results.Result = (ExitStatus)svmconfig.Raids.OnSurvivedState;
                    break;
                case ExitStatus.KILLED:
                    info.Results.Result = (ExitStatus)svmconfig.Raids.OnKilledState;
                    break;
                case ExitStatus.LEFT:
                    info.Results.Result = (ExitStatus)svmconfig.Raids.OnLeftState;
                    break;
                case ExitStatus.RUNNER:
                    info.Results.Result = (ExitStatus)svmconfig.Raids.OnRunnerState;
                    break;
                case ExitStatus.MISSINGINACTION:
                    info.Results.Result = (ExitStatus)svmconfig.Raids.OnMIAState;
                    break;
            }
        }
        public static void HealthEdit(Dictionary<string, BodyPartHealth> Data, Greed.Models.PlayerData.Health values, string type)
        {
            PropertyInfo healthData = typeof(CurrentMinMax).GetProperty(type);
            healthData.SetValue(Data["Head"].Health, (double)values.Head);
            healthData.SetValue(Data["Chest"].Health, (double)values.Chest);
            healthData.SetValue(Data["Stomach"].Health, (double)values.Stomach);
            healthData.SetValue(Data["LeftArm"].Health, (double)values.LeftArm);
            healthData.SetValue(Data["LeftLeg"].Health, (double)values.LeftLeg);
            healthData.SetValue(Data["RightArm"].Health, (double)values.RightArm);
            healthData.SetValue(Data["RightLeg"].Health, (double)values.RightLeg);
        }
    }
}
//    public class LocalRaidOverrider(ModHelper modhelper, ProfileHelper pf, HttpResponseUtil httpResponseUtil, MatchController matchController, MatchTable matchTable, TemplateTable templateTable) : MatchCallbacks(matchTable, httpResponseUtil, matchController)
//    {
//        //public override ValueTask<string> StartLocalRaid(string url, StartLocalRaidRequestData info, MongoId sessionID)
//        //{
//        //    try
//        //    {
//        //        var locs = DatabaseService.GetLocations();
//        //        MainClass.MainConfig cf = new SVMConfig(modhelper).CallConfig();
//        //        Random rnd = new Random();
//        //        if (cf.PMC.AItoPMC.AIConverterEnable && cf.PMC.EnablePMC)
//        //        {
//        //            foreach (var loc in locs.GetDictionary().Values)
//        //            {
//        //                loc.Base.NewSpawn = false;
//        //                loc.Base.OfflineNewSpawn = false;
//        //                foreach (var wave in loc.Base.BossLocationSpawn)//This is so wrong, not the code, but wave system.
//        //                {
//        //                    int chance = rnd.Next(1, 101);
//        //                    if (wave.BossName.Equals("pmcBEAR") || wave.BossName.Equals("pmcUSEC") && (chance > (100 - cf.PMC.AItoPMC.PMCToScav)))
//        //                    {
//        //                        wave.BossName = "assault";
//        //                        wave.BossEscortType = "assault";
//        //                    }
//        //                }
//        //                foreach (var wave in loc.Base.Waves)
//        //                {
//        //                    int chance = rnd.Next(1, 101);
//        //                    if ((wave.WildSpawnType == WildSpawnType.assault || wave.WildSpawnType == WildSpawnType.assaultGroup) && chance > (100 - cf.PMC.AItoPMC.ScavToPMC))
//        //                    {
//        //                        WildSpawnType result = rnd.Next(2) > 0 ? WildSpawnType.pmcBEAR : WildSpawnType.pmcUSEC;
//        //                        wave.WildSpawnType = result;
//        //                    }
//        //                }
//        //            }
//        //        }
//        //    }
//        //    catch { }// handler for empty config.
//        //        return new ValueTask<string>(HttpResponseUtil.GetBody(MatchController.StartLocalRaid(sessionID, info)));
//        //}
//        public ValueTask<string> EndLocalRaidAsync(string url, EndLocalRaidRequestData info, MongoId sessionID, CancellationToken cancellationToken = default(CancellationToken)) //LocationLifeCycle
//        {
//            try
//            {
//                MainClass.MainConfig cf = new SVMConfig(modhelper).CallConfig();
//                //Random rnd = new Random();
//                //var locs = DatabaseService.GetLocations();
//                //if (cf.PMC.AItoPMC.AIConverterEnable && cf.PMC.EnablePMC)
//                //{
//                //    foreach (var loc in locs.GetDictionary().Values)
//                //    {
//                //        loc.Base.NewSpawn = false;
//                //        loc.Base.OfflineNewSpawn = false;
//                //        foreach (var wave in loc.Base.BossLocationSpawn)//This is so wrong, not the code, but wave system.
//                //        {
//                //            int chance = rnd.Next(1, 101);
//                //            if (wave.BossName.Equals("pmcBEAR") || wave.BossName.Equals("pmcUSEC")) // && (chance > (100 - svmcfg.PMC.AItoPMC.PMCToScav))
//                //            {
//                //                wave.BossName = "assault";
//                //                wave.BossEscortType = "assault";
//                //            }
//                //        }
//                //        foreach (var wave in loc.Base.Waves)
//                //        {
//                //            int chance = rnd.Next(1, 101);
//                //            if ((wave.WildSpawnType == WildSpawnType.assault || wave.WildSpawnType == WildSpawnType.assaultGroup) && chance > (100 - cf.PMC.AItoPMC.ScavToPMC))
//                //            {
//                //                WildSpawnType result = rnd.Next(2) > 0 ? WildSpawnType.pmcBEAR : WildSpawnType.pmcUSEC;
//                //                wave.WildSpawnType = result;
//                //            }
//                //        }
//                //    }
//                //}
//                if (cf.Raids.RaidStartup.SaveLoot && cf.Raids.EnableRaids && cf.Raids.RaidStartup.EnableRaidStartup)
//                {
//                    //Bad writing, still - if Section and subsection is on AND practice mode(saveloot) is on - ignore any changes to raid, including scav raids.
//                }
//                else
//                {
//                    if (info.Results.Profile.Info.Side != "Savage" && info.Results.Result != ExitStatus.TRANSIT && cf.Raids.EnableRaids)
//                    {
//                        DefineRaidStatus(cf, info);
//                        if (info.Results.Result != ExitStatus.TRANSIT)
//                        {                                                  // So, this is funny case, since my field is int converting to ExitStatus, my 'ignore raid' state hits same number as Transit,
//                            matchController.EndLocalRaidAsync(sessionID, info); // therefore knowing that initially it can't roll transit exit on entry i utilise it to ignore raid altogether
//                                                                           // Very doohickey.
//                        }
//                    }
//                    else
//                    {
//                        matchController.EndLocalRaidAsync(sessionID, info);
//                    }
//                    if (cf.Scav.EnableScav)
//                    {
//                        if (info.Results.Result != ExitStatus.SURVIVED && info.Results.Result != ExitStatus.TRANSIT && info.Results.Result != ExitStatus.RUNNER && info.Results.Profile.Info.Side == "Savage")//Cursed, i hate it.
//                        {
//                            if (cf.Scav.EnableScavHealth || cf.Scav.EnableStats)
//                            {
//                                PmcData? scavdata = pf.GetScavProfile(sessionID);
//                                if (cf.Scav.EnableStats)
//                                {
//                                    scavdata.Health.Energy.Maximum = cf.Scav.ScavStats.MaxEnergy;
//                                    scavdata.Health.Hydration.Maximum = cf.Scav.ScavStats.MaxHydration;
//                                }
//                                if (cf.Scav.ScavCustomPockets)
//                                {
//                                    foreach (var item in scavdata.Inventory.Items)
//                                    {
//                                        if (item.SlotId == "Pockets")
//                                        {
//                                            item.Template = "a8edfb0bce53d103d3f6219b";
//                                        }
//                                    }
//                                }
//                                if (cf.Scav.EnableScavHealth)
//                                {
//                                    Dictionary<string, SPTarkov.Server.Core.Models.Eft.Common.Tables.BodyPartHealth>? health = scavdata.Health.BodyParts;
//                                    HealthEdit(health, cf.Scav.Health, "Current"); //Since scav should be at full health at every generation.
//                                    HealthEdit(health, cf.Scav.Health, "Maximum");
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//            catch {
//                matchController.EndLocalRaidAsync(sessionID, info);
//            }
//            return new ValueTask<string>(httpResponseUtil.NullResponse());
//        }
//    }
//}
