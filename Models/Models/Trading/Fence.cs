namespace Greed.Models.Trading
{
    public class Fence : Clamper
    {
        public bool EnableFence { get; set; }
        public int ArmorDurability_Max { get; set => SetClamped(ref field, value, 0, 100); } = 60;
        public int GunDurability_Max { get; set => SetClamped(ref field, value, 0, 100); } = 60;
        public int ArmorDurability_Min { get; set => SetClamped(ref field, value, 0, 100); } = 35;
        public int GunDurability_Min { get; set => SetClamped(ref field, value, 0, 100); } = 35;
        public double PriceMult { get; set; } = 1.2;
        public int PremiumAmountOnSale { get; set; } = 50;
        public int PresetCount { get; set; } = 5;
        public int StockTime_Min { get; set; } = 50;
        public int StockTime_Max { get; set; } = 150;
        public int AmountOnSale { get; set; } = 140;
        public double PresetMult { get; set; } = 2;
    }
}
