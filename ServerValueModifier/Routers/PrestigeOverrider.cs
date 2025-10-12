using Greed.Models;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Controllers;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services;

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
        public const string SCAVCUSTOM_POCKETS = "a8edfb0bce53d103d3f62b9b";
        public override Prestige GetPrestige(MongoId sessionId)
        {
            try
            {
                MainClass.MainConfig cf = new SVMConfig(modhelper).CallConfig();
                PmcData? pmcdata = _profileHelper.GetPmcProfile(sessionId);
                PmcData? scavdata = _profileHelper.GetScavProfile(sessionId);
                //Attempt to revive pockets to default value.
                int pocketCount = 0;
                foreach (var item in pmcdata.Inventory.Items)
                {
                    if (item.SlotId == "Pockets" && pmcdata.Info.GameVersion == "unheard_edition")
                    {
                        item.Template = UNHEARD_POCKETS; //Unheard pockets
                        pocketCount++;
                    }
                    else if (item.SlotId == "Pockets")
                    {
                        item.Template = DEFAULT_POCKETS;
                        pocketCount++;
                    }
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
                    pmcdata.Inventory.Items.Add(newpocket);
                }

                //PMC Custom Pockets
                if (cf.CSM.EnableCSM && cf.CSM.CustomPocket)
                {
                    pocketCount = 0;
                    foreach (var item in pmcdata.Inventory.Items)
                    {
                        if (item.SlotId == "Pockets") //Custom Pocket
                        {
                            item.Template = CUSTOM_POCKETS; //Custom Pocket Template ID, TODO: Make it configurable.
                            pocketCount++;
                        }
                    }

                    if (pocketCount == 0)
                    {
                        Item? newpocket = new()
                        {
                            Id = new MongoId(),
                            ParentId = pmcdata.Inventory.Equipment,
                            Template = CUSTOM_POCKETS,
                            SlotId = "Pockets"
                        };
                        pmcdata.Inventory.Items.Add(newpocket); //Attempt to add pockets if they were removed.
                    }
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
                        foreach (var item in scavdata.Inventory.Items)
                        {
                            if (item.SlotId == "Pockets")
                            {
                                item.Template = SCAVCUSTOM_POCKETS;
                            }
                        }
                    }
                }

                return _databaseService.GetTemplates().Prestige;
            }
            catch (Exception ex)
            {
                _logger.Error("New Profile Detected: Restart SPT to apply Pockets/Health/Stats changes.");
                return _databaseService.GetTemplates().Prestige;
            }
        }
    }
}
