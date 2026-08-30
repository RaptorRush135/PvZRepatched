namespace PvZRepatched.Extensions;

using System.Diagnostics.CodeAnalysis;

using UnityObject = UnityEngine.Object;

public static class UnityExtensions
{
    private const string UnityNullJustification =
        "UnityEngine.Object does not support ?. or ?? for detached objects; explicit null check required.";

    [SuppressMessage(
        "Style",
        "IDE0029:Use coalesce expression",
        Justification = UnityNullJustification)]
    [SuppressMessage(
        "Roslynator",
        "RCS1084:Use coalesce expression instead of conditional expression",
        Justification = UnityNullJustification)]
    public static T? Ref<T>(this T? obj)
        where T : UnityObject
    {
        return obj != null ? obj : null;
    }
}
