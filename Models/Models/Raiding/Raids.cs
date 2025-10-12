namespace Greed.Models.Raiding
{
    public class Raids
    {
        public int SandboxAccessLevel { get; set; } = 20;
        public int RaidTime { get; set; } = 0;
        public bool SaveQuestItems { get; set; }
        public Exfils Exfils { get; set; }
        public bool NoRunThrough { get; set; }
        public int Timeacceleration { get; set; } = 7;
        public bool SafeExit { get; set; }
        public bool SaveGearAfterDeath { get; set; }
        public RaidEvents RaidEvents { get; set; }
        public bool LabInsurance { get; set; }
        public bool EnableRaids { get; set; }
        public bool Removelabkey { get; set; }

        public bool EnableCarCoop { get; set; }
        public bool ForceBTRFriendly { get; set; }

        public bool ForceTransitStash { get; set; }
        public int TransitHeight { get; set; } = 2;
        public int TransitWidth { get; set; } = 5;
        public bool ForceBTRStash { get; set; }
        public bool EnableBTR { get; set; }
        public int BTRCoverPrice { get; set; } = 30000;
        public int BTRTaxiPrice { get; set; } = 7000;

        public int BTRWoodsTimeMin { get; set; } = 5;

        public int BTRWoodsTimeMax { get; set; } = 10;
        public int BTRWoodsChance { get; set; } = 50;
        public int BTRStreetsChance { get; set; } = 50;
        public int BTRStreetsTimeMin { get; set; } = 5;

        public int BTRStreetsTimeMax { get; set; } = 10;

        public double UsecMult { get; set; } = 1.5;
        public double BearMult { get; set; } = 1;
        public double ScavMult { get; set; } = 0.8;
        public int BTRHeight { get; set; } = 2;
        public int BTRWidth { get; set; } = 5;
        public int Season { get; set; } = 0;
        public bool ForceSeason { get; set; }
        public RaidStartup RaidStartup { get; set; }
        public Raids()
        {
            Exfils = new Exfils();
            RaidEvents = new RaidEvents();
            RaidStartup = new RaidStartup();
        }
    }
}
