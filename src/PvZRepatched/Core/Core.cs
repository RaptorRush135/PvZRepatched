namespace PvZRepatched;

using MelonLoader;

using PvZRepatched.Fixes;

public sealed class Core : MelonMod
{
    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        DanceCodeFix.Apply(sceneName);
    }
}
