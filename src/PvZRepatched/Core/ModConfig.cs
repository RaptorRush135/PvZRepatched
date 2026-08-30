namespace PvZRepatched;

using System.Text.Json;

using MelonLoader;

using PvZRepatched.Logger;

internal sealed record ModConfig(
    UnityLogMode UnityLogMode = UnityLogMode.Filtered,
    bool LogMissingLocalization = false)
{
    public static ModConfig Default { get; } = new();

    public static ModConfig DeserializeConfig(FileInfo configFile)
    {
        if (!configFile.Exists)
        {
            return CreateDefaultConfig(configFile);
        }

        try
        {
            using FileStream stream = configFile.OpenRead();
            return JsonSerializer.Deserialize<ModConfig>(stream)
                ?? throw new InvalidOperationException("Failed to deserialize configuration file");
        }
        catch (Exception ex)
        {
            Melon<Core>.Logger.Warning($"Failed to read configuration file\n{ex}");
            return Default;
        }
    }

    private static ModConfig CreateDefaultConfig(FileInfo configFile)
    {
        try
        {
            configFile.Directory?.Create();
            var options = new JsonSerializerOptions { WriteIndented = true };
            using FileStream stream = configFile.Create();
            JsonSerializer.Serialize(stream, Default, options);
        }
        catch (Exception ex)
        {
            Melon<Core>.Logger.Warning($"Failed to write default configuration file\n{ex}");
        }

        return Default;
    }
}
