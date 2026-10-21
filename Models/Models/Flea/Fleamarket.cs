namespace Greed.Models.Flea
{
    public class Fleamarket : Clamper
    {
        public bool EnableFleaConditions { get; set; }
        public bool EnablePlayerOffers { get; set; }
        public bool FleaFIR { get; set; }
        public bool FleaNoFIRSell { get; set; }
        public bool EventOffers { get; set; }
        public int SellOffersAmount { get; set => SetClamped(ref field, value, 0, 100); } = 10;
        public FleaConditions FleaConditions { get; set; }
        public bool OverrideOffers { get; set; }
        public int FleaMarketLevel { get; set => SetClamped(ref field, value, 0, 100); } = 15;
        public bool DisableBSGList { get; set; }
        public bool EnableFleamarket { get; set; }
        public double Sell_mult { get; set => SetClamped(ref field, value, 0.01, 100); } = 1.24;
        public int Tradeoffer_max { get; set => SetClamped(ref field, value, 0, 60); } = 1;
        public double Rep_loss { get; set => SetClamped(ref field, value, 0, 10); } = 0.03;
        public bool TieredFlea { get; set; } = true;
        public double Rep_gain { get; set => SetClamped(ref field, value, 0, 10); } = 0.02;
        public int Tradeoffer_min { get; set => SetClamped(ref field, value, 0, 60); } = 0;
        public int Sell_chance { get; set => SetClamped(ref field, value, 0, 100); } = 50;
        public double FeesMult { get; set => SetClamped(ref field, value, 0, 10); } = 1;
        public DynamicOffers DynamicOffers { get; set; }

        public Fleamarket()
        {
            FleaConditions = new FleaConditions();
            DynamicOffers = new DynamicOffers();
        }
    }
}
