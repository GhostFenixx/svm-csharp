namespace Greed.Models.AI
{
    public class BotDurability : Clamper
    {
        public int ArmorMin { get; set => SetClamped(ref field, value, 0, 100); }
        public int ArmorMax { get; set => SetClamped(ref field, value, 0, 100); }
        public int WeaponMin { get; set => SetClamped(ref field, value, 0, 100); }
        public int WeaponMax { get; set => SetClamped(ref field, value, 0, 100); }
    }
}