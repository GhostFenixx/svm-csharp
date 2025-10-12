using Greed.Models;
using Greed.Models.CaseSpaceManager;
using SPTarkov.Common.Extensions;
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
    internal class Scav(ISptLogger<SVM> logger, ConfigServer configServer, DatabaseService databaseService, MainClass.MainConfig svmconfig, ICloner cloner, TemplateItem custompocket)
    {
        private readonly Globals globals = databaseService.GetGlobals();
        private readonly InRaidConfig inraid = configServer.GetConfig<InRaidConfig>();
        private readonly SPTarkov.Server.Core.Models.Spt.Server.Locations locationsdb = databaseService.GetLocations();
        Dictionary<MongoId, TemplateItem> items = databaseService.GetItems();
        public void ScavSection() //Shortest section by now, lul.
        {
            globals.Configuration.SavagePlayCooldown = svmconfig.Scav.ScavTimer;
            inraid.CarExtractBaseStandingGain = svmconfig.Scav.CarBaseStanding;
            locationsdb.Laboratory.Base.DisabledForScav = !svmconfig.Scav.ScavLab;

            foreach (var level in globals.Configuration.FenceSettings.Levels)
            {

                if (svmconfig.Scav.HostileScavs)
                {
                    level.Value.AreHostileBossesPresent = svmconfig.Scav.HostileScavs;
                }
                if (svmconfig.Scav.HostileBosses)
                {
                    level.Value.AreHostileBossesPresent = svmconfig.Scav.HostileBosses;
                }
                if (svmconfig.Scav.FriendlyScavs)
                {
                    level.Value.AreHostileBossesPresent = !svmconfig.Scav.FriendlyScavs;
                }
                if (svmconfig.Scav.FriendlyBosses)
                {
                    level.Value.AreHostileBossesPresent = !svmconfig.Scav.FriendlyBosses;
                }
            }

            if (svmconfig.Scav.ScavCustomPockets)
            {
                Greed.Models.ScavData.SCAVPockets pocketsize = svmconfig.Scav.SCAVPockets;
                foreach (var cell in custompocket.Properties.Grids)
                {
                    cell.Parent = custompocket.Id;
                }

                foreach (var cell in custompocket.Properties.Slots)
                {
                    cell.Parent = custompocket.Id;
                }
                List<Grid> grids = custompocket.Properties.Grids.ToList();
               grids[0].Id = "a8edfb0bce53d103d3f6229b";
               grids[0].Properties.CellsH = pocketsize.FirstWidth;
               grids[0].Properties.CellsV = pocketsize.FirstHeight;
               grids[1].Id = "a8edfb0bce53d103d3f6239b";
               grids[1].Properties.CellsH = pocketsize.SecondWidth;
               grids[1].Properties.CellsV = pocketsize.SecondHeight;
               grids[2].Id = "a8edfb0bce53d103d3f6249b";
               grids[2].Properties.CellsH = pocketsize.ThirdWidth;
               grids[2].Properties.CellsV = pocketsize.ThirdHeight;
               grids[3].Id = "a8edfb0bce53d103d3f6259b";
               grids[3].Properties.CellsH = pocketsize.FourthWidth;
               grids[3].Properties.CellsV = pocketsize.FourthHeight;

                if (pocketsize.FourthWidth == 0 || pocketsize.FourthHeight == 0)
                {
                   grids.Splice(3, 1);
                }

                if (pocketsize.ThirdWidth == 0 || pocketsize.ThirdHeight == 0)
                {
                   grids.Splice(2, 1);
                }

                if (pocketsize.SecondWidth == 0 || pocketsize.SecondHeight == 0)
                {
                   grids.Splice(1, 1);
                }

                if (pocketsize.FirstWidth == 0 || pocketsize.FirstHeight == 0)
                {
                   grids.Splice(0, 1);
                }
                custompocket.Properties.Grids = grids;
                items["a8edfb0bce53d103d3f6219b"] = custompocket;
            }
        }
    }
}
