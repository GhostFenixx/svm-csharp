namespace Greed.Models.CaseSpaceManager
{
    public class Pockets : Clamper
    {
        public bool SpecGKeychain { get; set; }
        public bool SpecSimpleWallet { get; set; }
        public bool SpecWZWallet { get; set; }
        public bool SpecKeycardHolder { get; set; }
        public bool SpecKeytool { get; set; }
        public bool SpecInjectorCase { get; set; }
        public int SpecSlots { get; set => SetClamped(ref field, value, 1, 5); } = 3;
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
