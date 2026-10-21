namespace Greed.Models.Flea
{
    public class DynamicOffers : Clamper
    {
        public int ExpireThreshold { get; set => SetClamped(ref field, value, 120, 7200); } = 1400;
        public int BundleOfferChance { get; set => SetClamped(ref field, value, 0, 100); } = 6;
        public int BarterChance { get; set => SetClamped(ref field, value, 0, 100); } = 20;
        public int Stack_min { get; set => SetClamped(ref field, value, 1, 100); } = 10;
        public int PerOffer_min { get; set => SetClamped(ref field, value, 1, 100); } = 7;
        public int Stack_max { get; set => SetClamped(ref field, value, 1, 1200); } = 600;
        public int PerOffer_max { get; set => SetClamped(ref field, value, 0, 300); } = 30;
        public int Eurooffers { get; set => SetClamped(ref field, value, 0, 100); } = 8;
        public int Dollaroffers { get; set => SetClamped(ref field, value, 0, 100); } = 14;
        public int Roubleoffers { get; set => SetClamped(ref field, value, 0, 100); } = 78;
        public int NonStack_min { get; set => SetClamped(ref field, value, 0, 100); } = 1;
        public int Time_min { get; set => SetClamped(ref field, value, 0, 710); } = 6;
        public double Price_min { get; set => SetClamped(ref field, value, 0.01, 10); } = 0.8;
        public int NonStack_max { get; set => SetClamped(ref field, value, 0, 100); } = 10;
        public int Time_max { get; set => SetClamped(ref field, value, 1, 720); } = 60;
        public double Price_max { get; set => SetClamped(ref field, value, 0.01, 10); } = 1.2;
    }
}
