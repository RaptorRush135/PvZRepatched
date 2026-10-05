namespace PvZRepatched.Configuration;

using BloomEngine.Config;
using BloomEngine.Config.Inputs;

using PvZRepatched.Logger;

internal static class ModBloomConfig
{
    public static BoolConfigInput RunInBackground { get; } = ConfigService.CreateBool(
        name: "Run in Background",
        description: "Prevents the game from freezing/pausing when it loses focus.",
        defaultValue: ModConfig.Defaults.RunInBackground);

    public static EnumConfigInput<UnityLogMode> UnityLogMode { get; } = ConfigService.CreateEnum(
        name: "Unity Log Mode",
        description: "Controls Unity logging verbosity.",
        defaultValue: ModConfig.Defaults.UnityLogMode);

    public static BoolConfigInput LogMissingLocalization { get; } = ConfigService.CreateBool(
        name: "Log Missing Localization",
        description: "Enables logging of missing localization IDs.",
        defaultValue: ModConfig.Defaults.LogMissingLocalization);
}
