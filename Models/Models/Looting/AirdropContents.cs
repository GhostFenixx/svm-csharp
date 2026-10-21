namespace Greed.Models.Looting
{
    public class AirdropContents : Clamper
    {
        public int ArmorMin { get; set => SetClamped(ref field, value, 0, 59); }
        public int ArmorMax { get; set => SetClamped(ref field, value, 0, 60); }
        public int BarterMin { get; set => SetClamped(ref field, value, 0, 59); }
        public int BarterMax { get; set => SetClamped(ref field, value, 0, 60); }
        public int PresetMin { get; set => SetClamped(ref field, value, 0, 59); }
        public int PresetMax { get; set => SetClamped(ref field, value, 0, 60); }
        public int CratesMin { get; set => SetClamped(ref field, value, 0, 9); }
        public int CratesMax { get; set => SetClamped(ref field, value, 0, 10); }
    }
}
