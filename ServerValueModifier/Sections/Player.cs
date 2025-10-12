using Greed.Models;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Spt.Config;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services;

namespace ServerValueModifier.Sections
{
    internal class Player(ISptLogger<SVM> logger, ConfigServer configServer, DatabaseService databaseService, MainClass.MainConfig Config)
    {
        public void PlayerSection()
        {
            Globals globals = databaseService.GetGlobals();
            HealthConfig healthconfig = configServer.GetConfig<HealthConfig>();
            globals.Configuration.SkillsSettings.SkillProgressRate = Config.Player.SkillProgMult;
            globals.Configuration.SkillsSettings.WeaponSkillProgressRate = Config.Player.WeaponSkillMult;
           // healthconfig.HealthMultipliers.Death = Config.Player.DiedHealth.Health_death; TODO: REMOVED
            healthconfig.HealthMultipliers.Blacked = Config.Player.DiedHealth.Health_blacked;
            healthconfig.Save.Health = Config.Player.DiedHealth.Savehealth;
            //healthconfig.Save.Effects = Config.Player.DiedHealth.Saveeffects; TODO: REMOVED

            if (Config.Player.EnableFatigue)
            {
                globals.Configuration.SkillMinEffectiveness = Config.Player.Skills.SkillMinEffect;
                globals.Configuration.SkillFatiguePerPoint = Config.Player.Skills.SkillFatiguePerPoint;
                globals.Configuration.SkillFreshEffectiveness = Config.Player.Skills.SkillFreshEffect;
                globals.Configuration.SkillFreshPoints = Config.Player.Skills.SkillFPoints;
                globals.Configuration.SkillPointsBeforeFatigue = Config.Player.Skills.SkillPointsBeforeFatigue;
                globals.Configuration.SkillFatigueReset = Config.Player.Skills.SkillFatigueReset;
            }
            //############## Player level XP box ############## 
            globals.Configuration.Exp.Kill.VictimBotLevelExperience = Config.Player.CharXP.ScavKill;
            globals.Configuration.Exp.Kill.VictimLevelExperience = Config.Player.CharXP.PMCKill;
            globals.Configuration.Exp.Kill.BotHeadShotMultiplier = Config.Player.CharXP.ScavHMult;
            globals.Configuration.Exp.Kill.PmcHeadShotMultiplier = Config.Player.CharXP.PMCHMult;
            //############## XP mults Box ############## 
            globals.Configuration.Exp.MatchEnd.RunnerMultiplier = Config.Player.RaidMult.Runner;
            globals.Configuration.Exp.MatchEnd.MiaMultiplier = Config.Player.RaidMult.MIA;
            globals.Configuration.Exp.MatchEnd.SurvivedMultiplier = Config.Player.RaidMult.Survived;
            globals.Configuration.Exp.MatchEnd.KilledMultiplier = Config.Player.RaidMult.Killed;
            //############## Health ############## 
            globals.Configuration.Health.Effects.Existence.HydrationLoopTime /= Config.Player.HydrationLoss;
            globals.Configuration.Health.Effects.Existence.EnergyLoopTime /= Config.Player.EnergyLoss;
            globals.Configuration.Health.Effects.Existence.DestroyedStomachEnergyTimeFactor = Config.Player.BlackStomach;
            globals.Configuration.Health.Effects.Existence.DestroyedStomachHydrationTimeFactor = Config.Player.BlackStomach;

            //############## Stamina ############## 
            if (Config.Player.EnableStaminaLegs)
            {
                globals.Configuration.Stamina.Capacity = Config.Player.MaxStaminaLegs;
                globals.Configuration.Stamina.BaseRestorationRate = Config.Player.RegenStaminaLegs;
                globals.Configuration.Stamina.JumpConsumption = Config.Player.JumpConsumption;
                globals.Configuration.Stamina.StandupConsumption.X = Config.Player.LayToStand;
                globals.Configuration.Stamina.PoseLevelConsumptionPerNotch.X = Config.Player.CrouchToStand / 10;
            }
            if (Config.Player.EnableStaminaHands)
            {
                globals.Configuration.Stamina.HandsCapacity = Config.Player.MaxStaminaHands;
                globals.Configuration.Stamina.HandsRestoration = Config.Player.RegenStaminaHands;
                globals.Configuration.Stamina.AimConsumptionByPose.X = Config.Player.LayingDown;
                globals.Configuration.Stamina.AimConsumptionByPose.Y = Config.Player.Crouching;
                globals.Configuration.Stamina.AimConsumptionByPose.Z = Config.Player.Standing;
            }
            if (Config.Player.UnlimitedStamina)
            {
                globals.Configuration.Stamina.Capacity = 500;
                globals.Configuration.Stamina.BaseRestorationRate = 500;
                globals.Configuration.Stamina.StaminaExhaustionCausesJiggle = false;
                globals.Configuration.Stamina.StaminaExhaustionStartsBreathSound = false;
                globals.Configuration.Stamina.StaminaExhaustionRocksCamera = false;
                globals.Configuration.Stamina.SprintDrainRate = 0;
                globals.Configuration.Stamina.JumpConsumption = 0;
                globals.Configuration.Stamina.AimDrainRate = 0;
                globals.Configuration.Stamina.SitToStandConsumption = 0;
            }
            if (Config.Player.FallDamage)
            {
                globals.Configuration.Health.Falling.SafeHeight = 200;
                globals.Configuration.Health.Falling.DamagePerMeter = 0;
            }

        }
    }
}
