namespace Greed.Models.AI
{
    public class Bots
    {
        public AIChance AIChance { get; set; }
        public BotDurability PMC { get; set; }
        public BotDurability SCAV { get; set; }
        public BotDurability Boss { get; set; }
        public BotDurability Follower { get; set; }
        public BotDurability Rogue { get; set; }
        public BotDurability Raider { get; set; }
        public BotDurability Marksman { get; set; }
        public bool EnableBots { get; set; }
        public Bots()
        {
            AIChance = new AIChance();
            PMC = new()
            {
                ArmorMin = 90,
                ArmorMax = 100,
                WeaponMin = 95,
                WeaponMax = 100,
            };
            SCAV = new()
            {
                ArmorMin = 0,
                ArmorMax = 50,
                WeaponMin = 85,
                WeaponMax = 100,
            };
            Boss = new()
            {
                ArmorMin = 85,
                ArmorMax = 100,
                WeaponMin = 50,
                WeaponMax = 100,
            };
            Follower = new()
            {
                ArmorMin = 90,
                ArmorMax = 100,
                WeaponMin = 85,
                WeaponMax = 100,
            };
            Rogue = new()
            {
                ArmorMin = 90,
                ArmorMax = 100,
                WeaponMin = 80,
                WeaponMax = 100,
            };
            Raider = new()
            {
                ArmorMin = 90,
                ArmorMax = 100,
                WeaponMin = 80,
                WeaponMax = 100,
            };
            Marksman = new()
            {
                ArmorMin = 90,
                ArmorMax = 100,
                WeaponMin = 60,
                WeaponMax = 100,
            };
        }
    }
}
