using Greed.Models;
using HarmonyLib;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Spt.Config;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services;

namespace ServerValueModifier.Sections
{
    internal class Fleamarket(ISptLogger<SVM> logger, ConfigServer configServer, DatabaseService databaseService, MainClass.MainConfig svmconfig)
    {
        public void FleamarketSection()
        {
            var fleaconfig = configServer.GetConfig<RagfairConfig>();
            Globals globals = databaseService.GetGlobals();
            if (svmconfig.Fleamarket.EnablePlayerOffers)
            {
                globals.Configuration.RagFair.MinUserLevel = svmconfig.Fleamarket.FleaMarketLevel;
                fleaconfig.Dynamic.PurchasesAreFoundInRaid = svmconfig.Fleamarket.FleaFIR;
                globals.Configuration.RagFair.IsOnlyFoundInRaidAllowed = svmconfig.Fleamarket.FleaNoFIRSell;
                fleaconfig.Dynamic.Barter.ChancePercent = svmconfig.Fleamarket.BarterChance;
                fleaconfig.Sell.Fees = svmconfig.Fleamarket.EnableFees;
                fleaconfig.Sell.Chance.Base = svmconfig.Fleamarket.Sell_chance;
                fleaconfig.Sell.Chance.SellMultiplier = svmconfig.Fleamarket.Sell_mult;
                fleaconfig.Sell.Time.Min = svmconfig.Fleamarket.Tradeoffer_min;
                fleaconfig.Sell.Time.Max = svmconfig.Fleamarket.Tradeoffer_max;
                fleaconfig.Dynamic.RemoveSeasonalItemsWhenNotInEvent = !svmconfig.Fleamarket.EventOffers;
                globals.Configuration.RagFair.RatingIncreaseCount = svmconfig.Fleamarket.Rep_gain;
                globals.Configuration.RagFair.RatingDecreaseCount = svmconfig.Fleamarket.Rep_loss;
                if (svmconfig.Fleamarket.OverrideOffers)
                {
                    MaxActiveOfferCount offerCount = new();
                    offerCount.From = -100000;
                    offerCount.To = 100000;
                    offerCount.CountForSpecialEditions = svmconfig.Fleamarket.SellOffersAmount;
                    offerCount.Count = svmconfig.Fleamarket.SellOffersAmount;
                    globals.Configuration.RagFair.MaxActiveOfferCount = [offerCount];

                }
            }
            fleaconfig.TieredFlea.Enabled = !svmconfig.Fleamarket.TieredFlea;
            fleaconfig.Dynamic.Pack.ChancePercent = svmconfig.Fleamarket.DynamicOffers.BundleOfferChance;
            fleaconfig.Dynamic.ExpiredOfferThreshold = svmconfig.Fleamarket.DynamicOffers.ExpireThreshold;
            fleaconfig.Dynamic.OfferItemCount["default"].Min = svmconfig.Fleamarket.DynamicOffers.PerOffer_min;
            fleaconfig.Dynamic.OfferItemCount["default"].Max = svmconfig.Fleamarket.DynamicOffers.PerOffer_max;
            fleaconfig.Dynamic.PriceRanges.Default.Min = svmconfig.Fleamarket.DynamicOffers.Price_min;//Maybe someday i'll make a field for each one of them.
            fleaconfig.Dynamic.PriceRanges.Default.Max = svmconfig.Fleamarket.DynamicOffers.Price_max;
            fleaconfig.Dynamic.PriceRanges.Pack.Min = svmconfig.Fleamarket.DynamicOffers.Price_min;
            fleaconfig.Dynamic.PriceRanges.Pack.Max = svmconfig.Fleamarket.DynamicOffers.Price_max;
            fleaconfig.Dynamic.PriceRanges.Preset.Min = svmconfig.Fleamarket.DynamicOffers.Price_min;
            fleaconfig.Dynamic.PriceRanges.Preset.Max = svmconfig.Fleamarket.DynamicOffers.Price_max;
            fleaconfig.Dynamic.EndTimeSeconds.Min = svmconfig.Fleamarket.DynamicOffers.Time_min * 60;
            fleaconfig.Dynamic.EndTimeSeconds.Max = svmconfig.Fleamarket.DynamicOffers.Time_max * 60;
            fleaconfig.Dynamic.NonStackableCount.Min = svmconfig.Fleamarket.DynamicOffers.NonStack_min;
            fleaconfig.Dynamic.NonStackableCount.Max = svmconfig.Fleamarket.DynamicOffers.NonStack_max;
            fleaconfig.Dynamic.StackablePercent.Min = svmconfig.Fleamarket.DynamicOffers.Stack_min;
            fleaconfig.Dynamic.StackablePercent.Max = svmconfig.Fleamarket.DynamicOffers.Stack_max;
            //Currency Ratio
            fleaconfig.Dynamic.OfferCurrencyChangePercent[ItemTpl.MONEY_ROUBLES] = svmconfig.Fleamarket.DynamicOffers.Roubleoffers;
            fleaconfig.Dynamic.OfferCurrencyChangePercent[ItemTpl.MONEY_DOLLARS] = svmconfig.Fleamarket.DynamicOffers.Dollaroffers;
            fleaconfig.Dynamic.OfferCurrencyChangePercent[ItemTpl.MONEY_EUROS] = svmconfig.Fleamarket.DynamicOffers.Eurooffers;

            if (svmconfig.Fleamarket.EnableFleaConditions)//Flea Section > Item Conditions, horrible, as usual
            {
                fleaconfig.Dynamic.Condition["5422acb9af1c889c16000029"].Max.Min = (double)(svmconfig.Fleamarket.FleaConditions.FleaWeapons_Min / 100);
                fleaconfig.Dynamic.Condition["543be5664bdc2dd4348b4569"].Max.Min = (double)(svmconfig.Fleamarket.FleaConditions.FleaMedical_Min / 100);
                fleaconfig.Dynamic.Condition["5447e0e74bdc2d3c308b4567"].Max.Min = (double)(svmconfig.Fleamarket.FleaConditions.FleaSpec_Min / 100);
                fleaconfig.Dynamic.Condition["543be5e94bdc2df1348b4568"].Max.Min = (double)(svmconfig.Fleamarket.FleaConditions.FleaKeys_Min / 100);
                fleaconfig.Dynamic.Condition["5448e5284bdc2dcb718b4567"].Max.Min = (double)(svmconfig.Fleamarket.FleaConditions.FleaVests_Min / 100);
                fleaconfig.Dynamic.Condition["57bef4c42459772e8d35a53b"].Max.Min = (double)(svmconfig.Fleamarket.FleaConditions.FleaArmor_Min / 100);
                fleaconfig.Dynamic.Condition["543be6674bdc2df1348b4569"].Max.Min = (double)(svmconfig.Fleamarket.FleaConditions.FleaFood_Min / 100);
                fleaconfig.Dynamic.Condition["5422acb9af1c889c16000029"].Max.Max = (double)(svmconfig.Fleamarket.FleaConditions.FleaWeapons_Max / 100);
                fleaconfig.Dynamic.Condition["543be5664bdc2dd4348b4569"].Max.Max = (double)(svmconfig.Fleamarket.FleaConditions.FleaMedical_Max / 100);
                fleaconfig.Dynamic.Condition["5447e0e74bdc2d3c308b4567"].Max.Max = (double)(svmconfig.Fleamarket.FleaConditions.FleaSpec_Max / 100);
                fleaconfig.Dynamic.Condition["543be5e94bdc2df1348b4568"].Max.Max = (double)(svmconfig.Fleamarket.FleaConditions.FleaKeys_Max / 100);
                fleaconfig.Dynamic.Condition["5448e5284bdc2dcb718b4567"].Max.Max = (double)(svmconfig.Fleamarket.FleaConditions.FleaVests_Max / 100);
                fleaconfig.Dynamic.Condition["57bef4c42459772e8d35a53b"].Max.Max = (double)(svmconfig.Fleamarket.FleaConditions.FleaArmor_Max / 100);
                fleaconfig.Dynamic.Condition["543be6674bdc2df1348b4569"].Max.Max = (double)(svmconfig.Fleamarket.FleaConditions.FleaFood_Max / 100);
            }
        }
    }
}
