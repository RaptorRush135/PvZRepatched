namespace PvZRepatched.Configuration;

using System.Reflection;

using BloomEngine.ModMenu;

using MelonLoader;

using PvZRepatched.Logger;

internal static class ModConfig
{
    private static IModConfig? instance;

    public static IModConfig TryRegister(MelonMod mod)
    {
        if (instance != null)
        {
            throw new InvalidOperationException($"{nameof(ModConfig)} has already been registered.");
        }

        instance = TryRegisterInternal(mod);
        return instance;
    }

    private static IModConfig TryRegisterInternal(MelonMod mod)
    {
        try
        {
            return Register(mod);
        }
        catch (FileNotFoundException ex)
            when (IsMissingBloomEngine(ex))
        {
            return DefaultModConfig(null);
        }
        catch (Exception ex)
        {
            return DefaultModConfig(ex);
        }

        static IModConfig DefaultModConfig(Exception? exception)
        {
            const string MessagePrefix = "Could not load BloomEngine, using defaults";
            if (exception != null)
            {
                Melon<Core>.Logger.Warning($"{MessagePrefix}\n{exception}");
            }
            else
            {
                Melon<Core>.Logger.Msg(MessagePrefix);
            }

            var defaultConfig = new DefaultModConfig();
            Melon<Core>.Logger.Msg(defaultConfig);
            return defaultConfig;
        }
    }

    private static IModConfig Register(MelonMod mod)
    {
        ModMenuService.CreateEntry(mod)
            .AddConfigClass(typeof(ModBloomConfig))
            .Register();

        return new BloomEngineModConfig();
    }

    private static bool IsMissingBloomEngine(FileNotFoundException exception)
    {
        if (string.IsNullOrEmpty(exception.FileName))
        {
            return false;
        }

        try
        {
            return new AssemblyName(exception.FileName).Name == "BloomEngine";
        }
        catch (Exception)
        {
            return false;
        }
    }

    internal static class Defaults
    {
        public const bool RunInBackground = true;

        public const UnityLogMode UnityLogMode = UnityLogMode.Filtered;

        public const bool LogMissingLocalization = false;
    }

    private sealed record DefaultModConfig : IModConfig
    {
#pragma warning disable CS0067
        public event Action<bool>? OnRunInBackgroundChanged;
#pragma warning restore CS0067

        public bool RunInBackground => Defaults.RunInBackground;

        public UnityLogMode UnityLogMode => Defaults.UnityLogMode;

        public bool LogMissingLocalization => Defaults.LogMissingLocalization;
    }
}
