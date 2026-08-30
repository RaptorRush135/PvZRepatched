namespace PvZRepatched.Fixes;

#pragma warning disable SA1313 // Parameter names should begin with lower-case letter

using HarmonyLib;

using Il2CppReloaded.Gameplay;
using Il2CppReloaded.TreeStateActivities;

// Fix extra spike height caused by a leftover Windows Phone adjustment
[HarmonyPatch(typeof(Plant), nameof(Plant.PlantDrawHeightOffset))]
internal static class SpikeHeightFix
{
    [HarmonyPostfix]
    private static void Postfix(
        ref float __result,
        GameplayActivity app,
        Board? theBoard,
        SeedType theSeedType,
        int theCol,
        int theRow)
    {
        if (theSeedType is not (SeedType.Spikeweed or SeedType.Spikerock)
            || theBoard == null)
        {
            return;
        }

        Plant? pot = theBoard.GetFlowerPotAt(theCol, theRow);
        if (pot != null && app.GameMode != GameMode.ChallengeZenGarden)
        {
            return;
        }

        if (theBoard.StageHasRoof() || theBoard.IsPoolSquare(theCol, theRow))
        {
            return;
        }

        bool has6Rows = theBoard.StageHas6Rows();
        int lastRow = has6Rows ? 5 : 4;
        if (theRow != lastRow || theCol < 7)
        {
            return;
        }

        const int expectedOffset = 12;
        int actualOffset = has6Rows ? 1 : 15;
        __result += expectedOffset - actualOffset;
    }
}
