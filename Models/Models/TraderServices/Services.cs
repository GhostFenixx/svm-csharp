namespace Greed.Models.TraderServices
{
    public class Services
    {
        public RepairBox RepairBox { get; set; }
        public bool EnableHealMarkup { get; set; }
        public bool EnableInsurance { get; set; }
        public bool EnableTimeOverride { get; set; }
        public int FreeHealLvl { get; set; } = 5;
        public int FreeHealRaids { get; set; } = 30;
        public int ReturnChancePrapor { get; set; } = 85;
        public int ReturnChanceTherapist { get; set; } = 95;
        public int InsuranceInterval { get; set; } = 600;
        public int InsuranceTimeOverride { get; set; } = 30;
        public int InsuranceAttachmentChance { get; set; } = 10;
        public int TherapistStorageTime { get; set; } = 144;
        public int PraporStorageTime { get; set; } = 96;
        public int Prapor_Max { get; set; } = 36;
        public int Prapor_Min { get; set; } = 24;
        public int Therapist_Max { get; set; } = 24;
        public int Therapist_Min { get; set; } = 12;
        public double TherapistLvl1 { get; set; } = 1;
        public double TherapistLvl2 { get; set; } = 1.1;
        public double TherapistLvl3 { get; set; } = 1.2;
        public double TherapistLvl4 { get; set; } = 1.35;
        public double InsuranceMultTherapistLvl1 { get; set; } = 20;
        public double InsuranceMultTherapistLvl2 { get; set; } = 21;
        public double InsuranceMultTherapistLvl3 { get; set; } = 22;
        public double InsuranceMultTherapistLvl4 { get; set; } = 23;
        public double InsuranceMultPraporLvl1 { get; set; } = 16;
        public double InsuranceMultPraporLvl2 { get; set; } = 17;
        public double InsuranceMultPraporLvl3 { get; set; } = 18;
        public double InsuranceMultPraporLvl4 { get; set; } = 19;

        public bool EnableServices { get; set; }
        public bool EnableRepair { get; set; }
        public bool ClothesAnySide { get; set; }
        public bool ClothesLevelUnlock { get; set; }
        public bool ClothesFree { get; set; }
        public Services()
        {
            RepairBox = new RepairBox();
        }
    }
}
