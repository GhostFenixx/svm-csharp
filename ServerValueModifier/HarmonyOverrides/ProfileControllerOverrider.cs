using Greed.Models;
using HarmonyLib;
using SPTarkov.Common.Models.Logging;
using SPTarkov.DI.Annotations;
using SPTarkov.Reflection.Patching;
using SPTarkov.Server.Core.Controllers;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Generators;
using SPTarkov.Server.Core.Generators.Bot;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Helpers.Profile;
using SPTarkov.Server.Core.Helpers.Server;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Routers;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services;
using SPTarkov.Server.Core.Services.Profile;
using SPTarkov.Server.Core.Utils;
using SPTarkov.Server.Core.Utils.Cloners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ServerValueModifier.HarmonyOverrides
{
    [Injectable]

    public class ProfileControllerAsyncPatch : AbstractPatch
    {
        private static ModHelper modhelper;
        private static PlayerScavGenerator _playerScavGenerator;

        protected override MethodBase GetTargetMethod()
        {
            return typeof(ProfileController).GetMethod(nameof(ProfileController.GeneratePlayerScav))!;
        }
        [PatchPrefix]
        public static bool Prefix(MongoId sessionId)
        {
            try//try-catch in case no config
            {
                PmcData scavdata = _playerScavGenerator.Generate(sessionId);
                MainClass.MainConfig cf = new SVMConfig(modhelper).CallConfig();
                if (cf.Scav.EnableScav)
                {
                    if (cf.Scav.EnableStats)
                    {
                        scavdata.Health.Energy.Maximum = cf.Scav.ScavStats.MaxEnergy;
                        scavdata.Health.Hydration.Maximum = cf.Scav.ScavStats.MaxHydration;
                    }

                    if (cf.Scav.EnableScavHealth)
                    {
                        Dictionary<string, SPTarkov.Server.Core.Models.Eft.Common.Tables.BodyPartHealth>? health = scavdata.Health.BodyParts;
                        LocalRaidPatch.HealthEdit(health, cf.Scav.Health, "Current"); //Since scav should be at full health at every generation.
                        LocalRaidPatch.HealthEdit(health, cf.Scav.Health, "Maximum");
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
                }
                return false;
            }
            catch
            {
                return true;
            }
        }
        public bool CheckPocketQuest(PmcData pmcdata)
        {
            return pmcdata.Quests.Any(quest => quest.QId == QuestTpl.OLD_PATTERNS && quest.Status == QuestStatusEnum.Success);
        }
    }
}
//internal class ProfileControllerOverrider(ISptLogger<ProfileController> _logger, ModHelper modhelper, SaveServer _saveServer, ProfileFixerService _profileFixerService,
//    CreateProfileService _createProfileService, PlayerScavGenerator _playerScavGenerator, ProfileHelper _profileHelper) 
//    : ProfileController(_logger, _saveServer, _createProfileService,_profileFixerService, _playerScavGenerator, _profileHelper)
//{
//    public PmcData GeneratePlayerScav(MongoId sessionId) //This is recreation of cursed method - rewrite a scav stats on generation when survived.
//    {
//        try
//        {
//            PmcData scavdata = _playerScavGenerator.Generate(sessionId);
//            MainClass.MainConfig cf = new SVMConfig(modhelper).CallConfig();
//            if (cf.Scav.EnableScav)
//            {
//                if (cf.Scav.EnableStats)
//                {
//                    scavdata.Health.Energy.Maximum = cf.Scav.ScavStats.MaxEnergy;
//                    scavdata.Health.Hydration.Maximum = cf.Scav.ScavStats.MaxHydration;
//                }

//                if (cf.Scav.EnableScavHealth)
//                {
//                    Dictionary<string, SPTarkov.Server.Core.Models.Eft.Common.Tables.BodyPartHealth>? health = scavdata.Health.BodyParts;
//                    LocalRaidOverrider.HealthEdit(health, cf.Scav.Health, "Current"); //Since scav should be at full health at every generation.
//                    LocalRaidOverrider.HealthEdit(health, cf.Scav.Health, "Maximum");
//                }
//                if (cf.Scav.ScavCustomPockets)
//                {
//                    foreach (var item in scavdata.Inventory.Items)
//                    {
//                        if (item.SlotId == "Pockets")
//                        {
//                            item.Template = "a8edfb0bce53d103d3f6219b";
//                        }
//                    }
//                }
//            }
//            return scavdata;
//        }
//        catch
//        {
//            return _playerScavGenerator.Generate(sessionId);
//        }
//    }
//}
