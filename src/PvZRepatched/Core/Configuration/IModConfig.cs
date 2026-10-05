namespace PvZRepatched.Configuration;

using PvZRepatched.Logger;

internal interface IModConfig
{
    event Action<bool>? OnRunInBackgroundChanged;

    bool RunInBackground { get; }

    UnityLogMode UnityLogMode { get; }

    bool LogMissingLocalization { get; }
}
