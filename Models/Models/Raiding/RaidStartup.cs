namespace Greed.Models.Raiding
{
    public class RaidStartup
    {
        public bool EnableRaidStartup { get; set; }
        public int TimeBeforeDeployLocal { get; set; } = 10;
        public int AIAmount { get; set; } = 0;
        public bool SaveLoot { get; set; } = false;
        public int AIDifficulty { get; set; } = 0;
        public bool MIAEndofRaid { get; set; } = true;
        public bool TaggedAndCursed { get; set; }
        public bool EnableBosses { get; set; } = true;
        public bool ScavWars { get; set; }
    }
}
