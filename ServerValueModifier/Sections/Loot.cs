using Greed.Models;
using Greed.Models.Looting;
using SPTarkov.Common.Models.Logging;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Spt.Config;
using SPTarkov.Server.Core.Models.Spt.Tables;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services;

namespace ServerValueModifier.Sections
{
    internal class Loot(ISptLogger<SVM> logger, AirdropConfig airdropconfig, TemplateTable templateTable, LocationConfig locsloot, LocationTable locsdatabase, MainClass.MainConfig svmconfig)
    {
        public void LootSection()
        {
            
            Locations lootlc = svmconfig.Loot.Locations; // lc = local config, aka greed class
            Airdrops airlc = svmconfig.Loot.Airdrops;
            //Parsing config, mapname, loose loot and container from config and apply them.
            LootValues(locsloot, "labyrinth", lootlc.Labyrinth.Loose, lootlc.Labyrinth.Container);
            LootValues(locsloot, "bigmap", lootlc.Bigmap.Loose, lootlc.Bigmap.Container);
            LootValues(locsloot, "factory4_day", lootlc.FactoryDay.Loose, lootlc.FactoryDay.Container);
            LootValues(locsloot, "factory4_night", lootlc.FactoryNight.Loose, lootlc.FactoryNight.Container);
            LootValues(locsloot, "interchange", lootlc.Interchange.Loose, lootlc.Interchange.Container);
            LootValues(locsloot, "laboratory", lootlc.Laboratory.Loose, lootlc.Laboratory.Container);
            LootValues(locsloot, "rezervbase", lootlc.Reserve.Loose, lootlc.Reserve.Container);
            LootValues(locsloot, "shoreline", lootlc.Shoreline.Loose, lootlc.Shoreline.Container);
            LootValues(locsloot, "woods", lootlc.Woods.Loose, lootlc.Woods.Container);
            LootValues(locsloot, "lighthouse", lootlc.Lighthouse.Loose, lootlc.Lighthouse.Container);
            LootValues(locsloot, "tarkovstreets", lootlc.Streets.Loose, lootlc.Streets.Container);
            LootValues(locsloot, "sandbox", lootlc.Sandbox.Loose, lootlc.Sandbox.Container);
            LootValues(locsloot, "sandbox_high", lootlc.SandboxHard.Loose, lootlc.SandboxHard.Container);
            //Randomisation of boxes, IIRC if disabled all containers possible would spawn
            locsloot.ContainerRandomisationSettings.Enabled = !svmconfig.Loot.Locations.AllContainers;
            //Certain data we need is stored in location's DB instead of config, thererefore we parse location data and greed config.
            AirdropsValues(locsdatabase.Bigmap.Base.AirdropParameters[0], airlc.Bigmap_air, airlc.AirtimeMin, airlc.AirtimeMax, svmconfig.Raids.RaidTime, svmconfig.Raids.EnableRaids);
            AirdropsValues(locsdatabase.Shoreline.Base.AirdropParameters[0], airlc.Shoreline_air, airlc.AirtimeMin, airlc.AirtimeMax, svmconfig.Raids.RaidTime, svmconfig.Raids.EnableRaids);
            AirdropsValues(locsdatabase.RezervBase.Base.AirdropParameters[0], airlc.Reserve_air, airlc.AirtimeMin, airlc.AirtimeMax, svmconfig.Raids.RaidTime, svmconfig.Raids.EnableRaids);
            AirdropsValues(locsdatabase.Lighthouse.Base.AirdropParameters[0], airlc.Lighthouse_air, airlc.AirtimeMin, airlc.AirtimeMax, svmconfig.Raids.RaidTime, svmconfig.Raids.EnableRaids);
            AirdropsValues(locsdatabase.Interchange.Base.AirdropParameters[0], airlc.Interchange_air, airlc.AirtimeMin, airlc.AirtimeMax, svmconfig.Raids.RaidTime, svmconfig.Raids.EnableRaids);
            AirdropsValues(locsdatabase.TarkovStreets.Base.AirdropParameters[0], airlc.Streets_air, airlc.AirtimeMin, airlc.AirtimeMax, svmconfig.Raids.RaidTime, svmconfig.Raids.EnableRaids);
            AirdropsValues(locsdatabase.Sandbox.Base.AirdropParameters[0], airlc.Sandbox_air, airlc.AirtimeMin, airlc.AirtimeMax, svmconfig.Raids.RaidTime, svmconfig.Raids.EnableRaids);
            AirdropsValues(locsdatabase.SandboxHigh.Base.AirdropParameters[0], airlc.Sandbox_air, airlc.AirtimeMin, airlc.AirtimeMax, svmconfig.Raids.RaidTime, svmconfig.Raids.EnableRaids);
            AirdropsValues(locsdatabase.Woods.Base.AirdropParameters[0], airlc.Woods_air, airlc.AirtimeMin, airlc.AirtimeMax, svmconfig.Raids.RaidTime, svmconfig.Raids.EnableRaids);
            //This is from the config, we just parse string with container type and it's values from greed config.
            AirdropContents("mixed", airlc.Mixed);
            AirdropContents("weaponArmor", airlc.Weapon);
            AirdropContents("barter", airlc.Barter);
            AirdropContents("foodMedical", airlc.Medical);
        }
        public void AirdropsValues(AirdropParameter airdrop, int chance, int min, int max, int time, bool enabled)
        {
            airdrop.PlaneAirdropChance = (double)chance / 100;
            airdrop.PlaneAirdropStartMin = min * 60;
            airdrop.PlaneAirdropStartMax = max * 60;
            //airdrop.PlaneAirdropCooldownMax = 9999; // Requested by user - removing useless extra complexity to the system, First is the range when you can get an airdrop,
            //airdrop.PlaneAirdropCooldownMin = 9999; // then there's where another can be spawned which takes effect in Ground Zero and Streets, discarded that.
            //                                        // Not entirely sure how that would work with calling quest/supply drops using flares however.
            if (enabled)
            {
                airdrop.PlaneAirdropEnd = time * 60;
            }
        }
        public void LootValues(LocationConfig locs, string map, double loose, double container)
        {
            locs.LooseLootMultiplier[map] = loose;
            locs.StaticLootMultiplier[map] = container;
        }
        public void AirdropContents(string dbtype, AirdropContents type)
        {
            airdropconfig.Loot[dbtype].ItemCount.Min = type.BarterMin;
            airdropconfig.Loot[dbtype].ItemCount.Max = type.BarterMax;
            airdropconfig.Loot[dbtype].WeaponPresetCount.Min = type.PresetMin;
            airdropconfig.Loot[dbtype].WeaponPresetCount.Max = type.PresetMax;
            airdropconfig.Loot[dbtype].ArmorPresetCount.Min = type.ArmorMin;
            airdropconfig.Loot[dbtype].ArmorPresetCount.Max = type.ArmorMax;
            airdropconfig.Loot[dbtype].WeaponCrateCount.Min = type.CratesMin;
            airdropconfig.Loot[dbtype].WeaponCrateCount.Max = type.CratesMax;
        }
    }
}
