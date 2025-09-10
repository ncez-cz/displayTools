namespace Scalesoft.DisplayTool.Renderer.Extensions;

public static class KeyValuePairExtensions
{
    public static bool IsDefault<TKey, TValue>(this KeyValuePair<TKey, TValue> value)
    {
        var defVal = default(KeyValuePair<TKey, TValue>);
        return ((value.Key == null && defVal.Key == null) || value.Key?.Equals(defVal.Key) == true) && ((value.Value == null && defVal.Value == null) || value.Value?.Equals(defVal.Value) == true);
    }
}