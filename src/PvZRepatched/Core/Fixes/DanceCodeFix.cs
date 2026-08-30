namespace PvZRepatched.Fixes;

using HarmonyLib;

using Il2CppReloaded;
using Il2CppReloaded.Binders;
using Il2CppReloaded.DataModels;
using Il2CppReloaded.Services;

using Il2CppTekly.DataModels.Binders;
using Il2CppTekly.DataModels.Models;
using Il2CppTekly.Localizations;

using MelonLoader;

using UnityEngine;
using UnityEngine.UI;

[HarmonyPatch(typeof(UserService), nameof(UserService.GetDanceModeAvailable))]
internal static class DanceCodeFix
{
    public static void Apply(string sceneName)
    {
        if (sceneName != Constants.Transition.GAMEPLAY)
        {
            return;
        }

        const string danceModeKey = "settings.isDanceMode";
        if (!RootModel.Instance.TryGetModel(ModelKey.Parse(danceModeKey), 0, out ToggleModel danceModeToggleModel))
        {
            Melon<Core>.Logger.Warning("Could not find dance mode toggle model");
            return;
        }

        Transform effectOptionsTransform = GameObject.Find("Panels")
            .transform.Find("P_GamePlay_OptionsPanel/P_OptionsPanel_Canvas/Layout/Center/Panel/Top/EffectOptions");

        effectOptionsTransform.GetComponent<VerticalLayoutGroup>().spacing = 30;

        Transform templateToggleTransform = effectOptionsTransform.Find("daisies");

        var danceToggle = Object.Instantiate(
            templateToggleTransform.gameObject,
            effectOptionsTransform,
            worldPositionStays: false);
        danceToggle.name = "dance";
        danceToggle.transform.SetSiblingIndex(templateToggleTransform.GetSiblingIndex());

        var textLocalizer = danceToggle.GetComponentInChildren<TextLocalizer>();
        textLocalizer.Id = "$GAMEPLAY_TOGGLE_DANCE";

        var toggleBinder = danceToggle.GetComponentInChildren<UnityToggleBinder>();
        toggleBinder.name = "dance_CheckBox";
        toggleBinder.m_toggleModel = danceModeToggleModel;
        toggleBinder.m_key = ModelRef.Create(danceModeKey);

        var danceVisibilityBinder = danceToggle.GetComponent<VisibilityBinder>();
        danceVisibilityBinder.m_key = ModelRef.Create("platform.isDanceModeAvailable");

        var binderContainer = effectOptionsTransform.GetComponent<BinderContainer>();

        binderContainer.Binders.Add(danceVisibilityBinder);
    }
}
