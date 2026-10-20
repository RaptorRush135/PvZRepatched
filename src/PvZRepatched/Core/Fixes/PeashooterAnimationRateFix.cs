namespace PvZRepatched.Fixes;

#pragma warning disable SA1313 // Parameter names should begin with lower-case letter

using HarmonyLib;

using Il2CppReloaded.Gameplay;

using Il2CppSource.Controllers;

using MelonLoader;

[HarmonyPatch(typeof(Plant), nameof(Plant.PlantInitialize))]
internal static class PeashooterAnimationRateFix
{
    private static bool IsSinglePea(SeedType type) =>
        type is SeedType.Peashooter
            or SeedType.Snowpea
            or SeedType.Repeater
            or SeedType.Leftpeater
            or SeedType.Gatlingpea;

    [HarmonyPostfix]
    private static void Postfix(Plant __instance, SeedType theSeedType)
    {
        if (!IsSinglePea(theSeedType))
        {
            return;
        }

        var controller = __instance.mController;
        if (controller == null)
        {
            Melon<Core>.Logger.Warning($"PlantInitialize: unexpected null controller for '{theSeedType}'");
            return;
        }

        float rate = Common.RandRangeFloat(15f, 20f);
        controller.SetAnimationRateOnTrack(rate, CharacterAnimationTrack.Body);
        controller.PlayAnimationOnTrack("head_idle", CharacterAnimationTrack.Head, rate);
    }
}
