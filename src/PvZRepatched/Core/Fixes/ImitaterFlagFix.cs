namespace PvZRepatched.Fixes;

#pragma warning disable SA1313 // Parameter names should begin with lower-case letter

using HarmonyLib;

using Il2CppReloaded.Gameplay;

[HarmonyPatch(typeof(Plant), nameof(Plant.PlantInitialize))]
internal static class ImitaterFlagFix
{
    [HarmonyPrefix]
    private static void Prefix(Plant __instance)
        => __instance.mIsImitation = false;
}
