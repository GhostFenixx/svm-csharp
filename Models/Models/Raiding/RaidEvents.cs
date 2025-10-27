namespace Greed.Models.Raiding
{
    public class RaidEvents
    {
        public bool DisableEvents { get; set; }
        public int KillaFactoryChance { get; set; } = 100;
        public int CultistBossesChance { get; set; } = 100;
        public int GoonsFactoryChance { get; set; } = 100;
        public bool CultistBosses { get; set; }
        public bool GoonsFactory { get; set; }
        public bool BossesOnCustoms { get; set; }
        public bool BossesOnHealthResort { get; set; }
        public bool TagillaInterchange { get; set; }
        public bool HealthResortIncludeGuards { get; set; }
        public int HoundsWoods { get; set; } = 5;
        public int HoundsCustoms { get; set; } = 5;
        public int SkierFighters { get; set; } = 4;
        public int PeaceFighters { get; set; } = 15;
        public bool Christmas { get; set; }
        public bool NonSeasonalQuests { get; set; }
        public bool Halloween { get; set; }
        public bool DisableZombies { get; set; }
        public bool DisableHalloweenAIFriendly { get; set; }
        public bool IncludeStreetBosses { get; set; }
        public bool KillaFactory { get; set; }
        public bool BossesOnReserve { get; set; }
        public bool AITypeOverride { get; set; }
        public int AIType { get; set; } = 0;
        public bool GlukharLabs { get; set; }
    }
}
