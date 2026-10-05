namespace PvZRepatched.Configuration;

using PvZRepatched.Logger;

internal sealed class BloomEngineModConfig : IModConfig
{
    public BloomEngineModConfig()
    {
        ModBloomConfig.RunInBackground.WithOnValueApplied(
            value => this.OnRunInBackgroundChanged?.Invoke(value));
    }

    public event Action<bool>? OnRunInBackgroundChanged;

    public bool RunInBackground => ModBloomConfig.RunInBackground.Value;

    public UnityLogMode UnityLogMode => ModBloomConfig.UnityLogMode.Value;

    public bool LogMissingLocalization => ModBloomConfig.LogMissingLocalization.Value;
}
