namespace Greed.Models.Looting
{
    public class Airdrops : Clamper
    {
        public AirdropContents Mixed { get; set; }
        public AirdropContents Medical { get; set; }
        public AirdropContents Barter { get; set; }
        public AirdropContents Weapon { get; set; }
        public int Sandbox_air { get; set => SetClamped(ref field, value, 0, 100); } = 13;
        public int Streets_air { get; set => SetClamped(ref field, value, 0, 100); } = 13;
        public int AirtimeMin { get; set => SetClamped(ref field, value, 0, 120); } = 5;
        public int AirtimeMax { get; set => SetClamped(ref field, value, 0, 120); } = 15;
        public int Lighthouse_air { get; set => SetClamped(ref field, value, 0, 100); } = 20;
        public int Bigmap_air { get; set => SetClamped(ref field, value, 0, 100); } = 20;
        public int Interchange_air { get; set => SetClamped(ref field, value, 0, 100); } = 20;
        public int Shoreline_air { get; set => SetClamped(ref field, value, 0, 100); } = 20;
        public int Reserve_air { get; set => SetClamped(ref field, value, 0, 100); } = 10;
        public int Woods_air { get; set => SetClamped(ref field, value, 0, 100); } = 25;

        public Airdrops()
        {
            Mixed = new AirdropContents()
            {
                ArmorMin = 1,
                ArmorMax = 5,
                PresetMin = 3,
                PresetMax = 5,
                BarterMin = 15,
                BarterMax = 35,
                CratesMin = 1,
                CratesMax = 2,
            };
            Medical = new AirdropContents()
            {
                ArmorMin = 0,
                ArmorMax = 0,
                PresetMin = 0,
                PresetMax = 0,
                BarterMin = 25,
                BarterMax = 45,
                CratesMin = 0,
                CratesMax = 0,
            };
            Barter = new AirdropContents()
            {
                ArmorMin = 0,
                ArmorMax = 0,
                PresetMin = 0,
                PresetMax = 0,
                BarterMin = 20,
                BarterMax = 35,
                CratesMin = 0,
                CratesMax = 0,
            };
            Weapon = new AirdropContents()
            {
                ArmorMin = 3,
                ArmorMax = 6,
                PresetMin = 6,
                PresetMax = 8,
                BarterMin = 11,
                BarterMax = 22,
                CratesMin = 0,
                CratesMax = 2,
            };

        }
    }
}
