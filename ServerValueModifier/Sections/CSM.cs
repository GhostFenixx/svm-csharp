using Greed.Models;
using Greed.Models.CaseSpaceManager;
using SPTarkov.Common.Extensions;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Spt.Config;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services;
using SPTarkov.Server.Core.Services.Mod;
using SPTarkov.Server.Core.Utils;
using System.Text.Json;
using SPTarkov.Server.Core.Utils.Cloners;
using SPTarkov.Server.Core.Constants;
using SPTarkov.Server.Core.Models.Common;



namespace ServerValueModifier.Sections
{
    internal class CSM(ISptLogger<SVM> logger, ConfigServer configServer, DatabaseService databaseService, MainClass.MainConfig svmconfig,ICloner cloner, TemplateItem custompocket)
    {

        public void CSMSection()//Will probably separate pockets and cases.
        {
            Dictionary<SPTarkov.Server.Core.Models.Common.MongoId, TemplateItem> items = databaseService.GetItems();
            InsuranceConfig insurance = configServer.GetConfig<InsuranceConfig>();
            //Temporary TODO: rework this when CSM is fully implemented
            //List of Cases IDs, order is important since used in cycles
            MongoId[] casesID = ["59fb016586f7746d0d4b423a", "5783c43d2459774bbe137486", "60b0f6c058e0b0481a09ad11", "5e2af55f86f7746d4159f07c", "59fb042886f7746c5005a7b2", "59fb023c86f7746d0d4b423c", "5b7c710788a4506dec015957", "5aafbde786f774389d0cbc0f", "5c127c4486f7745625356c13", "5c093e3486f77430cb02e593", "5aafbcd986f7745e590fff23", "5c0a840b86f7742ffa4f2482", "5b6d9ce188a4501afc1b2b25", "5d235bb686f77443f4331278", "59fafd4b86f7745ca07e1232", "590c60fc86f77412b13fddcf", "567143bf4bdc2d1a0f8b4567", "5c093db286f7740a1b2617e3", "619cbf7d23893217ec30b689", "619cbf9e0a7c3a1a2731940a", "62a09d3bcf4a99369e262447", "66bc98a01a47be227a5e956e", "67600929bd0a0549d70993f6", "67d3ed3271c17ff82e0a5b0b"];

            //Lacy said to use less arrays and more lists. also used case class for secure containers.
            if (svmconfig.CSM.EnableSecureCases)
            {
                //Temporary TODO: rework this when CSM is fully implemented
                //List of Sec.Cases IDs, order is important since used in cycles
                MongoId[] secConID = ["544a11ac4bdc2d470e8b456a", "5c093ca986f7740a1867ab12", "676008db84e242067d0dc4c9", "5857a8b324597729ab0a0e7d", "59db794186f77448bc595262", "5857a8bc2459772bad15db29", "665ee77ccf2d642e98220bca", "5732ee6a24597719ae0c0281", "64f6f4c5911bcdfe8b03b0dc"];
                Greed.Models.CaseSpaceManager.SecureContainers secCon = svmconfig.CSM.SecureContainers;
                List<Case> secsizes = [secCon.Alpha,secCon.Kappa,secCon.DesecratedKappa,secCon.Beta,secCon.Epsilon,secCon.Gamma,secCon.GammaTUE,secCon.WaistPouch,secCon.Dev];

                for (int i = 0; i < secConID.Length; i++)
                {
                    List<Grid> grids = items[secConID[i]].Properties.Grids.ToList();
                    grids[0].Properties.CellsH = secsizes[i].Width;
                    grids[0].Properties.CellsV = secsizes[i].Height;
                    items[secConID[i]].Properties.Grids = grids;
                }

            }
            if (svmconfig.CSM.EnableCases)
            {
                //Sorting Sizes, order is important to apply for IDs above
                Cases cases = svmconfig.CSM.Cases;
                List<Case> casesSizes = [
                        cases.MoneyCase,
                    cases.SimpleWallet,
                    cases.WZWallet,
                    cases.GrenadeCase,
                    cases.ItemsCase,
                    cases.WeaponCase,
                    cases.LuckyScav,
                    cases.AmmunitionCase,
                    cases.MagazineCase,
                    cases.DogtagCase,
                    cases.MedicineCase,
                    cases.ThiccItemsCase,
                    cases.ThiccWeaponCase,
                    cases.SiccCase,
                    cases.Keytool,
                    cases.DocumentsCase,
                    cases.PistolCase,
                    cases.Holodilnick,
                    cases.InjectorCase,
                    cases.KeycardHolderCase,
                    cases.GKeychain,
                    cases.StreamerCase,
                    cases.ArmorPlateCase,
                    cases.KeysCase
                    ];

                for (int i = 0; i < casesID.Length; i++)
                {
                    List<Grid> grids = items[casesID[i]].Properties.Grids.ToList();
                    grids[0].Properties.CellsH = casesSizes[i].Width;
                    grids[0].Properties.CellsV = casesSizes[i].Height;
                    if (casesSizes[i].Filter == true)
                    {
                        var filter = grids[0].Properties.Filters.ToList();
                        filter[0].Filter = ["54009119af1c881c07000029"];
                        filter[0].ExcludedFilter = [];
                        grids[0].Properties.Filters = filter;
                    }
                    items[casesID[i]].Properties.Grids = grids;
                }
            }

            if (svmconfig.CSM.CustomPocket)
            {
                Pockets pocketsize = svmconfig.CSM.Pockets;
                //TemplateItem custompocket = _cloner.Clone(items["627a4e6b255f7527fb05a0f6"]);
                //custompocket.Id = "a8edfb0bce53d103d3f62b9b";
                foreach (var cell in custompocket.Properties.Grids)
                {
                    cell.Parent = custompocket.Id;
                }

                foreach (var cell in custompocket.Properties.Slots)
                {
                    cell.Parent = custompocket.Id;
                }
                List<Grid> grids = custompocket.Properties.Grids.ToList();
                grids[0].Id = "a8edfb0bce53d103d3f62b0b";
                grids[0].Properties.CellsH = pocketsize.FirstWidth;
                grids[0].Properties.CellsV = pocketsize.FirstHeight;
                grids[1].Id = "a8edfb0bce53d103d3f62b1b";
                grids[1].Properties.CellsH = pocketsize.SecondWidth;
                grids[1].Properties.CellsV = pocketsize.SecondHeight;
                grids[2].Id = "a8edfb0bce53d103d3f62b2b";
                grids[2].Properties.CellsH = pocketsize.ThirdWidth;
                grids[2].Properties.CellsV = pocketsize.ThirdHeight;
                grids[3].Id = "a8edfb0bce53d103d3f62b3b";
                grids[3].Properties.CellsH = pocketsize.FourthWidth;
                grids[3].Properties.CellsV = pocketsize.FourthHeight;
                List<Slot> slots  = custompocket.Properties.Slots.ToList();
                slots[0].Id = "a8edfb0bce53d103d3f62b4b";
                slots[1].Id = "a8edfb0bce53d103d3f62b5b";
                slots[2].Id = "a8edfb0bce53d103d3f62b6b";

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

                switch (svmconfig.CSM.Pockets.SpecSlots)
                {
                    case 0:
                        slots.Splice(0, 3);
                        break;
                    case 1:
                       slots.Splice(1, 2);
                        break;
                    case 2:
                        slots.Splice(2, 1);
                        break;
                    case 3:
                        break;
                    case 4:
                        slots[3] = cloner.Clone(slots[2]);
                        slots[3].Id = "a8edfb0bce53d103d3f62b7b";
                        slots[3].Name = "SpecialSlot4";
                        slots[3].Parent = "a8edfb0bce53d103d3f62b9b";
                        //custompocket.Properties.Slots[3].Properties = custompocket.Properties.Slots[2].Properties; Why is this here? TODO CHECK REMOVE
                        break;
                    case 5://Sadly i can't recreate it the way it was in JS. TODO maybe?
                        slots[3] = cloner.Clone(slots[2]);
                        slots[3].Id = "a8edfb0bce53d103d3f62b7b";
                        slots[3].Name = "SpecialSlot4";
                        slots[3].Parent = "a8edfb0bce53d103d3f62b9b";
                        slots[4] = cloner.Clone(slots[2]);
                        slots[4].Id = "a8edfb0bce53d103d3f62b8b";
                        slots[4].Name = "SpecialSlot5";
                        slots[4].Parent = "a8edfb0bce53d103d3f62b9b";
                        break;
                }
                custompocket.Properties.Grids = grids;
                custompocket.Properties.Slots = slots;
                Pockets pocketfilter = svmconfig.CSM.Pockets;
                bool[] filter = [pocketfilter.SpecSimpleWallet, pocketfilter.SpecWZWallet, pocketfilter.SpecKeycardHolder, pocketfilter.SpecInjectorCase, pocketfilter.SpecKeytool, pocketfilter.SpecGKeychain];
                MongoId[] specialslotsfilter = ["5783c43d2459774bbe137486", "60b0f6c058e0b0481a09ad11", "619cbf9e0a7c3a1a2731940a", "619cbf7d23893217ec30b689", "59fafd4b86f7745ca07e1232", "62a09d3bcf4a99369e262447"];

                for (int i = 0; i < filter.Length; i++)//Bandaid to allow them inside special slots without them vanishing + removing insurance from them
                {
                    if (filter[i] == true)//This is awful, gotta test this, TODO.
                    {
                        items[specialslotsfilter[i]].Properties.DiscardLimit = -1;
                        items[specialslotsfilter[i]].Properties.InsuranceDisabled = true;
                    }
                }

                foreach (var slot in custompocket.Properties.Slots)
                {
                    for (int i = 0; i < filter.Length; i++)
                    {
                        if (filter[i] == true)
                        {
                            var filterlist = slot.Properties.Filters.ToList();
                            filterlist[0].Filter.Add(specialslotsfilter[i]);
                            slot.Properties.Filters = filterlist;
                        }
                    }
                }
                items["a8edfb0bce53d103d3f62b9b"] = custompocket;
            }
        }
    }
}
