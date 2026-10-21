namespace Greed.Models.ScavData
{
    public class SCAVPockets : Clamper
    {
        public int FourthWidth { get; set => SetClamped(ref field, value, 1, 6); } = 1;
        public int FourthHeight { get; set => SetClamped(ref field, value, 1, 20); } = 1;
        public int ThirdWidth { get; set => SetClamped(ref field, value, 1, 6); } = 1;
        public int ThirdHeight { get; set => SetClamped(ref field, value, 1, 20); } = 1;
        public int SecondWidth { get; set => SetClamped(ref field, value, 1, 6); } = 1;
        public int SecondHeight { get; set => SetClamped(ref field, value, 1, 20); } = 1;
        public int FirstWidth { get; set => SetClamped(ref field, value, 1, 6); } = 1;
        public int FirstHeight { get; set => SetClamped(ref field, value, 1, 20); } = 1;
    }
}
