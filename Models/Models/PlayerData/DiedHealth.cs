namespace Greed.Models.PlayerData
{
    public class DiedHealth : Clamper
    {
        public bool Saveeffects { get; set; } = true;
        public bool Savehealth { get; set; } = true;
        public double Health_blacked { get; set => SetClamped(ref field, value, 0, 1); } = 0.1;
    }
}
