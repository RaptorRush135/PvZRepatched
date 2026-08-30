namespace PvZRepatched.Logger;

using MelonLoader;

using UnityEngine;

internal static class UnityLogger
{
    private static readonly IReadOnlyCollection<string> IgnoredMessages = [
        "Tween's 'endValue' equals to the current animated value:",
        "Tween is started on GameObject that is not active in hierarchy:",
        "Unloading the last loaded scene Assets/Scenes/Gameplay.unity(build index: -1), is not supported.",
    ];

    public static void Install(UnityLogMode logMode, bool logMissingLocalization)
    {
        if (logMode == UnityLogMode.Disabled)
        {
            return;
        }

        Application.add_logMessageReceived((Application.LogCallback)LogMessageReceived);

        void LogMessageReceived(string condition, string stackTrace, LogType type)
        {
            if (logMode == UnityLogMode.Filtered
                && !ShouldLog(condition))
            {
                return;
            }

            string text = $"[Unity] ({type}) {condition}\n{stackTrace}";

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
            if (IgnoredMessages.Any(condition.StartsWith))
            {
                return false;
            }

            if (!logMissingLocalization
                && condition.StartsWith("Failed to find localization ID:"))
            {
                return false;
            }

            return true;
        }
    }
}
