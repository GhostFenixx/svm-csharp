namespace Greed.Models.CaseSpaceManager
{
    public class Case : Clamper
    {
        public int Height { get; set => SetClamped(ref field, value, 1, 18); }
        public int Width { get; set => SetClamped(ref field, value, 1, 36); }
        public bool Filter { get; set; }
    }
}
