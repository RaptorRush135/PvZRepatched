namespace PvZRepatched.Extensions;

using System.Reflection;

public static class ReflectionExtensions
{
    public static MethodInfo GetMethodOrThrow(this Type type, string name, params Type[] types)
    {
        return type.GetMethod(name, types)
            ?? throw new MissingMethodException(type.FullName, name);
    }
}
