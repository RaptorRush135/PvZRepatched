namespace PvZRepatched.Fixes;

#pragma warning disable SA1313 // Parameter names should begin with lower-case letter

using HarmonyLib;

using Il2CppReloaded.Characters;
using Il2CppReloaded.Gameplay;

/*
 * The original game only used two pumpkin damage stages:
 *  - DAMAGE1: health < 2/3 of max (~66.66%)
 *  - DAMAGE2: unused
 *  - DAMAGE3: health < 1/3 of max (~33.33%)
 *
 * Replanted enables DAMAGE2 and redistributes the stages in quarters, which moved the first stage up to 75%:
 *  - DAMAGE1: health < 3/4 of max (75%)
 *  - DAMAGE2: health < 2/4 of max (50%)
 *  - DAMAGE3: health < 1/4 of max (25%)
 *
 * Board.CanPlantAt only allows First Aid repair below 2/3 of max (~66.66%),
 * so a pumpkin between ~66.6% and 75% looked damaged but could not be repaired.
*/
[HarmonyPatch]
internal static class PumpkinStageFix
{
    private const string Slot = "Pumpkin_front";

    private const string DamagePrefix = "IMAGE_REANIM_PUMPKIN_DAMAGE";

    private static bool inPumpkinAnimate;

    [HarmonyPatch(typeof(Plant), nameof(Plant.Animate))]
    internal static class AnimatePatch
    {
        [HarmonyPrefix]
        private static void Prefix(Plant __instance)
            => inPumpkinAnimate = __instance.mSeedType == SeedType.Pumpkinshell;

        [HarmonyPostfix]
        private static void Postfix(Plant __instance)
        {
            if (!inPumpkinAnimate || __instance.mSquished)
            {
                return;
            }

            inPumpkinAnimate = false;

            var controller = __instance.mController;

            string? image = GetPumpkinDamageImage(__instance.mPlantHealth, __instance.mPlantMaxHealth);

            if (controller.GetImageOverride(Slot) != image)
            {
                controller.SetImageOverride(Slot, image);
            }

            static string? GetPumpkinDamageImage(int health, int maxHealth)
            {
                // < ~22.22%
                if (health < maxHealth * 2 / 9)
                {
                    return $"{DamagePrefix}3";
                }

                // < ~44.44%
                if (health < maxHealth * 4 / 9)
                {
                    return $"{DamagePrefix}2";
                }

                // < ~66.66%, same as the First Aid repair check in CanPlantAt
                if (health < maxHealth * 6 / 9)
                {
                    return $"{DamagePrefix}1";
                }

                return null;
            }
        }

        [HarmonyFinalizer]
        private static Exception? Finalizer(Exception? __exception)
        {
            inPumpkinAnimate = false;
            return __exception;
        }
    }

    [HarmonyPatch(
        typeof(CharacterSkinController),
        nameof(CharacterSkinController.SetImageOverride))]
    private static class SkipImageOverridePatch
    {
        [HarmonyPrefix]
        private static bool Prefix(string slotTrack)
            => !(inPumpkinAnimate && slotTrack == Slot);
    }
}
