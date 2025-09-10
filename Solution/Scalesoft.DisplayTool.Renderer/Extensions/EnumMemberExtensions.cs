using System.Reflection;
using System.Runtime.Serialization;

namespace Scalesoft.DisplayTool.Renderer.Extensions;

public static class EnumMemberExtensions
{
    public static string? ToEnumString<T>(this T value) where T : struct, Enum
    {
        var enumType = typeof(T);
        var name = Enum.GetName(value);
        if (name == null)
        {
            return null;
        }

        var field = enumType.GetField(name);
        if (field == null)
        {
            return null;
        }

        var attribute = field.GetCustomAttribute<EnumMemberAttribute>();
        return attribute?.Value;
    }

    public static T? ToEnum<T>(this string? str) where T : struct, Enum
    {
        if (string.IsNullOrEmpty(str))
        {
            return null;
        }
        var enumType = typeof(T);

        foreach (var value in Enum.GetValues<T>())
        {
            var name = Enum.GetName(value);
            if (name == null)
            {
                continue;
            }

            var field = enumType.GetField(name);
            if (field == null)
            {
                continue;
            }

            var attribute = field.GetCustomAttribute<EnumMemberAttribute>();
            if (attribute?.Value == str) return value;
        }

        return null;
    }
}