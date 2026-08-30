namespace PvZRepatched.Logger;

using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
internal enum UnityLogMode
{
    Disabled,
    Filtered,
    All,
}
