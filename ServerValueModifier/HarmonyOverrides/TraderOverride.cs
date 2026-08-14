using Greed.Models;
using HarmonyLib.Tools;
using Microsoft.AspNetCore.Components;
using SPTarkov.Common.Models.Logging;
using SPTarkov.DI.Annotations;
using SPTarkov.Reflection.Patching;
using SPTarkov.Server.Core.Callbacks;
using SPTarkov.Server.Core.Controllers;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Helpers.Profile;
using SPTarkov.Server.Core.Helpers.Server;
using SPTarkov.Server.Core.Helpers.Traders;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Spt.Tables;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services;
using SPTarkov.Server.Core.Services.Commerce;
using SPTarkov.Server.Core.Utils;
using SPTarkov.Server.Core.Utils.Cloners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TraderID = SPTarkov.Server.Core.Models.Enums.Traders;

namespace ServerValueModifier.HarmonyOverrides
{
    [Injectable]
    public class TraderPatch : AbstractPatch
    {
        private static ModHelper modhelper;
        private static TradersTable traders;
        public static TraderAssortHelper assortHelper;

        protected override MethodBase GetTargetMethod()
        {
            return typeof(TraderAssortHelper).GetMethod(nameof(TraderAssortHelper.ResetExpiredTrader))!;
        }
        [PatchPrefix]
        public static bool Prefix(Trader trader)
        {
            try//I really hope it won't override existing trader assort and only take an effect of the one active, maybe find a way to select the one with passed timer?
            {
                MainClass.MainConfig svmcfg = new SVMConfig(modhelper).CallConfig();
                if (svmcfg.Traders.EnableTraders && svmcfg.Traders.RandomizeAssort)
                {
                    Random rnd = new();
                    foreach (var scheme in trader.Assort.BarterScheme)
                    {
                        var barter = scheme.Value[0][0].Template;
                        if (trader.Base.Id != TraderID.LIGHTHOUSEKEEPER && trader.Base.Id != TraderID.FENCE && trader.Assort is not null)//excessive check?
                        {
                            foreach (Item elem in trader.Assort.Items)
                            {
                                if (elem.Id == scheme.Key)
                                {
                                    elem.Upd.UnlimitedCount = false;
                                    elem.Upd.StackObjectsCount = rnd.Next(1440);//Major TODO
                                                                               //PLANS: Separate assort by IDs to apply different random ranges.
                                                                               // Weight system to roll 'Out of stock often' maybe?
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return true;
        }
    }
}
    //public class TraderOverride(ISptLogger<TraderAssortHelper> logger,
    //TradersTable traders,
    //TimeUtil timeUtil,
    //ProfileHelper profileHelper,
    //AssortHelper assortHelper,
    //TraderPurchasePersisterService traderPurchasePersisterService,
    //TraderHelper traderHelper,
    //FenceService fenceService,
    //ModHelper modhelper,
    //ICloner cloner) : TraderAssortHelper(logger, traders,  timeUtil, profileHelper, assortHelper, traderPurchasePersisterService, traderHelper, fenceService, cloner)
    //{
    //    public void ResetExpiredTrader(Trader trader)
    //    {
    //        try//I really hope it won't override existing trader assort and only take an effect of the one active, maybe find a way to select the one with passed timer?
    //        {
    //            MainClass.MainConfig svmcfg = new SVMConfig(modhelper).CallConfig();
    //            if (svmcfg.Traders.EnableTraders && svmcfg.Traders.RandomizeAssort)
    //            {
    //                Random rnd = new();
    //                foreach (var scheme in trader.Assort.BarterScheme)
    //                {
    //                    var barter = scheme.Value[0][0].Template;
    //                    if (trader.Base.Id != TraderID.LIGHTHOUSEKEEPER && trader.Base.Id != TraderID.FENCE && trader.Assort is not null)//excessive check?
    //                    {
    //                        foreach (Item elem in trader.Assort.Items)
    //                        {
    //                            if (elem.Id == scheme.Key)
    //                            {
    //                                elem.Upd.UnlimitedCount = false;
    //                                elem.Upd.StackObjectsCount = rnd.Next(480);//Major TODO
    //                                                                           //PLANS: Separate assort by IDs to apply different random ranges.
    //                                                                           // Weight system to roll 'Out of stock often' maybe?
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //        }
    //        trader.Assort.Items = GetPristineTraderAssorts(trader.Base.Id);

    //        // Update resupply value to next timestamp
    //        trader.Base.NextResupply = (int)traderHelper.GetNextUpdateTimestamp(trader.Base.Id);

    //        // Flag a refresh is needed so ragfair update() will pick it up
    //        trader.Base.RefreshTraderRagfairOffers = true;
    //    }
    //}
