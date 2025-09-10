using System.Reflection;
using Scalesoft.DisplayTool.Renderer.Utils;

namespace Scalesoft.DisplayTool.Renderer.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> source) where T : class
    {
        return source.Where(item => item != null).Cast<T>();
    }

    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> source) where T : struct
    {
        return source.Where(item => item != null).Select(x => x!.Value);
    }


    public static bool ContainsAnyOf<T>(this IEnumerable<T> source, params T[] items)
    {
        return source.Any(items.Contains);
    }

    public static bool ContainsAllOf<T>(this IEnumerable<T> source, params T[] items)
    {
        return items.All(source.Contains);
    }

    public static bool HasAnyOfGroup<T>(this IEnumerable<T> source, string groupName) where T : Enum
    {
        return source
            .Select(property => typeof(T).GetField(property.ToString()))
            .Select(field => field?.GetCustomAttribute<GroupAttribute>())
            .Any(groupAttribute => groupAttribute?.GroupName == groupName);
    }

    public static bool ContainsOnly<T>(this IEnumerable<T> source, params T[] items)
    {
        var sourceSet = source.ToHashSet();
        var itemsSet = items.ToHashSet();

        return sourceSet.SetEquals(itemsSet);
    }
}