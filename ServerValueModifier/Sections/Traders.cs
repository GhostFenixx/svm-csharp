using Greed.Models;
using Greed.Models.Questing;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Spt.Config;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services;

namespace ServerValueModifier.Sections
{
    internal class Traders(ISptLogger<SVM> logger, ConfigServer configServer, DatabaseService databaseService, MainClass.MainConfig svmcfg)
    {

        public void TradersSection()
        {
            //Defining Globals, SPT's config of traders and traders database
            Globals globals = databaseService.GetGlobals();
            TraderConfig traderConfig = configServer.GetConfig<TraderConfig>();
            QuestConfig questConfig = configServer.GetConfig<QuestConfig>();
            Dictionary<MongoId, Quest> quests = databaseService.GetQuests();
            Dictionary<MongoId, Trader> traders = databaseService.GetTraders();
            //Need to move trader array outside the method
            string[] traderArray = ["54cb50c76803fa8b248b4571", "54cb57776803fa99248b456e", "58330581ace78e27b8b10cee", "5935c25fb3acc3127c3d8cd9", "5a7c2eca46aef81a7ca2145d", "5ac3b934156ae10c4430e83c", "5c0647fdd443bc2504c2d371", "6617beeaa9cfa777ca915b7c", "579dc571d53a0658a154fbec"];
            Greed.Models.Trading.TraderMarkup traderMarkupList = svmcfg.Traders.TraderMarkup;
            Greed.Models.Trading.TraderSell sell = svmcfg.Traders.TraderSell;
            double[] sellarray = [sell.Prapor, sell.Therapist, sell.Skier, sell.Peacekeeper, sell.Mechanic, sell.Ragman, sell.Jaeger, sell.Ref];
            int[] Discount = [traderMarkupList.Prapor, traderMarkupList.Therapist, traderMarkupList.Skier, traderMarkupList.Peacekeeper, traderMarkupList.Mechanic, traderMarkupList.Ragman, traderMarkupList.Jaeger, traderMarkupList.Ref, traderMarkupList.Fence];

            //Fence settings
            if (svmcfg.Traders.Fence.EnableFence)
            {
                traderConfig.Fence.AssortSize = svmcfg.Traders.Fence.AmountOnSale;
                traderConfig.Fence.DiscountOptions.AssortSize = svmcfg.Traders.Fence.PremiumAmountOnSale;
                traderConfig.Fence.WeaponPresetMinMax.Min = svmcfg.Traders.Fence.PresetCount;
                traderConfig.Fence.WeaponPresetMinMax.Max = svmcfg.Traders.Fence.PresetCount;
                traderConfig.Fence.PresetPriceMult = svmcfg.Traders.Fence.PresetMult;
                traderConfig.Fence.ItemPriceMult = svmcfg.Traders.Fence.PriceMult;
                traderConfig.Fence.WeaponDurabilityPercentMinMax.Current.Min = svmcfg.Traders.Fence.GunDurability_Min;
                traderConfig.Fence.WeaponDurabilityPercentMinMax.Current.Max = svmcfg.Traders.Fence.GunDurability_Max;
                traderConfig.Fence.ArmorMaxDurabilityPercentMinMax.Current.Min = svmcfg.Traders.Fence.ArmorDurability_Min;
                traderConfig.Fence.ArmorMaxDurabilityPercentMinMax.Current.Max = svmcfg.Traders.Fence.ArmorDurability_Max;

                //Fence stock time
                foreach (UpdateTime test in traderConfig.UpdateTime)//Funny, i thought i'd get rid of this foreach.
                {
                    if (test.TraderId == "579dc571d53a0658a154fbec")
                    {
                        test.Seconds.Min = svmcfg.Traders.Fence.StockTime_Min * 60;
                        test.Seconds.Max = svmcfg.Traders.Fence.StockTime_Max * 60;
                    }
                }
            }
            //LightKeeper options
            if (svmcfg.Traders.LightKeeper.EnableLightKeeper)
            {
                globals.Configuration.BufferZone.CustomerAccessTime = svmcfg.Traders.LightKeeper.AccessTime * 60;
                globals.Configuration.BufferZone.CustomerKickNotifTime = svmcfg.Traders.LightKeeper.LeaveTime * 60;
            }

            //Durability of items to be sold to traders
            globals.Configuration.TradingSettings.BuyoutRestrictions.MinDurability = (double)svmcfg.Traders.MinDurabSell / 100;
            //Time given to redeem rewards for completed quests
            questConfig.MailRedeemTimeHours["default"] = svmcfg.Traders.QuestRedeemDefault;
            questConfig.MailRedeemTimeHours["unheard_edition"] = svmcfg.Traders.QuestRedeemUnheard;
            //Whether bought items from traders considered FIR
            traderConfig.PurchasesAreFoundInRaid = svmcfg.Traders.FIRTrade;
            //Trader access by default
            traders["5c0647fdd443bc2504c2d371"].Base.UnlockedByDefault = svmcfg.Traders.UnlockJaeger;
            traders["6617beeaa9cfa777ca915b7c"].Base.UnlockedByDefault = svmcfg.Traders.UnlockRef;
            int i = 0;//Horrible solution, simple enumerator to avoid using dictionary, will change later, aka TODO.
            foreach (var trader in traderArray)
            {
                if (traderArray[i] != "579dc571d53a0658a154fbec")//Bandaid, added it to fit markup yet there is no assort to edit for fence
                {
                    foreach (var assort in traders[trader].Assort.BarterScheme)
                    {
                        switch (assort.Value[0][0].Template)
                        {
                            case "5449016a4bdc2d6f028b456f":
                            case "569668774bdc2da2298b4568":
                            case "5696686a4bdc2da3298b456a":
                            case "5d235b4d86f7742e017bc88a":
                                if (assort.Value[0][0].Count is not null)
                                {
                                    assort.Value[0][0].Count *= sellarray[i];
                                }
                                break;
                        }
                    }
                }
                foreach (var levels in traders[trader].Base.LoyaltyLevels)
                {
                    levels.BuyPriceCoefficient = 100 - Discount[i];
                }
                i++;
            }
            //if (svmcfg.Traders.RemoveTimeCondition || svmcfg.Traders.FIRRestrictsQuests || svmcfg.Traders.AllQuestsAvailable)
            //{
            //Had to sacrifice 'if' to fit multiplier field.
            foreach (KeyValuePair<MongoId, Quest> questid in quests)
            {
                List<QuestCondition>? startcondition = questid.Value.Conditions.AvailableForStart;
                List<QuestCondition>? finishcondition = questid.Value.Conditions.AvailableForFinish;
                //Remove conditions where you need wait 12/24H to acquire new quest after handling one.
                if (svmcfg.Traders.RemoveTimeCondition)
                {
                    foreach (var requirements in startcondition)
                    {
                        if (requirements.AvailableAfter is not null)
                        {
                            requirements.AvailableAfter = 0;
                        }
                    }
                }
                //Checking for every quest 'availableForFinish' aka in process quests to remove FIR requirements.
                // if (svmcfg.Traders.FIRRestrictsQuests)
                //{
                foreach (var requirements in finishcondition)
                {
                    requirements.PlantTime = requirements.PlantTime * svmcfg.Traders.PlantingTime;
                    if (requirements.OnlyFoundInRaid is not null && svmcfg.Traders.FIRRestrictsQuests)
                    {
                        requirements.OnlyFoundInRaid = false;
                    }
                }
                //Removing quest condition to start one
                //}
                if (svmcfg.Traders.AllQuestsAvailable) // Added into single cycle TODO check for problems.
                {
                    startcondition = [];
                }
                questid.Value.Conditions.AvailableForStart = startcondition;
                questid.Value.Conditions.AvailableForFinish = finishcondition;
            }
            //}
            if (svmcfg.Traders.TradersLvl4)
            {
                foreach (KeyValuePair<MongoId, Trader> traderid in traders)
                {
                    List<TraderLoyaltyLevel>? traderloyalty = traderid.Value.Base.LoyaltyLevels;
                    foreach (TraderLoyaltyLevel loyalty in traderloyalty)
                    {
                        loyalty.MinLevel = 1;
                        loyalty.MinSalesSum = 0;
                        loyalty.MinStanding = 0;
                    }
                }
            }
            //Remove quest requirements, adjust amount of items and how many you can buy from traders -- Very smooth comparing to JS.
            foreach (var traderid in traders)
            {
                if (traderid.Key != "638f541a29ffd1183d187f57" && traderid.Key != "579dc571d53a0658a154fbec")
                {
                    try
                    {
                        if (svmcfg.Traders.UnlockQuestAssort && traderid.Value.QuestAssort["success"] is not null)
                        {
                            traderid.Value.QuestAssort["success"] = []; // please work
                        }
                    }
                    catch 
                    {
                        logger.Warning("[SVM] Failed to modify trader quest assort with name: " + traderid.Value.Base.Name);
                    }
                    //Find all trader offers, sort them through currencies (RUB,USD,EUR,GPcoin,Lega)
                    //Then Adjust their amounts and restrictions
                    //If not falling under ID - it will count as offer using other items aka default = barter.
                    Random rnd = new();
                    foreach (var scheme in traderid.Value.Assort.BarterScheme)
                    {
                        var barter = scheme.Value[0][0].Template;

                        switch (barter)
                        {
                            case "5449016a4bdc2d6f028b456f":
                            case "569668774bdc2da2298b4568":
                            case "5696686a4bdc2da3298b456a":
                            case "5d235b4d86f7742e017bc88a":
                            case "6656560053eaaa7a23349c86":
                                foreach (Item elem in traderid.Value.Assort.Items)
                                {
                                    if (elem.Id == scheme.Key)//I don't remember why - but this is very important.
                                    {
                                        elem.Upd.StackObjectsCount *= svmcfg.Traders.CurrencyOffers;
                                        if (elem.Upd.StackObjectsCount >= 999999 && elem.Upd.UnlimitedCount is not null && !svmcfg.Traders.RandomizeAssort)
                                        {
                                            elem.Upd.StackObjectsCount = 999999;
                                            elem.Upd.UnlimitedCount = true;
                                        }
                                        else //Randomize Assort - first cycle should be done here, the rest dynamically done in TraderOverride router.
                                        {
                                            elem.Upd.UnlimitedCount = false;
                                            elem.Upd.StackObjectsCount = rnd.Next(480);
                                        }
                                            elem.Upd.BuyRestrictionMax = (int?)(elem.Upd.BuyRestrictionMax * svmcfg.Traders.CurrencyRestrictions);
                                        //Safety guard in case something hit less than 1
                                        if (elem.Upd.BuyRestrictionMax < 1)
                                        {
                                            elem.Upd.BuyRestrictionMax = 1;
                                        }
                                        if (elem.Upd.StackObjectsCount < 1 && !svmcfg.Traders.RandomizeAssort)
                                        {
                                            elem.Upd.StackObjectsCount = 1;
                                        }
                                    }
                                }
                                break;
                            default:
                                foreach (Item elem in traderid.Value.Assort.Items)
                                {
                                    if (elem.Id == scheme.Key)
                                    {
                                        elem.Upd.StackObjectsCount *= svmcfg.Traders.BarterOffers;
                                        if (elem.Upd.StackObjectsCount >= 999999 && elem.Upd.UnlimitedCount is not null)
                                        {
                                            elem.Upd.StackObjectsCount = 999999;
                                            elem.Upd.UnlimitedCount = true;
                                        }
                                        else //Randomize Assort - first cycle should be done here, the rest dynamically done in TraderOverride router.
                                        {
                                            elem.Upd.UnlimitedCount = false;
                                            elem.Upd.StackObjectsCount = rnd.Next(480);
                                        }
                                        elem.Upd.BuyRestrictionMax = (int?)(elem.Upd.BuyRestrictionMax * svmcfg.Traders.BarterRestrictions);
                                        //Safety guard in case something hit less than 1
                                        if (elem.Upd.BuyRestrictionMax < 1)
                                        {
                                            elem.Upd.BuyRestrictionMax = 1;
                                        }
                                        if (elem.Upd.StackObjectsCount < 1 && !svmcfg.Traders.RandomizeAssort)
                                        {
                                            elem.Upd.StackObjectsCount = 1;
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }
            }
        }
    }
}