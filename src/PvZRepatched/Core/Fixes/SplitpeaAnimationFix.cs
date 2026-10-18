namespace PvZRepatched.Fixes;

#pragma warning disable SA1313 // Parameter names should begin with lower-case letter

using HarmonyLib;

using Il2CppReloaded.Gameplay;

using Il2CppSource.Controllers;

[HarmonyPatch]
internal static class SplitpeaAnimationFix
{
    /*
     * The Splitpea branch restarts the front head's idle animation in the wrong order:
     * SetAnimationTimeOnTrack(bodyTime, Head);         // 1. Sync time
     * PlayAnimationOnTrack(head_idle, Head, bodyRate); // 2. Restart animation
     */
    [HarmonyPatch]
    internal static class FrontHeadFix
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

    /*
     * Split Pea's back head (Head2) can get stuck on the last frame of
     * splitpea_shooting instead of returning to splitpea_idle.
     *
     * FindTargetAndFire sets mShootingCounter to 26, but the replanted Spine animation takes more ticks to complete.
     * UpdateShooting only attempts to restart splitpea_idle when the counter reaches 0 and LoopCount > 0.
     * Since Spine increments LoopCount only on Complete, the check fails before the animation finishes,
     * and the counter's early return prevents further retries.
     *
     * When the counter reaches 0 and Head2 is still PlayOnceAndHold with LoopCount < 1, reset the counter to 1.
     * This retries the original restart logic each tick until Spine completes the animation.
     */
    [HarmonyPatch(typeof(Plant), nameof(Plant.UpdateShooting))]
    internal static class BackHeadFix
    {
        [HarmonyPrefix]
        private static void Prefix(Plant __instance, out int __state)
        {
            __state = __instance.mShootingCounter;
        }

        [HarmonyPostfix]
        private static void Postfix(Plant __instance, int __state)
        {
            if (__instance.mSeedType != SeedType.Splitpea)
            {
                return;
            }

            // Only the 1 -> 0 tick
            if (__state != 1 || __instance.mShootingCounter != 0)
            {
                return;
            }

            var controller = __instance.mController;

            // Back head still held on the shoot pose and complete has not fired yet, retry next tick
            if (controller.GetAnimLoopType(CharacterAnimationTrack.Head2) == ReanimLoopType.PlayOnceAndHold
                && controller.GetLoopCount(CharacterAnimationTrack.Head2) < 1)
            {
                __instance.mShootingCounter = 1;
            }
        }
    }
}
