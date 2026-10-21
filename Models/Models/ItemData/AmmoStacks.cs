namespace Greed.Models.ItemData
{
    public class AmmoStacks : Clamper
    {
        public int MarksmanRound { get; set => SetClamped(ref field, value, 5, 1200); } = 50;
        public int RifleRound { get; set => SetClamped(ref field, value, 5, 1400); } = 80;
        public int ShotgunRound { get; set => SetClamped(ref field, value, 2, 400); } = 25;
        public int PistolRound { get; set => SetClamped(ref field, value, 5, 1500); } = 100;
        public int LargeCaliberRound { get; set => SetClamped(ref field, value, 5, 800); } = 30;
    }
}
