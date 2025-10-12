using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Controllers;
using SPTarkov.Server.Core.DI;
using SPTarkov.Reflection.Patching;
using System.Reflection;

namespace ServerValueModifier.HarmonyOverrides;

[Injectable(TypePriority = OnLoadOrder.PreSptModLoader + 2)]
public class StartAsyncPatch : AbstractPatch
{
    protected override MethodBase GetTargetMethod()
    {
        return typeof(GameController).GetMethod("UpdateProfileHealthValues");
    }
    [PatchPrefix]
    public static bool Prefix()
    {
        return false;
    }
}


