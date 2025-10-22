namespace Greed.Models.ItemData
{
    public class Items
    {
        public bool ExamineKeys { get; set; }
        public bool WeaponHeatOff { get; set; }
        public bool SMGToHolster { get; set; }
        public bool PistolToMain { get; set; }
        public bool AllExaminedItems { get; set; }
        public bool EquipRigsWithArmors { get; set; }
        public bool RemoveSecureContainerFilters { get; set; }
        public int BackpackStacking { get; set; } = 7;
        public double MisfireChance { get; set; } = 1;
        public double FragmentMult { get; set; } = 1;
        public double HeatFactor { get; set; } = 1;
        public double ExamineTime { get; set; } = 1;
        public double MalfunctChanceMult { get; set; } = 1;
        public double WeightChanger { get; set; } = 1;
        public double ItemPriceMult { get; set; } = 1;
        public bool EnableCurrency { get; set; }
        public int RubStack { get; set; } = 1000000;
        public int DollarStack { get; set; } = 50000;
        public int GPStack { get; set; } = 100;
        public int EuroStack { get; set; } = 50000;
        public double AmmoLoadSpeed { get; set; } = 1;
        public double LootExp { get; set; } = 1;
        public bool EnableItems { get; set; }
        public double ExamineExp { get; set; } = 1;
        public AmmoStacks AmmoStacks { get; set; }
        public Keys Keys { get; set; }
        public bool AmmoSwitch { get; set; }
        public bool RemoveRaidRestr { get; set; }
        public bool RemoveBackpacksRestrictions { get; set; }
        public bool SurvCMSToSpec { get; set; }
        public bool SurvCMSSecConBlock { get; set; }
        public bool NoGearPenalty { get; set; }
        public bool RaidDrop { get; set; }
        public Items()
        {
            Keys = new Keys();
            AmmoStacks = new AmmoStacks();
        }
    }
}
