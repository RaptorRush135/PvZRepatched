namespace PvZRepatched.Fixes;

#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
#pragma warning disable IDE0051 // Remove unused private member

using HarmonyLib;

using Il2CppReloaded.DataModels;
using Il2CppReloaded.Gameplay;

using PvZRepatched.Extensions;

// Prevent seed packets costing more than 75 sun from being incorrectly greyed out during Versus plant selection
[HarmonyPatch(typeof(SeedBankDataModel), nameof(SeedBankDataModel.IsChoosing), MethodType.Getter)]
internal static class VersusSeedPacketsFix
{
    [HarmonyPrefix]
    private static bool Prefix(SeedBankDataModel __instance, ref bool __result)
    {
        if (__instance.m_gameplayActivity.Ref() is
            {
                ReloadedGameMode: ReloadedGameMode.Versus,
                VersusMode.Phase: VersusPhase.ChoosePlantPacket or VersusPhase.ChooseZombiePacket
            })
        {
            __result = true;
            return false;
        }

        return true;
    }
}
