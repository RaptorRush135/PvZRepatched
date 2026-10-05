[assembly: MelonLoader.MelonOptionalDependencies("BloomEngine")]

namespace PvZRepatched;

using MelonLoader;

using PvZRepatched.Configuration;
using PvZRepatched.Features;
using PvZRepatched.Fixes;
using PvZRepatched.Logger;

internal sealed class Core : MelonMod
{
    public override void OnInitializeMelon()
    {
        var config = ModConfig.TryRegister(this);

        UnityLogger.Install(config);

        RunInBackground.Bind(config);
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        DanceCodeFix.Apply(sceneName);
    }
}
