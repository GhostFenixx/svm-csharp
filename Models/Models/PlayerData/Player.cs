namespace Greed.Models.PlayerData
{
    public class Player
    {
        public bool EnableFatigue { get; set; }
        public Stats PMCStats { get; set; }
        public CharXP CharXP { get; set; }
        public RaidMult RaidMult { get; set; }
        public bool EnableStats { get; set; }
        public Skills Skills { get; set; }
        public bool FallDamage { get; set; }
        public double BlackStomach { get; set; } = 5;
        public double HydrationLoss { get; set; } = 1;
        public double EnergyLoss { get; set; } = 1;
        public bool EnableHealth { get; set; }
        public double SkillProgMult { get; set; } = 0.4;
        public Health Health { get; set; }
        public double WeaponSkillMult { get; set; } = 1;
        public bool EnablePlayer { get; set; }
        public DiedHealth DiedHealth { get; set; }
        public int MaxStaminaLegs { get; set; } = 115;
        public int MaxStaminaHands { get; set; } = 80;
        public bool EnableStaminaHands { get; set; }
        public bool EnableStaminaLegs { get; set; }
        public double RegenStaminaLegs { get; set; } = 4.5;
        public double RegenStaminaHands { get; set; } = 2.1;
        public int JumpConsumption { get; set; } = 14;
        public int LayToStand { get; set; } = 20;
        public int CrouchToStand { get; set; } = 11;
        public double Standing { get; set; } = 1;
        public double LayingDown { get; set; } = 0.15;
        public double Crouching { get; set; } = 0.75;
        public bool UnlimitedStamina { get; set; }

        public Player()
        {
            PMCStats = new Stats();
            CharXP = new CharXP();
            RaidMult = new RaidMult();
            Skills = new Skills();
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
            DiedHealth = new DiedHealth();
        }
    }
}
