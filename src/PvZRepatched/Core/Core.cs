namespace PvZRepatched;

using MelonLoader;

using PvZRepatched.Fixes;
using PvZRepatched.Logger;

internal sealed class Core : MelonMod
{
    public override void OnInitializeMelon()
    {
        // TODO: Read params from config
        UnityLogger.Install(UnityLogMode.Filtered, false);
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        DanceCodeFix.Apply(sceneName);
    }
}
