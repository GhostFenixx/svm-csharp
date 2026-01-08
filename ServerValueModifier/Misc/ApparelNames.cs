using SPTarkov.Server.Core.Models.Common;

namespace ServerValueModifier;

public class ApparelNames
{
    public class LocaleEntry
    {
        public required string Name { get; set; }
        public required string ShortName { get; set; }
        public required string Description { get; set; }
    }

    public static Dictionary<MongoId, LocaleEntry> LOCALES = new()
    {
        // Uppers
        { "66a259bc0080c7eb925db101", new LocaleEntry { Name = "Cultist jacket", ShortName = "SVM", Description = "SVM's Unheard Clothing" } },
        { "66a259bc0080c7eb925db128", new LocaleEntry { Name = "Mutkevich t-shirt (Black)", ShortName = "SVM", Description = "SVM's BattlePass Clothing" } },
        { "66a259bc0080c7eb925db131", new LocaleEntry { Name = "Mutkevich t-shirt (White)", ShortName = "SVM", Description = "SVM's BattlePass Clothing" } },
        { "66a259bc0080c7eb925db134", new LocaleEntry { Name = "SBEU Mosquito t-shirt (Coyote)", ShortName = "SVM", Description = "SVM's BattlePass Clothing" } },
        { "66a259bc0080c7eb925db137", new LocaleEntry { Name = "SBEU Mosquito t-shirt (Olive)", ShortName = "SVM", Description = "SVM's BattlePass Clothing" } },
        { "66a259bc0080c7eb925db140", new LocaleEntry { Name = "Fucker & Motherfucker t-shirt", ShortName = "SVM", Description = "SVM's BattlePass Clothing" } },
        { "66a259bc0080c7eb925db143", new LocaleEntry { Name = "Saiga t-shirt", ShortName = "SVM", Description = "SVM's BattlePass Clothing" } },
        { "66a259bc0080c7eb925db146", new LocaleEntry { Name = "Knives Only t-shirt", ShortName = "SVM", Description = "SVM's BattlePass Clothing" } },
        { "66a259bc0080c7eb925db113", new LocaleEntry { Name = "Ragged vest (Blue)", ShortName = "SVM", Description = "SVM's Scav Clothing" } },
        { "66a259bc0080c7eb925db125", new LocaleEntry { Name = "Old parka (Beige)", ShortName = "SVM", Description = "SVM's Scav Clothing" } },
        { "66a259bc0080c7eb925db119", new LocaleEntry { Name = "Leather jacket", ShortName = "SVM", Description = "SVM's Scav Clothing" } },
        { "66a259bc0080c7eb925db116", new LocaleEntry { Name = "Motorcycle jacket (White)", ShortName = "SVM", Description = "SVM's Scav Clothing" } },
        { "66a259bc0080c7eb925db122", new LocaleEntry { Name = "Hunting jacket", ShortName = "SVM", Description = "SVM's Scav Clothing" } },
        { "66a259bc0080c7eb925db104", new LocaleEntry { Name = "Russia jacket", ShortName = "SVM", Description = "SVM's Scav Clothing" } },
        { "66a259bc0080c7eb925db107", new LocaleEntry { Name = "UnderTarmor jacket", ShortName = "SVM", Description = "SVM's Scav Clothing" } },
        { "66a259bc0080c7eb925db110", new LocaleEntry { Name = "Old parka (Yellow)", ShortName = "SVM", Description = "SVM's Scav Clothing" } },
        // Lowers
        { "66a259bc0080c7eb925dc185", new LocaleEntry { Name = "BEAR Paladin (Coyote)", ShortName = "SVM", Description = "SVM's BattlePass Clothing" } },
        { "66a259bc0080c7eb925dc188", new LocaleEntry { Name = "BEAR Paladin (Ranger Green)", ShortName = "SVM", Description = "SVM's BattlePass Clothing" } },
        { "66a259bc0080c7eb925dc191", new LocaleEntry { Name = "BEAR Paladin (Tactical Olive)", ShortName = "SVM", Description = "SVM's BattlePass Clothing" } },
        { "66a259bc0080c7eb925dc176", new LocaleEntry { Name = "USEC K4 (Dark Olive)", ShortName = "SVM", Description = "SVM's BattlePass Clothing" } },
        { "66a259bc0080c7eb925dc179", new LocaleEntry { Name = "USEC K4 (Green)", ShortName = "SVM", Description = "SVM's BattlePass Clothing" } },
        { "66a259bc0080c7eb925dc182", new LocaleEntry { Name = "USEC K4 (Timber Brown)", ShortName = "SVM", Description = "SVM's BattlePass Clothing" } },
        { "66a259bc0080c7eb925dc170", new LocaleEntry { Name = "Faded jeans", ShortName = "SVM", Description = "SVM's Scav Clothing" } },
        { "66a259bc0080c7eb925dc173", new LocaleEntry { Name = "Cargo pants (Olive)", ShortName = "SVM", Description = "SVM's Scav Clothing" } },
        { "66a259bc0080c7eb925dc149", new LocaleEntry { Name = "Jogging pants (Dark Blue)", ShortName = "SVM", Description = "SVM's Scav Clothing" } },
        { "66a259bc0080c7eb925dc167", new LocaleEntry { Name = "Cargo pants (Black)", ShortName = "SVM", Description = "SVM's Scav Clothing" } },
        { "66a259bc0080c7eb925dc161", new LocaleEntry { Name = "Skinny jeans (Black)", ShortName = "SVM", Description = "SVM's Scav Clothing" } },
        { "66a259bc0080c7eb925dc158", new LocaleEntry { Name = "Sweatpants (Gray)", ShortName = "SVM", Description = "SVM's Scav Clothing" } },
        { "66a259bc0080c7eb925dc155", new LocaleEntry { Name = "Jeans with knee-pads", ShortName = "SVM", Description = "SVM's Scav Clothing" } },
        { "66a259bc0080c7eb925dc164", new LocaleEntry { Name = "Hunting pants", ShortName = "SVM", Description = "SVM's Scav Clothing" } },
        { "66a259bc0080c7eb925dc152", new LocaleEntry { Name = "Cargo pants (Flecktarn)", ShortName = "SVM", Description = "SVM's Scav Clothing" } },
    };
}