namespace Greed.Models.PlayerData
{
    public class CharXP : Clamper
    {
        public int ScavKill { get; set => SetClamped(ref field, value, 5, 10000); } = 80;
        public double ScavHMult { get; set => SetClamped(ref field, value, 0, 100); } = 1.1;
        public int PMCKill { get; set => SetClamped(ref field, value, 0, 10000); } = 175;
        public double PMCHMult { get; set => SetClamped(ref field, value, 0, 100); } = 1.2;
    }
}
