namespace Greed.Models.TraderServices
{
    public class RepairBox
    {
        public bool NoRandomRepair { get; set; }
        public bool OpGunRepair { get; set; }
        public double ArmorSkillMult { get; set; } = 0.05;
        public double WeaponMaintenanceSkillMult { get; set; } = 0.6;
        public double IntellectSkillMultWeaponKit { get; set; } = 0.045;
        public double IntellectSkillMultArmorKit { get; set; } = 0.03;
        public double IntellectSkillLimitTraders { get; set; } = 0.6;
        public double IntellectSkillLimitKit { get; set; } = 0.6;
        public bool OpArmorRepair { get; set; }
        public double RepairMult { get; set; } = 1;
    }
}
