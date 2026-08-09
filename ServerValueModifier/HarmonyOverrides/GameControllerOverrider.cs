using Greed.Models;
using SPTarkov.DI.Annotations;
using SPTarkov.Reflection.Patching;
using SPTarkov.Server.Core.Controllers;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers;
using System.Reflection;

namespace ServerValueModifier.HarmonyOverrides;

[Injectable(TypePriority = OnLoadOrder.PreSptModLoader + 2)]
public class StartAsyncPatch : AbstractPatch
{
    private static ModHelper _modHelper;
    public StartAsyncPatch(ModHelper modHelper)
    {
        _modHelper = modHelper;
    }
    protected override MethodBase GetTargetMethod()
    {
        return typeof(GameController).GetMethod("UpdateProfileHealthValues");
    }
    [PatchPrefix]
    public static bool Prefix()
    {
        try
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


