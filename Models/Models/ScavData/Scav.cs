using Greed.Models.PlayerData;

namespace Greed.Models.ScavData
{
    public class Scav
    {
        public SCAVPockets SCAVPockets { get; set; }
        public bool HostileBosses { get; set; }
        public bool FriendlyBosses { get; set; }
        public double CarBaseStanding { get; set; } = 0.25;
        public int ScavTimer { get; set; } = 900;
        public bool ScavCustomPockets { get; set; }
        public bool ScavLab { get; set; }
        public bool FriendlyScavs { get; set; }
        public bool HostileScavs { get; set; }
        public double StandingFriendlyKill { get; set; } = -0.04;
        public double StandingPMCKill { get; set; } = 0.01;
        public Health Health { get; set; }
        public bool EnableScavHealth { get; set; }
        public bool EnableScav { get; set; }
        public Stats ScavStats { get; set; }
        public bool EnableStats { get; set; }
        public Scav()
        {
            SCAVPockets = new SCAVPockets();
            Health = new Health()
            {
                Head = 35,
                Chest = 85,
                Stomach = 70,
                LeftArm = 60,
                LeftLeg = 65,
                RightArm = 60,
                RightLeg = 65
            };
            ScavStats = new Stats();
        }
    }
}
