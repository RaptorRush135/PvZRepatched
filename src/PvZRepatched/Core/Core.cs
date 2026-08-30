namespace PvZRepatched;

using MelonLoader;

using PvZRepatched.Environment;
using PvZRepatched.Extensions;
using PvZRepatched.Fixes;
using PvZRepatched.Logger;
using PvZRepatched.Metadata;

internal sealed class Core : MelonMod
{
    public override void OnInitializeMelon()
    {
        var environment = new ModEnvironment(ModInfo.Name);

        FileInfo configurationFile = environment.ModUserDataDirectory.GetFile("configuration.json");
        ModConfig config = ModConfig.DeserializeConfig(configurationFile);

        this.LoggerInstance.Msg(config);

        UnityLogger.Install(config.UnityLogMode, config.LogMissingLocalization);
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        DanceCodeFix.Apply(sceneName);
    }
}
