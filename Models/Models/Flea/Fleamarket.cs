namespace Greed.Models.Flea
{
    public class Fleamarket
    {
        public bool EnableFleaConditions { get; set; }
        public bool EnablePlayerOffers { get; set; }
        public bool FleaFIR { get; set; }
        public bool FleaNoFIRSell { get; set; }
        public bool EventOffers { get; set; }
        public int SellOffersAmount { get; set; } = 10;
        public FleaConditions FleaConditions { get; set; }
        public bool OverrideOffers { get; set; }
        public int FleaMarketLevel { get; set; } = 15;
        public List<object> FleaBlacklist { get; set; }
        //public TraderStaticOffers TraderStaticOffers { get; set; }
        public bool DisableBSGList { get; set; }
        public bool EnableFleamarket { get; set; }
        public double Sell_mult { get; set; } = 1.24;
        public int Tradeoffer_max { get; set; } = 1;
        public double Rep_loss { get; set; } = 0.03;
        public double Rep_gain { get; set; } = 0.02;
        public int Tradeoffer_min { get; set; } = 0;
        public int Sell_chance { get; set; } = 50;
        public bool EnableFees { get; set; } = true;
        public DynamicOffers DynamicOffers { get; set; }

        public Fleamarket()
        {
            FleaConditions = new FleaConditions();
            DynamicOffers = new DynamicOffers();
        }
    }
}
