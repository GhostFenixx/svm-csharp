using Greed.Models;
using SPTarkov.DI.Annotations;
using SPTarkov.Reflection.Patching;
using SPTarkov.Server.Core.Callbacks;
using SPTarkov.Server.Core.Controllers;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers.Server;
using System.Reflection;

namespace ServerValueModifier.HarmonyOverrides;

[Injectable]
public class HealthAsyncPatch : AbstractPatch
{
    private static ModHelper _modHelper;

    public HealthAsyncPatch(ModHelper modHelper)
        { 
        _modHelper = modHelper;
    }
    protected override MethodBase GetTargetMethod()
    {
        return typeof(GameController).GetMethod("UpdateProfileHealthValues", BindingFlags.Instance | BindingFlags.NonPublic);
    }
    [PatchPrefix]
    public static bool Prefix()
    {
        try//try-catch in case no config
        {
            MainClass.MainConfig cf = new SVMConfig(_modHelper).CallConfig();
            if (cf.Hideout.Regeneration.OfflineRegen)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        catch
        {
            return true;
        }
    }
}


