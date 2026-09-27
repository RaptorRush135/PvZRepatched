namespace PvZRepatched.Features;

using HarmonyLib;

using Il2CppSource.TreeStateActivities;

using UnityEngine;

[HarmonyPatch]
internal static class RunInBackground
{
    private static bool runInBackground;

    public static void Set(bool value)
    {
        Application.runInBackground = value;
        runInBackground = value;
    }

    [HarmonyPatch(
        typeof(TransitionWhenFocusLostActivity),
        nameof(TransitionWhenFocusLostActivity.OnPlatformFocusChanged))]
    [HarmonyPrefix]
    private static bool OnPlatformFocusChangedPrefix() => !runInBackground;
}
