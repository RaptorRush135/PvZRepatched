namespace PvZRepatched.Fixes;

#pragma warning disable SA1313 // Parameter names should begin with lower-case letter

using HarmonyLib;

using Il2CppReloaded.Gameplay;

[HarmonyPatch(typeof(Projectile), nameof(Projectile.Update))]
internal static class DeadProjectileFix
{
    [HarmonyPrefix]
    private static bool Prefix(Projectile __instance) => !__instance.mDead;
}
