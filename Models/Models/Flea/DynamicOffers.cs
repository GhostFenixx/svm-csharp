namespace Greed.Models.Flea
{
    public class DynamicOffers
    {
        public int ExpireThreshold { get; set; } = 1400;
        public int BundleOfferChance { get; set; } = 6;
        public int Stack_min { get; set; } = 10;
        public int PerOffer_min { get; set; } = 7;
        public int Stack_max { get; set; } = 600;
        public int PerOffer_max { get; set; } = 30;
        public int Eurooffers { get; set; } = 8;
        public int Dollaroffers { get; set; } = 14;
        public int Roubleoffers { get; set; } = 78;
        public int NonStack_min { get; set; } = 1;
        public int Time_min { get; set; } = 6;
        public double Price_min { get; set; } = 0.8;
        public int NonStack_max { get; set; } = 10;
        public int Time_max { get; set; } = 60;
        public double Price_max { get; set; } = 1.2;
    }
}
