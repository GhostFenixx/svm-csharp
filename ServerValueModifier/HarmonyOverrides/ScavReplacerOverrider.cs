using Greed.Models;
using SPTarkov.Common.Models.Logging;
using SPTarkov.DI.Annotations;
using SPTarkov.Reflection.Patching;
using SPTarkov.Server.Core.Controllers;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers.Profile;
using SPTarkov.Server.Core.Helpers.Server;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Spt.Config;
using SPTarkov.Server.Core.Models.Spt.Tables;
using SPTarkov.Server.Core.Services.Server;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ServerValueModifier.HarmonyOverrides
{

    [Injectable(TypePriority = OnLoadOrder.Preload)]
    public class ScavReplacerOverrider : AbstractPatch
    {
        private static ModHelper _modhelper = default!;
        private static LocationTable _locationTable = default!;

        public ScavReplacerOverrider(ModHelper modHelper, LocationTable locationTable, ISptLogger<SVM> logger)
        {
            _modhelper = modHelper;
            _locationTable = locationTable;
        }
        protected override MethodBase GetTargetMethod()
        {
            return typeof(PostDbLoadService).GetMethod("ReplaceScavWavesWithRole", BindingFlags.Instance | BindingFlags.NonPublic);
        }
        [PatchPrefix]
        public static bool Prefix(WildSpawnType newScavRole)
        {
            MainClass.MainConfig svmcfg = new SVMConfig(_modhelper).CallConfig();
            if (svmcfg.Raids.RaidEvents.AITypeOverride && svmcfg.Raids.EnableRaids) //Not sure we need to check for this but for sanity and possible mod compat? maybe?
            {
                WildSpawnType[] zombies = { WildSpawnType.infectedAssault, WildSpawnType.infectedPmc, WildSpawnType.infectedCivil, WildSpawnType.infectedLaborant };
                WildSpawnType[] bosses = { WildSpawnType.bossTagilla, WildSpawnType.bossKilla, WildSpawnType.bossKolontay, WildSpawnType.bossSanitar, WildSpawnType.bossKojaniy, WildSpawnType.bossGluhar, WildSpawnType.bossBoar, WildSpawnType.bossKnight, WildSpawnType.followerBirdEye, WildSpawnType.followerBigPipe, WildSpawnType.bossZryachiy, WildSpawnType.bossBully };
                WildSpawnType[] pmcs = { WildSpawnType.pmcBEAR, WildSpawnType.pmcUSEC };
                Random rnd = new Random();
                foreach (var location in _locationTable.GetDictionary().Values)
                {
                    if (location.Base?.Waves is null)
                    {
                        continue;
                    }

                    foreach (var wave in location.Base.Waves)
                    {
                        switch (svmcfg.Raids.RaidEvents.AIType)
                        {
                            case 0: wave.WildSpawnType = WildSpawnType.pmcBot; break;
                            case 1: wave.WildSpawnType = WildSpawnType.exUsec; break;
                            case 2: wave.WildSpawnType = WildSpawnType.sectantWarrior; break;
                            case 3: wave.WildSpawnType = zombies[rnd.Next(zombies.Length)]; break;
                            case 4: wave.WildSpawnType = bosses[rnd.Next(bosses.Length)]; break;
                            case 5: wave.WildSpawnType = pmcs[rnd.Next(pmcs.Length)]; break;
                        }
                    }
                }
                return false;
            }
            else
            {
                return true;
            }
        }
    }

        //if (svmcfg.Raids.RaidEvents.AITypeOverride)
        //{
        //    switch (svmcfg.Raids.RaidEvents.AIType) //2.0.1 change - using SPT functionality now. Waiting for 4.0.14? Potentially to expand this with multiple types.
        //    {
        //        case 0: bots.ReplaceScavWith = WildSpawnType.pmcBot; break;
        //        case 1: bots.ReplaceScavWith = WildSpawnType.exUsec; break;
        //        case 2: bots.ReplaceScavWith = WildSpawnType.sectantWarrior; break;
        //        case 3: bots.ReplaceScavWith = WildSpawnType.pmcBEAR; break;
        //        case 4: bots.ReplaceScavWith = WildSpawnType.pmcUSEC; break;
        //        case 5: bots.ReplaceScavWith = WildSpawnType.infectedAssault; break;
        //    }
        //}
}
