namespace Greed.Models.PlayerData
{
    public class Skills
    {
        public int SkillFatigueReset { get; set; } = 200;
        public double SkillFreshEffect { get; set; } = 1.3;
        public int SkillFPoints { get; set; } = 1;
        public int SkillPointsBeforeFatigue { get; set; } = 1;
        public double SkillMinEffect { get; set; } = 0.0001;
        public double SkillFatiguePerPoint { get; set; } = 0.6;
    }
}
