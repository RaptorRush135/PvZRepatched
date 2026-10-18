namespace PvZRepatched.Fixes;

#pragma warning disable SA1313 // Parameter names should begin with lower-case letter

using HarmonyLib;

using Il2CppReloaded.Gameplay;

using Il2CppSource.Controllers;

/*
 * The Splitpea branch restarts the front head's idle animation in the wrong order:
 * SetAnimationTimeOnTrack(bodyTime, Head);         // 1. Sync time
 * PlayAnimationOnTrack(head_idle, Head, bodyRate); // 2. Restart animation
*/
[HarmonyPatch]
internal static class SplitpeaHeadFix
{
    private static bool inSplitpeaUpdateShooter;

    [HarmonyPatch(typeof(Plant), nameof(Plant.UpdateShooter))]
    internal static class UpdateShooterPatch
    {
        [HarmonyPrefix]
        private static void Prefix(Plant __instance)
        {
            inSplitpeaUpdateShooter = __instance.mSeedType == SeedType.Splitpea
                && __instance.mLaunchCounter <= 1;
        }

        [HarmonyFinalizer]
        private static void Finalizer()
        {
            inSplitpeaUpdateShooter = false;
        }
    }

    [HarmonyPatch(
        typeof(ReloadedCharacterController),
        nameof(ReloadedCharacterController.PlayAnimationOnTrack),
        typeof(string),
        typeof(CharacterAnimationTrack),
        typeof(float))]
    internal static class PlayAnimationPatch
    {
        [HarmonyPostfix]
        private static void Postfix(
            ReloadedCharacterController __instance,
            string animationName,
            CharacterAnimationTrack track)
        {
            if (!inSplitpeaUpdateShooter
                || track != CharacterAnimationTrack.Head
                || animationName != "head_idle")
            {
                return;
            }

            __instance.SetAnimationTimeOnTrack(
                __instance.GetAnimationTimeOnTrack(CharacterAnimationTrack.Body),
                CharacterAnimationTrack.Head);
        }
    }
}
