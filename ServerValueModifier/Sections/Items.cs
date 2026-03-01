using Greed.Models;
using SPTarkov.Server.Core.Constants;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services;

namespace ServerValueModifier.Sections
{
    internal class Items(ISptLogger<SVM> logger, ConfigServer configServer, DatabaseService databaseService, MainClass.MainConfig svmconfig)
    {
        //TODO: Follow the example - either follow SPT's ENUM where available or create such variables for own use.
        //private  MongoId KEYCARD_PARENT = "5c164d2286f774194c5e69fa"; can be only string if used with const TODO
        private readonly MongoId[] WeaponTypesID = ["5447b5cf4bdc2d65278b4567", "5447b6254bdc2dc3278b4568", "5447b5f14bdc2d61278b4567", "5447bed64bdc2d97278b4568", "5447b6094bdc2dc3278b4567", "5447b5e04bdc2d62278b4567", "5447b6194bdc2d67278b4567"];
        private readonly MongoId[] KeyTypes = ["5c99f98d86f7745c314214b3", "5c164d2286f774194c5e69fa", "543be5e94bdc2df1348b4568"];
        private readonly string[] Pistol = ["9x19", "9x18pm", "9x21", "762x25tt", "46x30", "57x28", "1143x23", "127x33", "9x33r", "10MM", "40SW", "357SIG", "9MM", "45ACP", "50AE", "380AUTO"];
        private readonly string[] ARifle = ["762x39", "545x39", "556x45", "9x39", "366", "762x35", "300blk", "ATL15", "GRENDEL", "50WLF", "KURZ"];
        private readonly string[] Shotgun = ["12x70", "20x70", "23x75"];
        private readonly string[] MRifle = ["762x51", "68x51", "762х54R", "762x54", "762x54r", "277"];
        private readonly string[] LCRifle = ["127x55", "127x99", "86x70", "BMG"];
        private readonly MongoId[] OddKeys = ["5448ba0b4bdc2d02308b456c", "63a399193901f439517cafb6", "63a39fc0af870e651d58e6ae", "63a39fdf1e21260da44a0256", "6582dbf0b8d7830efc45016f", "664d3db6db5dea2bad286955", "664d3dd590294949fe2d81b7", "664d3ddfdda2e85aca370d75", "664d3de85f2355673b09aed5", "664d4b0103ef2c61246afb56", "6761a6ccd9bbb27ad703c48a", "6761a6f90575f25e020816a4"];
        private readonly MongoId[] MarkedKeys = ["5780cf7f2459777de4559322", "5d80c60f86f77440373c4ece", "5d80c62a86f7744036212b3f", "5ede7a8229445733cb4c18e2", "63a3a93f8a56922e82001f5d", "64ccc25f95763a1ae376e447", "62987dfc402c7f69bf010923"];
        private readonly Globals globals = databaseService.GetGlobals();
        Dictionary<MongoId, TemplateItem> items = databaseService.GetItems();
        HandbookBase handbook = databaseService.GetHandbook();
        public void ItemsSection()
        {
            //Speed to load/unload magazines in raid.
            globals.Configuration.BaseUnloadTime *= svmconfig.Items.AmmoLoadSpeed;
            globals.Configuration.BaseLoadTime *= svmconfig.Items.AmmoLoadSpeed;
            globals.Configuration.ItemsCommonSettings.MaxBackpackInserting = svmconfig.Items.BackpackStacking;
            //Remove raid restrictions.
            if (svmconfig.Items.RemoveRaidRestr)
            {
                globals.Configuration.RestrictionsInRaid = [];
            }
            foreach (var ele in handbook.Items)
            {
                if (ele.Id != "5b5f78b786f77447ed5636af" && ele.Price != null)
                {
                    ele.Price = ele.Price * svmconfig.Items.ItemPriceMult;
                }
            }
            
            //Main cycle that goes once in items.json
            foreach (TemplateItem basetemplate in items.Values)
            {
                if((svmconfig.Items.RemoveRaidRestr && (basetemplate.Parent == "57864a3d24597754843f8721"  || basetemplate.Parent == ""))) //Shitty attempt to fix BSG insurance filters.
                //Add Signal Pistol to PMC's Standard/Unheard/CSM's Custom special slots
                if (basetemplate.Id == "a8edfb0bce53d103d3f62b9b" || basetemplate.Id == ItemTpl.POCKETS_1X4_SPECIAL || basetemplate.Id == ItemTpl.POCKETS_1X4_TUE)
                {
                    //if (svmconfig.Items.AddSignalPistolToSpec) // Not actual.
                    //{
                    //  // basetemplate.Properties.Cartridges.First().Properties.MaxStackCount;
                    //    var slotfilters = basetemplate.Properties.Slots.ToList();
                    //    slotfilters.ForEach(slot => {
                    //        var filter = slot.Properties.Filters.ToList();
                    //        filter[0].Filter.Add(new MongoId ("620109578d82e67e7911abf2"));
                    //        slot.Properties.Filters = filter;
                    //    });
                    //    basetemplate.Properties.Slots = slotfilters;
                    //}
                    // Add Surv and CMS to PMC's Standard/Unheard/CSM's Custom special slots //5448bf274bdc2dfc2f8b456a
                    if (svmconfig.Items.SurvCMSToSpec)
                    {
                        var slotfilters = basetemplate.Properties.Slots.ToList();
                        slotfilters.ForEach(slot => {
                            var filter = slot.Properties.Filters.ToList();
                            filter[0].Filter.Add(new MongoId(ItemTpl.MEDICAL_SURV12_FIELD_SURGICAL_KIT));
                            slot.Properties.Filters = filter;
                        });
                        slotfilters.ForEach(slot => {
                            var filter = slot.Properties.Filters.ToList();
                            filter[0].Filter.Add(new MongoId(ItemTpl.MEDICAL_CMS_SURGICAL_KIT));
                            slot.Properties.Filters = filter;
                        });
                        basetemplate.Properties.Slots = slotfilters;
                    }
                }
                // Restrict Surv and CMS from all the Special Containers.
                if (svmconfig.Items.SurvCMSSecConBlock && basetemplate.Parent == "5448bf274bdc2dfc2f8b456a" && basetemplate.Id != ItemTpl.SECURE_CONTAINER_BOSS)
                {
                    var gridsfilters = basetemplate.Properties.Grids.ToList();
                    gridsfilters.ForEach(grid => {
                        var filter = grid.Properties.Filters.ToList();
                        filter[0].ExcludedFilter.Add(new MongoId(ItemTpl.MEDICAL_SURV12_FIELD_SURGICAL_KIT));
                        grid.Properties.Filters = filter;
                    });
                    gridsfilters.ForEach(grid => {
                        var filter = grid.Properties.Filters.ToList();
                        filter[0].ExcludedFilter.Add(new MongoId(ItemTpl.MEDICAL_CMS_SURGICAL_KIT));
                        grid.Properties.Filters = filter;
                    });
                    basetemplate.Properties.Grids = gridsfilters;
                }
                //Time multiplier required to examine an item
                if (basetemplate.Type == "Item" && basetemplate.Properties.ExamineTime is not null)
                {
                    basetemplate.Properties.ExamineTime = svmconfig.Items.ExamineTime;
                }
                //Multiplier of ammunition fragmentation chance
                if (basetemplate.Properties.FragmentationChance is not null)
                {
                    basetemplate.Properties.FragmentationChance *= svmconfig.Items.FragmentMult;
                }
                //Multiplier of a heat factor
                if (basetemplate.Properties.HeatFactor is not null)
                {
                    basetemplate.Properties.HeatFactor *= svmconfig.Items.HeatFactor;
                }
                if (basetemplate.Properties.HeatFactorByShot is not null)
                {
                    basetemplate.Properties.HeatFactorByShot *= svmconfig.Items.HeatFactor;
                }
                //Remove in-raid restrictions
                if (basetemplate.Type == "Item" && basetemplate.Properties.DiscardLimit is not null && svmconfig.Items.RaidDrop)
                {
                    basetemplate.Properties.DiscardLimit = -1;
                }
                //Disable weapon overheat (IIRC heat effect stays on, just no misfires)
                if (basetemplate.Properties.AllowOverheat is not null && svmconfig.Items.WeaponHeatOff)
                {
                    basetemplate.Properties.AllowOverheat = false;
                }
                //Base malfunctions chance in each weapon class
               // if (basetemplate.Parent.Equals(WeaponTypesID) && basetemplate.Properties.BaseMalfunctionChance is not null)
               if( SimpleFilter(WeaponTypesID, basetemplate.Parent))
                {
                    basetemplate.Properties.BaseMalfunctionChance *= svmconfig.Items.MalfunctChanceMult;
                }
                //same value.
                if (basetemplate.Parent == "5448bc234bdc2d3c308b4569" && basetemplate.Properties.MalfunctionChance is not null)
                {
                    basetemplate.Properties.MalfunctionChance *= svmconfig.Items.MalfunctChanceMult;
                }
                //Multiplier of a chance to misfire
                if (basetemplate.Parent == "5448bc234bdc2d3c308b4569" && basetemplate.Properties.MisfireChance is not null)
                {
                    basetemplate.Properties.MalfunctionChance *= svmconfig.Items.MisfireChance;
                }
                //Examine all items
                if (basetemplate.Properties.ExaminedByDefault is not null && svmconfig.Items.AllExaminedItems && svmconfig.Items.ExamineKeys)
                {
                    basetemplate.Properties.ExaminedByDefault = true;
                }
                //If ExamineKeys was off - examine everything except key types, mechanical and keycards.
                else if (basetemplate.Properties.ExaminedByDefault is not null && SimpleFilter(KeyTypes, basetemplate.Parent) && svmconfig.Items.ExamineKeys) //basetemplate.Parent.Equals(KeyTypes)
                {
                    basetemplate.Properties.ExaminedByDefault = true;
                }
                //Multiplier for weight of every single item in game (avoid pockets and inventory objects, i don't remember why, no weight parameter maybe?)
                if (basetemplate.Type != "Node" && basetemplate.Type is not null && basetemplate.Parent != "557596e64bdc2dc2118b4571" && basetemplate.Parent != "55d720f24bdc2d88028b456d")
                {
                    basetemplate.Properties.Weight *= svmconfig.Items.WeightChanger;
                }
                //Removing Ergonomic/Speed/Movement penalty from equipment
                if (svmconfig.Items.NoGearPenalty)
                {
                    if (basetemplate.Properties.MousePenalty is not null)
                    {
                        basetemplate.Properties.MousePenalty = 0;
                    }
                    if (basetemplate.Properties.WeaponErgonomicPenalty is not null)
                    {
                        basetemplate.Properties.WeaponErgonomicPenalty = 0;
                    }
                    if (basetemplate.Properties.SpeedPenaltyPercent is not null)
                    {
                        basetemplate.Properties.SpeedPenaltyPercent = 0;
                    }
                }
                //Ammo stacks section, allows you to adjust ammo stack sizes per caliber
                if (basetemplate.Parent == "5485a8684bdc2da71d8b4567" && svmconfig.Items.AmmoSwitch)
                {
                    if (SimpleFilter(ARifle, basetemplate.Name))
                    {
                        basetemplate.Properties.StackMaxSize = svmconfig.Items.AmmoStacks.RifleRound;
                    }
                    if (SimpleFilter(MRifle, basetemplate.Name))
                    {
                        basetemplate.Properties.StackMaxSize = svmconfig.Items.AmmoStacks.MarksmanRound;
                    }
                    if (SimpleFilter(Pistol, basetemplate.Name))
                    {
                        basetemplate.Properties.StackMaxSize = svmconfig.Items.AmmoStacks.PistolRound;
                    }
                    if (SimpleFilter(Shotgun, basetemplate.Name))
                    {
                        basetemplate.Properties.StackMaxSize = svmconfig.Items.AmmoStacks.ShotgunRound;
                    }
                    if (SimpleFilter(LCRifle, basetemplate.Name))
                    {
                        basetemplate.Properties.StackMaxSize = svmconfig.Items.AmmoStacks.LargeCaliberRound;
                    }
                }
                //Change currency stacks, Euro,Dollars,GPCoins,Roubles.
                if (basetemplate.Parent == "543be5dd4bdc2deb348b4569" && basetemplate.Properties.StackMaxSize is not null && svmconfig.Items.EnableCurrency)
                {
                    switch (basetemplate.Id)
                    {
                        case "569668774bdc2da2298b4568": basetemplate.Properties.StackMaxSize = svmconfig.Items.EuroStack; break; //Lacy would complain about this.
                        case "5696686a4bdc2da3298b456a": basetemplate.Properties.StackMaxSize = svmconfig.Items.DollarStack; break;
                        case "5d235b4d86f7742e017bc88a": basetemplate.Properties.StackMaxSize = svmconfig.Items.GPStack; break;
                        default: basetemplate.Properties.StackMaxSize = svmconfig.Items.RubStack; break;
                    }
                }
                //Allow carry Armored vests with armored rigs
                if (svmconfig.Items.EquipRigsWithArmors && basetemplate.Properties.BlocksArmorVest is not null)
                {
                    basetemplate.Properties.BlocksArmorVest = false;
                }
                //Remove Secure Container requirements - now supports Theta other slots, yay.
                if (svmconfig.Items.RemoveSecureContainerFilters && basetemplate.Parent == "5448bf274bdc2dfc2f8b456a" && basetemplate.Id != ItemTpl.SECURE_CONTAINER_BOSS)
                {
                    var gridsfilters = basetemplate.Properties.Grids.ToList();
                    gridsfilters.ForEach(Grid =>
                    {
                        if (Grid.Properties.Filters is not null)
                        {
                            var filters = Grid.Properties.Filters.ToList();
                                filters[0].Filter.Clear();
                                filters[0].Filter.Add(new MongoId("54009119af1c881c07000029"));
                                filters[0].ExcludedFilter = [];
                                Grid.Properties.Filters = filters;
                        }
                    });
                    basetemplate.Properties.Grids = gridsfilters;
                }
                //Remove Backpack Restrictions, now support any multi-slot backpacks, glory to C#
                if (svmconfig.Items.RemoveBackpacksRestrictions && basetemplate.Parent == "5448e53e4bdc2d60728b4567")
                {
                    List<Grid> gridsfilters = basetemplate.Properties.Grids.ToList();
                    gridsfilters.ForEach(Grid =>
                    {
                        try
                        {
                            if (Grid.Properties.Filters is not null)
                            {
                                var filters = Grid.Properties.Filters.ToList();
                                filters[0].Filter.Clear();
                                filters[0].Filter.Add(new MongoId("54009119af1c881c07000029"));
                                filters[0].ExcludedFilter = [];
                                Grid.Properties.Filters = filters;
                            }
                        }
                        catch {
                            logger.Warning("[SVM] Remove backpack restrictions - empty filters detected on ID - " + basetemplate.Id + " - Ignoring");
                        }
                    });
                    basetemplate.Properties.Grids = gridsfilters;
                }
                //Multiplier of loot experience (picking up in raid)
                if (basetemplate.Properties.LootExperience is not null)
                {
                    basetemplate.Properties.LootExperience = Math.Max((int)(basetemplate.Properties.LootExperience * svmconfig.Items.LootExp),1); //I think it's redundant here, but for sanity purposes we don't want it to be 0
                }
                //Multiplier of examination experience
                if (basetemplate.Properties.ExamineExperience is not null)
                {
                    basetemplate.Properties.ExamineExperience = Math.Max((int)(int)(basetemplate.Properties.ExamineExperience * svmconfig.Items.ExamineExp),1);
                }
                //Keys Section
                if (svmconfig.Items.Keys.EnableKeys)
                {
                    //First - check for making them infinite and whether other options were selected
                    if (basetemplate.Parent == "5c99f98d86f7745c314214b3" && basetemplate.Properties.MaximumNumberOfUsage is not null && svmconfig.Items.Keys.InfiniteKeys)
                    {
                        if (basetemplate.Parent == "5c99f98d86f7745c314214b3" && basetemplate.Properties.MaximumNumberOfUsage == 1 && !svmconfig.Items.Keys.AvoidSingleKeys)
                        {
                            basetemplate.Properties.MaximumNumberOfUsage = 0;
                        }
                        if (SimpleFilter(MarkedKeys, basetemplate.Id) && !svmconfig.Items.Keys.AvoidMarkedKeys)
                        {
                            basetemplate.Properties.MaximumNumberOfUsage = 0;
                        }
                        if (SimpleFilter(OddKeys, basetemplate.Id) && !svmconfig.Items.Keys.AvoidOddKeys)
                        {
                            basetemplate.Properties.MaximumNumberOfUsage = 0;
                        }
                        if (!SimpleFilter(MarkedKeys, basetemplate.Id) && (!SimpleFilter(OddKeys, basetemplate.Id)) && basetemplate.Properties.MaximumNumberOfUsage != 1)
                        {
                            basetemplate.Properties.MaximumNumberOfUsage = 0;
                        }
                    }
                    //Same with Keycards
                    if (basetemplate.Parent == "5c164d2286f774194c5e69fa" && basetemplate.Properties.MaximumNumberOfUsage is not null && svmconfig.Items.Keys.InfiniteKeycards)
                    {
                        //If Avoid Residential is true - we ignore this condition
                        if (basetemplate.Id == ItemTpl.KEYCARD_TERRAGROUP_LABS_RESIDENTIAL_UNIT && !svmconfig.Items.Keys.AvoidResidential)
                        {
                            basetemplate.Properties.MaximumNumberOfUsage = 0;
                        }
                        //If Avoid Access is true - we ignore this condition
                        if (basetemplate.Id == ItemTpl.KEYCARD_TERRAGROUP_LABS_ACCESS && !svmconfig.Items.Keys.IgnoreAccessCard)
                        {
                            basetemplate.Properties.MaximumNumberOfUsage = 0;
                        }
                        if (basetemplate.Properties.MaximumNumberOfUsage == 1 && !svmconfig.Items.Keys.AvoidSingleKeyCards)
                        {
                            basetemplate.Properties.MaximumNumberOfUsage = 0;
                        }
                        //The other keycards goes here
                        if (basetemplate.Id != ItemTpl.KEYCARD_TERRAGROUP_LABS_ACCESS && basetemplate.Id != ItemTpl.KEYCARD_TERRAGROUP_LABS_RESIDENTIAL_UNIT &&  basetemplate.Properties.MaximumNumberOfUsage != 1) //Horrible, might need to rework this, TODO
                        {
                            basetemplate.Properties.MaximumNumberOfUsage = 0;
                        }
                    }
                    //Now multiply keys and keycards if they weren't infinite but keep them under threshold
                    if (basetemplate.Parent == "5c99f98d86f7745c314214b3" && basetemplate.Properties.MaximumNumberOfUsage != 0)
                    {
                        basetemplate.Properties.MaximumNumberOfUsage = Math.Max((int)(basetemplate.Properties.MaximumNumberOfUsage * svmconfig.Items.Keys.KeyUseMult), 1);
                        if (basetemplate.Properties.MaximumNumberOfUsage > svmconfig.Items.Keys.KeyDurabilityThreshold)
                        {
                            basetemplate.Properties.MaximumNumberOfUsage = svmconfig.Items.Keys.KeyDurabilityThreshold;
                        }
                    }
                    if (basetemplate.Parent == "5c164d2286f774194c5e69fa" && basetemplate.Properties.MaximumNumberOfUsage != 0)
                    {
                        //Ignoring access card once again
                        if (basetemplate.Id != ItemTpl.KEYCARD_TERRAGROUP_LABS_ACCESS && !svmconfig.Items.Keys.IgnoreAccessCard)
                        {
                            basetemplate.Properties.MaximumNumberOfUsage = Math.Max((int)(basetemplate.Properties.MaximumNumberOfUsage * svmconfig.Items.Keys.KeycardUseMult),1);
                        }
                        if (basetemplate.Properties.MaximumNumberOfUsage > svmconfig.Items.Keys.KeyCardDurabilityThreshold)
                        {
                            basetemplate.Properties.MaximumNumberOfUsage = svmconfig.Items.Keys.KeyCardDurabilityThreshold;
                        }
                    }
                }
                //Allow SMG weapon type to be used in Holster slot
                if (svmconfig.Items.SMGToHolster && basetemplate.Id == ItemTpl.INVENTORY_DEFAULT) 
                {
                    List<Slot> slots = basetemplate.Properties.Slots.ToList();
                    var filters = slots[2].Properties.Filters.ToList();
                    filters[0].Filter.Add(new MongoId("5447b5e04bdc2d62278b4567"));//FIX ENUM
                    slots[2].Properties.Filters = filters;
                    basetemplate.Properties.Slots = slots;
                }
                //Allow Pistol weapon type to be used in Primary and Secondary slots.
                if (svmconfig.Items.PistolToMain && basetemplate.Id == ItemTpl.INVENTORY_DEFAULT)
                {
                    List<Slot> slots = basetemplate.Properties.Slots.ToList();
                    var filters = slots[0].Properties.Filters.ToList();
                    var filtertwo = slots[1].Properties.Filters.ToList();
                    filters[0].Filter.Add(new MongoId("5447b5cf4bdc2d65278b4567"));
                    filters[0].Filter.Add(new MongoId("617f1ef5e8b54b0998387733"));
                    filtertwo[0].Filter.Add(new MongoId("5447b5cf4bdc2d65278b4567"));
                    filtertwo[0].Filter.Add(new MongoId("617f1ef5e8b54b0998387733"));//FIX ENUM
                    slots[0].Properties.Filters = filters;
                    slots[1].Properties.Filters = filtertwo;
                    basetemplate.Properties.Slots = slots;
                }
            }
        }
        public static bool SimpleFilter(MongoId[] AmmoType, string CurrentType)
        {
            return AmmoType.Any(Ammo => CurrentType.Contains(Ammo));
        }
        public static bool SimpleFilter(string[] AmmoType, string CurrentType)
        {
            return AmmoType.Any(Ammo => CurrentType.Contains(Ammo));
        }
    }
}
