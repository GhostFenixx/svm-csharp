using Greed.Models;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Eft.Hideout;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Spt.Config;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services;
using SPTarkov.Server.Core.Utils.Cloners;

namespace ServerValueModifier.Sections
{
    internal class Hideout(ISptLogger<SVM> logger, ConfigServer configServer, DatabaseService databaseService, ICloner _cloner, MainClass.MainConfig svmconfig)
    {
        public void HideoutSection()
        {
            HideoutConfig hideoutConfig = configServer.GetConfig<HideoutConfig>();
            HideoutSettingsBase hideoutDB = databaseService.GetHideout().Settings;
            Globals globals = databaseService.GetGlobals();
            SPTarkov.Server.Core.Models.Spt.Hideout.Hideout hideout = databaseService.GetHideout();
            hideoutDB.GeneratorFuelFlowRate *= svmconfig.Hideout.FuelConsumptionRate;
            hideoutDB.GeneratorSpeedWithoutFuel *= svmconfig.Hideout.NoFuelMult;
            hideoutDB.AirFilterUnitFlowRate *= svmconfig.Hideout.AirFilterRate;
            hideoutDB.GpuBoostRate *= svmconfig.Hideout.GPUBoostRate;
            
            hideoutConfig.CultistCircle.MaxRewardItemCount = svmconfig.Hideout.CultistMaxRewards;
            hideoutConfig.CultistCircle.HideoutTaskRewardTimeSeconds = (int)(hideoutConfig.CultistCircle.HideoutTaskRewardTimeSeconds * svmconfig.Hideout.CultistTime);
            hideoutConfig.CultistCircle.CraftTimeThresholds.ForEach(c => { c.CraftTimeSeconds = (int)(c.CraftTimeSeconds * svmconfig.Hideout.CultistTime); });
            hideoutConfig.CultistCircle.DirectRewards.ForEach(c => { c.CraftTimeSeconds = (int)(c.CraftTimeSeconds * svmconfig.Hideout.CultistTime); });
            var itemsdb = databaseService.GetItems();
            //Stash size section with all 5 editions.
            if (svmconfig.Hideout.EnableStash)
            {
                MongoId[] stashesID = [ItemTpl.STASH_STANDARD_STASH_10X30, ItemTpl.STASH_LEFT_BEHIND_STASH_10X40, ItemTpl.STASH_PREPARE_FOR_ESCAPE_STASH_10X50, ItemTpl.STASH_EDGE_OF_DARKNESS_STASH_10X68, ItemTpl.STASH_THE_UNHEARD_EDITION_STASH_10X72];
                int[] stashesValue = [svmconfig.Hideout.Stash.StashLvl1, svmconfig.Hideout.Stash.StashLvl2, svmconfig.Hideout.Stash.StashLvl3, svmconfig.Hideout.Stash.StashLvl4, svmconfig.Hideout.Stash.StashTUE];
                int i = 0; //Simple reitration, maybe foreach wasn't smartest idea.TODO
                foreach (MongoId stash in stashesID)
                {
                    List<Grid> grids = itemsdb[stash].Properties.Grids.ToList();
                    grids[0].Properties.CellsV = stashesValue[i];
                    itemsdb[stash].Properties.Grids = grids;
                    i++;
                }
            }
            //if (svmconfig.Hideout.EnableStash)
            //{
            //    stashes["566abbc34bdc2d92178b4576"].Properties.Grids[0].Properties.CellsV = svmconfig.Hideout.Stash.StashLvl1;
            //    stashes["5811ce572459770cba1a34ea"].Properties.Grids[0].Properties.CellsV = svmconfig.Hideout.Stash.StashLvl2;
            //    stashes["5811ce662459770f6f490f32"].Properties.Grids[0].Properties.CellsV = svmconfig.Hideout.Stash.StashLvl3;
            //    stashes["5811ce772459770e9e5f9532"].Properties.Grids[0].Properties.CellsV = svmconfig.Hideout.Stash.StashLvl4;
            //    stashes["6602bcf19cc643f44a04274b"].Properties.Grids[0].Properties.CellsV = svmconfig.Hideout.Stash.StashTUE;
            //}
            //for (int i = 0; i < databaseService.GetHideout().Areas.Count; i++)
            //{
            //    for (int j = 0; j < databaseService.GetHideout().Areas[i].Stages.Count; j++)
            //        {
            //        databaseService.GetHideout().Areas[i].Stages[j.ToString()].ConstructionTime = 1;
            //    }
            //}
            //Hideout Construction time multiplier
            foreach (HideoutArea area in hideout.Areas)
            {
                foreach (Stage stage in area.Stages.Values)
                {
                    if (stage.ConstructionTime > 0)
                    {
                        stage.ConstructionTime = stage.ConstructionTime * svmconfig.Hideout.HideoutConstMult;
                        if (stage.ConstructionTime < 1)
                        {
                            stage.ConstructionTime = 2;
                        }
                    }
                }
            }
            //Production time multipliers including water and bitcoin generation
            foreach (HideoutProduction production in hideout.Production.Recipes)
            {
                if (production.Id == "5d5589c1f934db045e6c5492")
                {
                    production.ProductionTime = svmconfig.Hideout.WaterFilterTime * 60;
                    production.Requirements[1].Resource = svmconfig.Hideout.WaterFilterRate;
                }
                if (production.Id == "5d5c205bd582a50d042a3c0e")
                {
                    production.ProductionLimitCount = svmconfig.Hideout.MaxBitcoins;
                    production.ProductionTime = svmconfig.Hideout.BitcoinTime * 60;
                }
                if (production.Continuous == false && production.ProductionTime >= 10)
                {
                    production.ProductionTime *= svmconfig.Hideout.HideoutProdMult;
                    if (production.ProductionTime < 1)
                    {
                        production.ProductionTime = 2;
                    }
                }
            }
            //Hideout's scav case price modifiers for cash offers.
            foreach (ScavRecipe production in hideout.Production.ScavRecipes)
            {
                //Hideout's Scav case 'crafts' time multiplier
                production.ProductionTime *= svmconfig.Hideout.ScavCaseTime;
                if (production.ProductionTime < 1)
                {
                    production.ProductionTime = 2;
                }
                //Hideout's Scav case 'crafts' time multiplier
                if (production.Requirements[0].TemplateId == "5449016a4bdc2d6f028b456f" || production.Requirements[0].TemplateId == "5449016a4bdc2d6f028b456f" || production.Requirements[0].TemplateId == "5449016a4bdc2d6f028b456f")
                {
                    production.Requirements[0].Count = (int)(production.Requirements[0].Count * svmconfig.Hideout.ScavCasePrice);
                }
            }
            ///Removing different kind of requirements
            if (svmconfig.Hideout.RemoveConstructionsRequirements || svmconfig.Hideout.RemoveSkillRequirements || svmconfig.Hideout.RemoveTraderLevelRequirements || svmconfig.Hideout.RemoveConstructionsFIRRequirements)
            {
                Stage Rewriter = new();
                foreach (HideoutArea area in hideout.Areas)
                {
                    foreach (Stage stage in area.Stages.Values)
                    {
                        Rewriter = _cloner.Clone(stage);
                        Rewriter.Requirements.Clear();
                        foreach (StageRequirement requirements in stage.Requirements)
                        {
                            if (requirements is not null)
                            {
                                if (svmconfig.Hideout.RemoveConstructionsFIRRequirements && requirements.IsSpawnedInSession != null)
                                {
                                    requirements.IsSpawnedInSession = false;
                                }
                                if (!svmconfig.Hideout.RemoveConstructionsRequirements && !requirements.TemplateId.IsEmpty)
                                {
                                    Rewriter.Requirements.Add(requirements);
                                }
                                else if (!svmconfig.Hideout.RemoveSkillRequirements && requirements.SkillName != null)
                                {
                                    Rewriter.Requirements.Add(requirements);
                                }
                                else if (!svmconfig.Hideout.RemoveTraderLevelRequirements && !requirements.TraderId.IsEmpty)
                                {
                                    Rewriter.Requirements.Add(requirements);
                                }
                                else if (requirements.AreaType != null)
                                {
                                    Rewriter.Requirements.Add(requirements);
                                }
                            }
                        }
                        stage.Requirements = Rewriter.Requirements;
                    }
                }
            }
            //Health regen section - Shame on me, i couldn't find a way to go 'foreach' here.
            BodyHealth healthValues = globals.Configuration.Health.Effects.Regeneration.BodyHealth;
            healthValues.Stomach.Value *= svmconfig.Hideout.Regeneration.HealthRegen;
            healthValues.Head.Value *= svmconfig.Hideout.Regeneration.HealthRegen;
            healthValues.Chest.Value *= svmconfig.Hideout.Regeneration.HealthRegen;
            healthValues.LeftArm.Value *= svmconfig.Hideout.Regeneration.HealthRegen;
            healthValues.RightArm.Value *= svmconfig.Hideout.Regeneration.HealthRegen;
            healthValues.LeftLeg.Value *= svmconfig.Hideout.Regeneration.HealthRegen;
            healthValues.RightLeg.Value *= svmconfig.Hideout.Regeneration.HealthRegen;
            globals.Configuration.Health.Effects.Regeneration.Energy = svmconfig.Hideout.Regeneration.EnergyRegen;
            globals.Configuration.Health.Effects.Regeneration.Hydration = svmconfig.Hideout.Regeneration.HydrationRegen;

            //Removing passive bonuses for Health/Energy/Hydration regeneration.
            if (svmconfig.Hideout.RemoveConstructionsRequirements || svmconfig.Hideout.RemoveSkillRequirements || svmconfig.Hideout.RemoveTraderLevelRequirements || svmconfig.Hideout.RemoveConstructionsFIRRequirements)
            {   
                foreach (HideoutArea area in hideout.Areas)
                {
                    foreach (Stage stage in area.Stages.Values)
                    {
                        foreach (Bonus bonus in stage.Bonuses)
                        {
                            if (svmconfig.Hideout.Regeneration.HideoutHydration && bonus.Type == BonusType.HydrationRegeneration)
                            {
                                bonus.Value = 0;
                            }
                            if (svmconfig.Hideout.Regeneration.HideoutEnergy && bonus.Type == BonusType.EnergyRegeneration)
                            {
                                bonus.Value = 0;
                            }
                            if (svmconfig.Hideout.Regeneration.HideoutHealth && bonus.Type == BonusType.HealthRegeneration)
                            {
                                bonus.Value = 0;
                            }
                        }
                    }
                }
            }
        }
    }
}
