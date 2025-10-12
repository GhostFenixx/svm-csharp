namespace Greed.Models.Raiding
{
    public class Exfils
    {
        public int CarSandbox { get; set; } = 5000;
        public int CarShoreline { get; set; } = 5000;
        public int CoopPaidSandbox { get; set; } = 5000;
        public int CoopPaidShoreline { get; set; } = 5000;
        public int CoopPaidStreets { get; set; } = 5000;
        public int CoopPaidLighthouse { get; set; } = 5000;
        public int CarLighthouse { get; set; } = 5000;
        public int CarExtractTime { get; set; } = 60;
        public bool ArmorExtract { get; set; }
        public bool CoopPaid { get; set; }
        public bool FenceGift { get; set; }
        public int CoopPaidInterchange { get; set; } = 5000;
        public int CoopPaidWoods { get; set; } = 5000;
        public int CoopPaidReserve { get; set; } = 5000;

        
        public bool NoBackpack { get; set; }
        public bool FreeCoop { get; set; }
        public int CarInterchange { get; set; } = 5000;
        public int CarWoods { get; set; } = 5000;
        public int CarStreets { get; set; } = 5000;
        public int CarCustoms { get; set; } = 5000;
        public int CoopPaidCustoms { get; set; } = 5000;
        public bool ExtendedExtracts { get; set; }
        public bool ChanceExtracts { get; set; }
        public bool GearExtract { get; set; }
    }
}
