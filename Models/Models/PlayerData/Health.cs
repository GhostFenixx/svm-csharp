namespace Greed.Models.PlayerData
{
    public class Health : Clamper
    {
        public int LeftArm { get; set => SetClamped(ref field, value, 5, 1000); }
        public int RightArm { get; set => SetClamped(ref field, value, 5, 1000); }
        public int Head { get; set => SetClamped(ref field, value, 5, 1000); }
        public int Chest { get; set => SetClamped(ref field, value, 5, 1000); }
        public int Stomach { get; set => SetClamped(ref field, value, 5, 1000); }
        public int LeftLeg { get; set => SetClamped(ref field, value, 5, 1000); }
        public int RightLeg { get; set => SetClamped(ref field, value, 5, 1000); }
    }
}
