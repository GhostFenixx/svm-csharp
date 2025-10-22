using Greed.Models;
using SPTarkov.Server.Core.Models.Spt.Config;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services;

namespace ServerValueModifier.Sections
{
    internal class Bots(ISptLogger<SVM> logger, ConfigServer configServer, DatabaseService databaseService, MainClass.MainConfig svmconfig)
    {
        public void BotsSection()
        {
            var locs = databaseService.GetLocations();
            BotConfig bots = configServer.GetConfig<BotConfig>();
            bots.WeeklyBoss.Enabled = !svmconfig.Bots.AIChance.DisableWeeklyBoss;
            //Double cycle to go through every location and every boss wave,
            //using switch to sort through boss names to adjust their spawn chances accordingly
            foreach (var loc in locs.GetDictionary().Values)
            {
                foreach (var chances in loc.Base.BossLocationSpawn)
                {
                    switch (chances.BossName)
                    {
                        case "bossBoar":
                            chances.BossChance = svmconfig.Bots.AIChance.Kaban;
                            break;
                        case "bossKolontay":
                            if (loc.Base.Id == "Sandbox_high")
                            {
                                chances.BossChance = svmconfig.Bots.AIChance.KolontayGZ;
                            }
                            if (loc.Base.Id == "TarkovStreets")
                            {
                                chances.BossChance = svmconfig.Bots.AIChance.KolontayStreets;
                            }
                            break;
                        case "bossPartisan":
                            if (loc.Base.Id == "bigmap")
                            {
                                chances.BossChance = svmconfig.Bots.AIChance.PartisanCustoms;
                                if (svmconfig.Bots.AIChance.ForcePartisan) //Partisan has some quirky triggers i can't really work with,
                                                                           //therefore - removing it for boss to spawn just like any other - via chance
                                                                           //Could be nice to rework it to check it once, maybe method? TODO
                                {
                                    chances.TriggerId = "";
                                    chances.TriggerName = "";
                                }
                            }
                            if (loc.Base.Id == "Shoreline")
                            {
                                chances.BossChance = svmconfig.Bots.AIChance.PartisanShoreline;
                                if (svmconfig.Bots.AIChance.ForcePartisan)
                                {
                                    chances.TriggerId = "";
                                    chances.TriggerName = "";
                                }
                            }
                            if (loc.Base.Id == "Lighthouse")
                            {
                                chances.BossChance = svmconfig.Bots.AIChance.PartisanLighthouse;
                                if (svmconfig.Bots.AIChance.ForcePartisan)
                                {
                                    chances.TriggerId = "";
                                    chances.TriggerName = "";
                                }
                            }
                            if (loc.Base.Id == "Woods")
                            {
                                chances.BossChance = svmconfig.Bots.AIChance.PartisanWoods;
                                if (svmconfig.Bots.AIChance.ForcePartisan)
                                {
                                    chances.TriggerId = "";
                                    chances.TriggerName = "";
                                }
                            }
                            break;
                        case "bossBully":
                            chances.BossChance = svmconfig.Bots.AIChance.Reshala;
                            break;
                        case "bossSanitar":
                            chances.BossChance = svmconfig.Bots.AIChance.Sanitar;
                            break;
                        case "bossKilla":
                            chances.BossChance = svmconfig.Bots.AIChance.Killa;
                            break;
                        case "bossTagilla":
                            if (loc.Base.Id == "factory4_night")
                            {
                                chances.BossChance = svmconfig.Bots.AIChance.TagillaNight;
                            }
                            if (loc.Base.Name == "factory4_day")
                            {
                                chances.BossChance = svmconfig.Bots.AIChance.Tagilla;
                            }
                            break;
                        case "bossGluhar":
                            chances.BossChance = svmconfig.Bots.AIChance.Glukhar;
                            break;
                        case "bossKojaniy":
                            chances.BossChance = svmconfig.Bots.AIChance.Shturman;
                            break;
                        case "bossZryachiy":
                            chances.BossChance = svmconfig.Bots.AIChance.Zryachiy;
                            break;
                        case "exUsec":
                            chances.BossChance = svmconfig.Bots.AIChance.Rogue;
                            break;
                        case "bossKnight":
                            if (loc.Base.Id == "bigmap")
                            {
                                chances.BossChance = svmconfig.Bots.AIChance.Trio;
                            }
                            if (loc.Base.Id == "Shoreline")
                            {
                                chances.BossChance = svmconfig.Bots.AIChance.TrioShoreline;
                            }
                            if (loc.Base.Id == "Lighthouse")
                            {
                                chances.BossChance = svmconfig.Bots.AIChance.TrioLighthouse;
                            }
                            if (loc.Base.Id == "Woods")
                            {
                                chances.BossChance = svmconfig.Bots.AIChance.TrioWoods;
                            }
                            break;
                        case "pmcBot":
                            if (loc.Base.Id == "laboratory")
                            {
                                chances.BossChance = svmconfig.Bots.AIChance.RaiderLab;
                            }
                            if (loc.Base.Id == "RezervBase")
                            {
                                chances.BossChance = svmconfig.Bots.AIChance.RaiderReserve;
                            }
                            break;
                        case "sectantPriest":
                            if (loc.Base.Id == "factory4_night")
                            {
                                chances.BossChance = svmconfig.Bots.AIChance.CultistFactory;
                            }
                            if (loc.Base.Id == "Woods")
                            {
                                chances.BossChance = svmconfig.Bots.AIChance.CultistWoods;
                            }
                            if (loc.Base.Id == "bigmap")
                            {
                                chances.BossChance = svmconfig.Bots.AIChance.CultistCustoms;
                            }
                            if (loc.Base.Id == "Shoreline")
                            {
                                chances.BossChance = svmconfig.Bots.AIChance.CultistShoreline;
                            }
                            if (loc.Base.Id == "Sandbox")
                            {
                                chances.BossChance = svmconfig.Bots.AIChance.CultistGroundZero;
                            }
                            break;

                    }
                }
            }
            //bots.Durability.BotDurabilities
            //bots.Durability.BotDurabilities["assault"].Weapon
            //Separated in a different property.
            bots.Durability.Pmc.Armor.MinDelta = 100 - svmconfig.Bots.PMC.ArmorMin;
            bots.Durability.Pmc.Armor.MaxDelta = 100 - svmconfig.Bots.PMC.ArmorMax;
            bots.Durability.Pmc.Weapon.LowestMax = svmconfig.Bots.PMC.WeaponMin;
            bots.Durability.Pmc.Weapon.HighestMax = svmconfig.Bots.PMC.WeaponMax;
            foreach (var bottype in bots.Durability.BotDurabilities.Keys)
            {
                switch (bottype)
                {
                    case "assault" or "crazyassaultevent" or "cursedassault":
                        AdjustDurab(bots,bottype, svmconfig.Bots.SCAV);
                        break;
                    case "boss" or "sectantpriest":
                        AdjustDurab(bots, bottype, svmconfig.Bots.Boss);
                        break;
                    case "pmcbot":
                        AdjustDurab(bots, bottype, svmconfig.Bots.Raider);
                        break;
                    case "follower":
                        AdjustDurab(bots, bottype, svmconfig.Bots.Follower);
                        break;
                    case "exusec":
                        AdjustDurab(bots, bottype, svmconfig.Bots.Rogue);
                        break;
                    case "marksman":
                        AdjustDurab(bots, bottype, svmconfig.Bots.Marksman);
                        break;
                }
            }

        }
        public void AdjustDurab(BotConfig bots, string bottype, Greed.Models.AI.BotDurability type)
        {
            bots.Durability.BotDurabilities[bottype].Weapon.HighestMax = type.WeaponMax;
            bots.Durability.BotDurabilities[bottype].Weapon.LowestMax = type.WeaponMin;
            bots.Durability.BotDurabilities[bottype].Armor.MinDelta = 100 - type.ArmorMin;
            bots.Durability.BotDurabilities[bottype].Armor.MaxDelta = 100 - type.ArmorMax;
        }

    }
}
