//using Greed.Models;
//using SPTarkov.DI.Annotations;
//using SPTarkov.Server.Core.DI;
//using SPTarkov.Server.Core.Helpers;
//using SPTarkov.Server.Core.Models.Common;
//using SPTarkov.Server.Core.Models.Eft.Common;
//using SPTarkov.Server.Core.Models.Eft.Match;
//using SPTarkov.Server.Core.Models.Spt.Config;
//using SPTarkov.Server.Core.Models.Utils;
//using SPTarkov.Server.Core.Servers;
//using SPTarkov.Server.Core.Services;
//using SPTarkov.Server.Core.Utils;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static ServerValueModifier.Routers.Route;

//namespace ServerValueModifier.Routers
//{
//    public class Route // For those who actually checks my code - currently commented for this update - it would require separate fields and further testing due to inconsistency of results when paired with ABPS.
//    {
//        [Injectable]
//        public class CustomStaticRouter(JsonUtil jsonUtil, DatabaseService databaseService, ISptLogger<SVM> logger, ConfigServer configServer, ModHelper modHelper) : StaticRouter(jsonUtil, [
//            new RouteAction("/client/game/version/validate",
//                async (
//                    url,
//                    info,
//                    sessionID,
//                    output
//                ) =>
//                {
//                    logger.Warning("WE'RE TRIPPIN'!");
//                    MainClass.MainConfig cf = new SVMConfig(modHelper).CallConfig();
//                    if(cf.Raids.EnableRaids)
//                    {
//                    Sections.Events eventsLoad = new(logger, configServer, databaseService, cf, modHelper);
//                    eventsLoad.EventsSection();
//                    }
//                    return output;
//                }),
//            new RouteAction("/client/match/local/end",
//                async (
//                    url,
//                    info,
//                    sessionID,
//                    output
//                ) =>
//                {
//                   logger.Warning("Raid end route check");
//                                        MainClass.MainConfig cf = new SVMConfig(modHelper).CallConfig();
//                    if(cf.Raids.EnableRaids)
//                    {
//                    Sections.Events eventsLoad = new(logger, configServer, databaseService, cf, modHelper);
//                    eventsLoad.EventsSection();
//                    }
//                    return output;
//                }, typeof(EndLocalRaidRequestData))
//            ])
//        { }
//    }
//}
