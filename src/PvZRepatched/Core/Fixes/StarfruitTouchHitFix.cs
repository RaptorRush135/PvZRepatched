namespace PvZRepatched.Fixes;

#pragma warning disable SA1313 // Parameter names should begin with lower-case letter

using HarmonyLib;

using Il2CppReloaded.Gameplay;

[HarmonyPatch]
internal static class StarfruitTouchHitFix
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Projectile), nameof(Projectile.FindCollisionTarget))]
    private static void Postfix(Projectile __instance, ref Zombie? __result)
    {
        if (__result == null || __instance.mProjectileType != ProjectileType.Star)
        {
            return;
        }

        var projectileRect = __instance.GetProjectileRect();
        var zombieRect = __result.GetZombieRect();

        // GetRectOverlap truncates float rects to int, so a gap under 1px returns 0.
        // Require at least 1px of overlap, matching the original's `> 0`.
        if (Common.GetRectOverlap(ref projectileRect, ref zombieRect) > 0)
        {
            return;
        }

        __result = null;
    }
}
