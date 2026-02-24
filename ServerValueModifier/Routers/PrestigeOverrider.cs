using Greed.Models;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Controllers;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services;
using System.Xml.Linq;

namespace ServerValueModifier.Routers
{
    [Injectable]
    public class PrestigeOverrider(
    ISptLogger<PrestigeController> _logger,
    ProfileHelper _profileHelper,
    DatabaseService _databaseService,
    ModHelper modhelper,
    SaveServer _saveServer) : PrestigeController( _profileHelper, _databaseService, _saveServer)
    {
        public const string DEFAULT_POCKETS = "627a4e6b255f7527fb05a0f6";
        public const string UNHEARD_POCKETS = "65e080be269cbd5c5005e529";
        public const string CUSTOM_POCKETS = "a8edfb0bce53d103d3f62b9b";
        public const string SCAVCUSTOM_POCKETS = "a8edfb0bce53d103d3f6219b";
        public override Prestige GetPrestige(MongoId sessionId)
        {
            try
            {
                MainClass.MainConfig cf = new SVMConfig(modhelper).CallConfig();
                PmcData? pmcdata = _profileHelper.GetPmcProfile(sessionId);
                PmcData? scavdata = _profileHelper.GetScavProfile(sessionId);
                var allPockets = pmcdata.Inventory.Items.Where(item => item.SlotId == "Pockets");
                var hasCompletedPocketUpgradeQuest = CheckPocketQuest(pmcdata);
                var isUnheard = pmcdata.Info.GameVersion == "unheard_edition";
                //Attempt to revive pockets to default value.
                int pocketCount = 0;
                foreach (var pocket in allPockets)
                {
                    if (cf.CSM.EnableCSM && cf.CSM.CustomPocket)
                    {
                        pocket.Template = CUSTOM_POCKETS;
                    }
                    else
                    {
                        pocket.Template = (hasCompletedPocketUpgradeQuest || isUnheard) ? UNHEARD_POCKETS : DEFAULT_POCKETS;
                    }
                    pocketCount++;
                }
                if (pocketCount == 0)//If there is no pocket in profile - create a new one from scratch
                {
                    Item? newpocket = new()
                    {
                        Id = new MongoId(),
                        ParentId = pmcdata.Inventory.Equipment,
                        Template = DEFAULT_POCKETS,
                        SlotId = "Pockets"
                    };
                    //PMC Custom Pockets
                    if (cf.CSM.EnableCSM && cf.CSM.CustomPocket)
                    {
                        newpocket.Template = CUSTOM_POCKETS;
                    }
                    pmcdata.Inventory.Items.Add(newpocket);
                }

                if (cf.Player.EnablePlayer)
                {
                    if (cf.Player.EnableHealth)
                    {
                        Dictionary<string, BodyPartHealth>? health = pmcdata.Health.BodyParts;
                        LocalRaidOverrider.HealthEdit(health, cf.Player.Health, "Maximum");
                    }

                    if (cf.Player.EnableStats)
                    {
                        pmcdata.Health.Energy.Maximum = cf.Player.PMCStats.MaxEnergy;
                        pmcdata.Health.Hydration.Maximum = cf.Player.PMCStats.MaxHydration;
                    }
                }

                if (cf.Scav.EnableScav)
                {
                    if (cf.Scav.EnableStats)
                    {
                        scavdata.Health.Energy.Maximum = cf.Scav.ScavStats.MaxEnergy;
                        scavdata.Health.Hydration.Maximum = cf.Scav.ScavStats.MaxHydration;
                    }

                    if (cf.Scav.EnableScavHealth)
                    {
                        Dictionary<string, BodyPartHealth>? health = scavdata.Health.BodyParts;
                        LocalRaidOverrider.HealthEdit(health, cf.Scav.Health, "Current"); //Since scav should be at full health at every generation.
                        LocalRaidOverrider.HealthEdit(health, cf.Scav.Health, "Maximum");
                    }

                    if (cf.Scav.ScavCustomPockets)
                    {
                        var scavpockets = scavdata.Inventory.Items.Where(item => item.SlotId == "Pockets");
                        foreach (var pocket in scavpockets)
                        {
                            pocket.Template = SCAVCUSTOM_POCKETS;
                        }
                    }
                }
            }
            catch(FileNotFoundException) { }
            catch (Exception ex)
            {
                _logger.Warning("[SVM] Player Health,  new profile detected: Restart game client to apply Pockets/Health/Stats changes.");
            }
            return _databaseService.GetTemplates().Prestige;
        }
        public bool CheckPocketQuest(PmcData pmcdata)
        {
            return pmcdata.Quests.Any(quest => quest.QId == QuestTpl.OLD_PATTERNS && quest.Status == QuestStatusEnum.Success);
        }
    }
}
