namespace PvZRepatched;

using MelonLoader;

using PvZRepatched.Features;
using PvZRepatched.Fixes;
using PvZRepatched.Logger;

internal sealed class Core : MelonMod
{
    public override void OnInitializeMelon()
    {
        // TODO: Refactor, read from config, BloomEngine
        UnityLogger.Install(UnityLogMode.Filtered, false);
        RunInBackground.Set(true);
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        DanceCodeFix.Apply(sceneName);
    }
}
