namespace Greed.Models.ItemData
{
    public class Items : Clamper
    {
        public bool ExamineKeys { get; set; }
        public bool WeaponHeatOff { get; set; }
        public bool SMGToHolster { get; set; }
        public bool PistolToMain { get; set; }
        public bool AllExaminedItems { get; set; }
        public bool EquipRigsWithArmors { get; set; }
        public bool RemoveSecureContainerFilters { get; set; }
        public bool EnableCurrency { get; set; }
        public int RubStack { get; set => SetClamped(ref field, value, 50000, 50000000); } = 1000000;
        public int DollarStack { get; set => SetClamped(ref field, value, 5000, 1000000); } = 50000;
        public int EuroStack { get; set => SetClamped(ref field, value, 5000, 1000000); } = 50000;
        public int GPStack { get; set => SetClamped(ref field, value, 10, 500); } = 100;
        public double AmmoLoadSpeed { get; set => SetClamped(ref field, value, 0, 40); } = 1;
        public double AmmoUnLoadSpeed { get; set => SetClamped(ref field, value, 0, 40); } = 1;
        public double LootExp { get; set => SetClamped(ref field, value, 0, 100); } = 1;
        public bool EnableItems { get; set; }
        public double ExamineExp { get; set => SetClamped(ref field, value, 0, 100); } = 1;
        public AmmoStacks AmmoStacks { get; set; }
        public Keys Keys { get; set; }
        public bool AmmoSwitch { get; set; }
        public bool RemoveRaidRestr { get; set; }
        public bool RemoveBackpacksRestrictions { get; set; }
        public bool SurvCMSToSpec { get; set; }
        public bool SurvCMSSecConBlock { get; set; }
        public bool NoGearPenalty { get; set; }
        public bool RaidDrop { get; set; }

        public double MisfireChance { get; set => SetClamped(ref field, value, 0, 10); } = 1;
        public double FragmentMult { get; set => SetClamped(ref field, value, 0, 10);} = 1;
        public double HeatFactor { get; set => SetClamped(ref field, value, 0, 10);} = 1;
        public double MalfunctChanceMult { get; set => SetClamped(ref field, value, 0, 10); } = 1;
        public double ExamineTime { get; set => SetClamped(ref field, value, 0, 10); } = 1;
        public double WeightChanger { get; set => SetClamped(ref field, value, 0, 5); } = 1;
        public int BackpackStacking { get; set => SetClamped(ref field, value, 2, 25);} = 7;
        public double ItemPriceMult{get; set => SetClamped(ref field, value, 0.01, 5);} = 1;
        public Items()
        {
            Keys = new Keys();
            AmmoStacks = new AmmoStacks();
        }
    }
}
