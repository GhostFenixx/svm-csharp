namespace Greed.Models.CaseSpaceManager
{
    public class Cases
    {
        public Case GKeychain { get; set; }
        public Case KeycardHolderCase { get; set; }
        public Case InjectorCase { get; set; }
        public Case Holodilnick { get; set; }
        public Case PistolCase { get; set; }
        public Case DocumentsCase { get; set; }
        public Case Keytool { get; set; }
        public Case SiccCase { get; set; }
        public Case ThiccWeaponCase { get; set; }
        public Case ThiccItemsCase { get; set; }
        public Case MedicineCase { get; set; }
        public Case DogtagCase { get; set; }
        public Case MagazineCase { get; set; }
        public Case AmmunitionCase { get; set; }
        public Case WeaponCase { get; set; }
        public Case ItemsCase { get; set; }
        public Case GrenadeCase { get; set; }
        public Case WZWallet { get; set; }
        public Case SimpleWallet { get; set; }
        public Case MoneyCase { get; set; }
        public Case LuckyScav { get; set; }
        public Case StreamerCase { get; set; }
        public Case ArmorPlateCase { get; set; }
        public Case KeysCase { get; set; }
        public Cases()
        {
            ArmorPlateCase = new Case()
            {
                Height = 12,
                Width = 8,
                Filter = false
            };
            StreamerCase = new Case()
            {
                Height = 11,
                Width = 7,
                Filter = false
            };
            GKeychain = new Case()
            {
                Height = 2,
                Width = 2,
                Filter = false
            };
            KeycardHolderCase = new Case()
            {
                Height = 3,
                Width = 3,
                Filter = false
            };
            InjectorCase = new Case()
            {
                Height = 3,
                Width = 3,
                Filter = false
            };
            Holodilnick = new Case()
            {
                Height = 8,
                Width = 8,
                Filter = false
            };
            PistolCase = new Case()
            {
                Height = 3,
                Width = 4,
                Filter = false
            };
            DocumentsCase = new Case()
            {
                Height = 4,
                Width = 4,
                Filter = false
            };
            Keytool = new Case()
            {
                Height = 4,
                Width = 4,
                Filter = false
            };
            SiccCase = new Case()
            {
                Height = 5,
                Width = 5,
                Filter = false
            };
            ThiccWeaponCase = new Case()
            {
                Height = 15,
                Width = 6,
                Filter = false
            };
            ThiccItemsCase = new Case()
            {
                Height = 14,
                Width = 14,
                Filter = false
            };
            MedicineCase = new Case()
            {
                Height = 7,
                Width = 7,
                Filter = false
            };
            DogtagCase = new Case()
            {
                Height = 10,
                Width = 10,
                Filter = false
            };
            MagazineCase = new Case()
            {
                Height = 7,
                Width = 7,
                Filter = false
            };
            AmmunitionCase = new Case()
            {
                Height = 7,
                Width = 7,
                Filter = false
            };
            WeaponCase = new Case()
            {
                Height = 10,
                Width = 5,
                Filter = false
            };
            ItemsCase = new Case()
            {
                Height = 8,
                Width = 8,
                Filter = false
            };
            GrenadeCase = new Case()
            {
                Height = 8,
                Width = 8,
                Filter = false
            };
            WZWallet = new Case()
            {
                Height = 2,
                Width = 2,
                Filter = false
            };
            SimpleWallet = new Case()
            {
                Height = 2,
                Width = 2,
                Filter = false
            };
            MoneyCase = new Case()
            {
                Height = 7,
                Width = 7,
                Filter = false
            };
            LuckyScav = new Case()
            {
                Height = 14,
                Width = 14,
                Filter = false
            };
            KeysCase = new Case()
            {
                Height = 7,
                Width = 11,
                Filter = false
            };
        }
    }
}
