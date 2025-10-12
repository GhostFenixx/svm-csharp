namespace Greed.Models.CaseSpaceManager
{
    public class Pockets
    {
        public bool SpecGKeychain { get; set; }
        public bool SpecSimpleWallet { get; set; }
        public bool SpecWZWallet { get; set; }
        public bool SpecKeycardHolder { get; set; }
        public bool SpecKeytool { get; set; }
        public bool SpecInjectorCase { get; set; }
        public int SpecSlots { get; set; } = 3;
        public int FourthWidth { get; set; } = 1;
        public int FourthHeight { get; set; } = 1;
        public int ThirdWidth { get; set; } = 1;
        public int ThirdHeight { get; set; } = 1;
        public int SecondWidth { get; set; } = 1;
        public int SecondHeight { get; set; } = 1;
        public int FirstWidth { get; set; } = 1;
        public int FirstHeight { get; set; } = 1;
    }
}
