using Greed.Models.AI;
using Greed.Models.CaseSpaceManager;
using Greed.Models.Flea;
using Greed.Models.HideoutData;
using Greed.Models.ItemData;
using Greed.Models.Looting;
using Greed.Models.PlayerData;
using Greed.Models.PMCData;
using Greed.Models.Questing;
using Greed.Models.Raiding;
using Greed.Models.ScavData;
using Greed.Models.TraderServices;
using Greed.Models.Trading;

namespace Greed.Models
{
    public class MainClass
    {
        public class MainConfig
        {
            public string PresetNotes { get; set; } = "";
            public Items Items { get; set; }
            public Hideout Hideout { get; set; }
            public Traders Traders { get; set; }
            public Loot Loot { get; set; }
            public Player Player { get; set; }
            public Raids Raids { get; set; }
            public Fleamarket Fleamarket { get; set; }
            public Services Services { get; set; }
            public Quests Quests { get; set; }
            public CSM CSM { get; set; }
            public Scav Scav { get; set; }
            public Bots Bots { get; set; }
            public PMC PMC { get; set; }
            public Custom Custom { get; set; }
            public MainConfig()
            {
                Items = new Items();
                Hideout = new Hideout();
                Traders = new Traders();
                Loot = new Loot();
                Player = new Player();
                Raids = new Raids();
                Fleamarket = new Fleamarket();
                Services = new Services();
                Quests = new Quests();
                CSM = new CSM();
                Scav = new Scav();
                Bots = new Bots();
                PMC = new PMC();
                Custom = new Custom();
            }
        }
    }
}
