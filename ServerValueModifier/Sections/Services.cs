using Greed.Models;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Spt.Config;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services;
using SPTarkov.Server.Core.Utils.Cloners;

namespace ServerValueModifier.Sections
{
    internal class Services(ISptLogger<SVM> logger, ConfigServer configServer, DatabaseService databaseService, ICloner _cloner, LocaleService localeService, MainClass.MainConfig svmcfg)
    {
        public void ServicesSection()
        {
            var repair = configServer.GetConfig<RepairConfig>();
            var insurance = configServer.GetConfig<InsuranceConfig>();
            var traders = databaseService.GetTraders();
            var suits = databaseService.GetCustomization();
            Globals globals = databaseService.GetGlobals();
            //Insurance section
            if (svmcfg.Services.EnableInsurance)
            {

                insurance.ReturnChancePercent["54cb50c76803fa8b248b4571"] = svmcfg.Services.ReturnChancePrapor;
                insurance.ReturnChancePercent["54cb57776803fa99248b456e"] = svmcfg.Services.ReturnChanceTherapist;
                TraderInsurance? praporinsurance = traders["54cb50c76803fa8b248b4571"].Base.Insurance;
                TraderInsurance? therapistinsurance = traders["54cb57776803fa99248b456e"].Base.Insurance;
                praporinsurance.MaxStorageTime = svmcfg.Services.PraporStorageTime;
                praporinsurance.MinReturnHour = svmcfg.Services.Prapor_Min;
                praporinsurance.MaxReturnHour = svmcfg.Services.Prapor_Max;
                therapistinsurance.MaxStorageTime = svmcfg.Services.TherapistStorageTime;
                therapistinsurance.MinReturnHour = svmcfg.Services.Therapist_Min;
                therapistinsurance.MaxReturnHour = svmcfg.Services.Therapist_Max;

                insurance.ChanceNoAttachmentsTakenPercent = svmcfg.Services.InsuranceAttachmentChance;
                insurance.RunIntervalSeconds = svmcfg.Services.InsuranceInterval;
                double[] therapistlevels = [svmcfg.Services.InsuranceMultTherapistLvl1, svmcfg.Services.InsuranceMultTherapistLvl2, svmcfg.Services.InsuranceMultTherapistLvl3, svmcfg.Services.InsuranceMultTherapistLvl4];
                double[] praporlevels = [svmcfg.Services.InsuranceMultPraporLvl1, svmcfg.Services.InsuranceMultPraporLvl2, svmcfg.Services.InsuranceMultPraporLvl3, svmcfg.Services.InsuranceMultPraporLvl4];
                if (svmcfg.Services.EnableTimeOverride)
                {
                    insurance.ReturnTimeOverrideSeconds = svmcfg.Services.InsuranceTimeOverride;
                }
                int i = 0;
                foreach (var level in traders["54cb57776803fa99248b456e"].Base.LoyaltyLevels)
                {
                    level.InsurancePriceCoefficient = therapistlevels[i];
                    i++;

                }
                i = 0;
                foreach (var level in traders["54cb50c76803fa8b248b4571"].Base.LoyaltyLevels)
                {
                    level.InsurancePriceCoefficient = praporlevels[i];
                    i++;
                }
            }

            //Clothing section
            if (svmcfg.Services.ClothesAnySide)
            {
                foreach (var suit in suits)
                {
                    if (suit.Value.Parent == "5cd944ca1388ce03a44dc2a4" || suit.Value.Parent == "5cd944d01388ce000a659df9") //suit.Value.Parent == "5cc0868e14c02e000c6bea68"
                    {
                        suit.Value.Properties.AvailableAsDefault = true;
                        suit.Value.Properties.Game = ["eft"];
                        suit.Value.Properties.Side = ["Bear", "Usec", "Savage"];
                        suit.Value.Properties.ProfileVersions = [];
                    }
                }
            }

            if (svmcfg.Services.ClothesFree || svmcfg.Services.ClothesLevelUnlock)
            {
                foreach (var suit in traders["5ac3b934156ae10c4430e83c"].Suits)
                {
                    if (svmcfg.Services.ClothesLevelUnlock)
                    {
                        suit.IsHiddenInPVE = false;
                        suit.Requirements.LoyaltyLevel = 1;
                        suit.Requirements.ProfileLevel = 1;
                        suit.Requirements.Standing = 0;
                        suit.Requirements.PrestigeLevel = 0;
                        suit.Requirements.QuestRequirements = [];
                        suit.Requirements.AchievementRequirements = [];
                    }
                    if (svmcfg.Services.ClothesFree)
                    {
                        suit.Requirements.ItemRequirements = [];
                    }
                }
            }
            //Health Markup Section
            if (svmcfg.Services.EnableHealMarkup)
            {
                globals.Configuration.Health.HealPrice.TrialRaids = svmcfg.Services.FreeHealRaids;
                globals.Configuration.Health.HealPrice.TrialLevels = svmcfg.Services.FreeHealLvl;
                double[] therapistheallevels = [svmcfg.Services.TherapistLvl1, svmcfg.Services.TherapistLvl2, svmcfg.Services.TherapistLvl3, svmcfg.Services.TherapistLvl4];
                int i = 0;
                foreach (var level in traders["54cb57776803fa99248b456e"].Base.LoyaltyLevels)
                {
                    level.HealPriceCoefficient = 100 * therapistheallevels[i];
                    i++;
                }
            }
            //Repair Section
            if (svmcfg.Services.EnableRepair)
            {
                repair.ArmorKitSkillPointGainPerRepairPointMultiplier = svmcfg.Services.RepairBox.ArmorSkillMult;
                repair.WeaponTreatment.PointGainMultiplier = svmcfg.Services.RepairBox.WeaponMaintenanceSkillMult;
                repair.RepairKitIntellectGainMultiplier.Weapon = svmcfg.Services.RepairBox.IntellectSkillMultWeaponKit;
                repair.RepairKitIntellectGainMultiplier.Armor = svmcfg.Services.RepairBox.IntellectSkillMultArmorKit;
                repair.MaxIntellectGainPerRepair.Kit = svmcfg.Services.RepairBox.IntellectSkillLimitKit;
                repair.MaxIntellectGainPerRepair.Trader = svmcfg.Services.RepairBox.IntellectSkillLimitTraders;
                repair.ApplyRandomizeDurabilityLoss = !svmcfg.Services.RepairBox.NoRandomRepair;
                foreach (var trader in traders)
                {
                    if (trader.Key == "58330581ace78e27b8b10cee" || trader.Key == "54cb50c76803fa8b248b4571" || trader.Key == "5a7c2eca46aef81a7ca2145d") //5a7c2eca46aef81a7ca2145d
                    {
                        int i = 0;
                        foreach (var level in traders[trader.Key].Base.LoyaltyLevels)
                        {
                            level.RepairPriceCoefficient *= svmcfg.Services.RepairBox.RepairMult;
                            i++;
                        }
                    }
                }
                if (svmcfg.Services.RepairBox.OpArmorRepair)
                {
                    foreach (var armor in globals.Configuration.ArmorMaterials)
                    {
                        armor.Value.MaxRepairDegradation = 0;
                        armor.Value.MinRepairDegradation = 0;
                        armor.Value.MaxRepairKitDegradation = 0;
                        armor.Value.MinRepairKitDegradation = 0;
                    }
                }
                if (svmcfg.Services.RepairBox.OpGunRepair)
                {
                    Dictionary<MongoId, TemplateItem> items = databaseService.GetItems();
                    foreach (TemplateItem basetemplate in items.Values)
                    {
                        if (basetemplate.Properties.MaxRepairDegradation is not null && basetemplate.Properties.MaxRepairKitDegradation is not null)
                        {
                            basetemplate.Properties.MinRepairDegradation = 0;
                            basetemplate.Properties.MaxRepairDegradation = 0;
                            basetemplate.Properties.MinRepairKitDegradation = 0;
                            basetemplate.Properties.MaxRepairKitDegradation = 0;
                        }
                    }
                }
            }
        }
    }
}