namespace PvZRepatched.Logger;

using MelonLoader;

using PvZRepatched.Configuration;

using UnityEngine;

internal static class UnityLogger
{
    private const string MissingLocalizationPrefix = "Failed to find localization ID:";

    private static readonly IReadOnlyCollection<string> IgnoredMessages = [
        "RenderGraph is now disabled.",
        "Tween's 'endValue' equals to the current animated value:",
        "Tween is started on GameObject that is not active in hierarchy:",
        "Unloading the last loaded scene Assets/Scenes/Gameplay.unity(build index: -1), is not supported.",
    ];

    public static void Install(IModConfig config)
    {
        Application.add_logMessageReceived((Application.LogCallback)LogMessageReceived);

        void LogMessageReceived(string condition, string stackTrace, LogType type)
        {
            if (!ShouldLog(condition))
            {
                return;
            }

            string cleanedStackTrace = CleanStackTrace(stackTrace);

            string text = $"[Unity] ({type}) {condition}\n{cleanedStackTrace}";

            switch (type)
            {
                case LogType.Assert:
                case LogType.Warning:
                    Melon<Core>.Logger.Warning(text);
                    break;
                case LogType.Error:
                case LogType.Exception:
                    Melon<Core>.Logger.Error(text);
                    break;
                default:
                    Melon<Core>.Logger.Msg(text);
                    break;
            }
        }

        bool ShouldLog(string condition)
        {
            if (config.UnityLogMode == UnityLogMode.Disabled)
            {
                return false;
            }

            if (condition.StartsWith(MissingLocalizationPrefix))
            {
                return config.LogMissingLocalization;
            }

            return config.UnityLogMode != UnityLogMode.Filtered
                || !IgnoredMessages.Any(condition.StartsWith);
        }

        string CleanStackTrace(string stackTrace)
        {
            return stackTrace.Replace(
                " (at <00000000000000000000000000000000>:0)",
                string.Empty);
        }
    }
}
