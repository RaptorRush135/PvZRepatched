[assembly: MelonLoader.MelonOptionalDependencies("BloomEngine")]

namespace PvZRepatched;

using MelonLoader;

using PvZRepatched.Configuration;
using PvZRepatched.Features;
using PvZRepatched.Fixes;
using PvZRepatched.Logger;

internal sealed class Core : MelonMod
{
    private IDisposable? cobCannonCoinFix;

    public override void OnInitializeMelon()
    {
        var config = ModConfig.TryRegister(this);

        UnityLogger.Install(config);

        RunInBackground.Bind(config);

        this.cobCannonCoinFix = CobCannonCoinFix.Apply();
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        DanceCodeFix.Apply(sceneName);
    }

    public override void OnDeinitializeMelon()
    {
        this.cobCannonCoinFix?.Dispose();
    }
}
