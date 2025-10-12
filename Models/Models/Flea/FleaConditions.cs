namespace Greed.Models.Flea
{
    public class FleaConditions
    {
        public int FleaFood_Min { get; set; } = 5;
        public int FleaArmor_Min { get; set; } = 5;
        public int FleaFood_Max { get; set; } = 100;
        public int FleaArmor_Max { get; set; } = 100;
        public int FleaMedical_Min { get; set; } = 60;
        public int FleaSpec_Min { get; set; } = 2;
        public int FleaMedical_Max { get; set; } = 100;
        public int FleaSpec_Max { get; set; } = 100;
        public int FleaWeapons_Min { get; set; } = 60;
        public int FleaVests_Min { get; set; } = 5;
        public int FleaKeys_Min { get; set; } = 97;
        public int FleaWeapons_Max { get; set; } = 100;
        public int FleaVests_Max { get; set; } = 100;
        public int FleaKeys_Max { get; set; } = 100;
    }
}
