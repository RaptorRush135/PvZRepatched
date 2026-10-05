namespace PvZRepatched.Fixes;

#pragma warning disable SA1313 // Parameter names should begin with lower-case letter

using System.Diagnostics.CodeAnalysis;

using HarmonyLib;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[HarmonyPatch]
public static class ButtonPressFix
{
    private static readonly HashSet<string> TargetButtons =
        ["Shovel", "TreeFood", "P_Zen_ActionButtonTemplate(Clone)"];

    private static bool firing;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Selectable), nameof(Selectable.OnPointerDown))]
    private static void OnPointerDownPostfix(Selectable __instance, PointerEventData eventData)
    {
        if (!IsTarget(__instance, out var button))
        {
            return;
        }

        firing = true;
        try
        {
            button.OnPointerClick(eventData);
        }
        finally
        {
            firing = false;
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Button), nameof(Button.OnPointerClick))]
    private static bool OnPointerClickPrefix(Button __instance)
    {
        return firing || !IsTarget(__instance);
    }

    private static bool IsTarget(Component component)
        => TargetButtons.Contains(component.gameObject.name);

    private static bool IsTarget(
        Selectable selectable,
        [MaybeNullWhen(false)] out Button button)
    {
        if (!IsTarget(selectable))
        {
            button = null;
            return false;
        }

        button = selectable.TryCast<Button>();
        return button != null;
    }
}
