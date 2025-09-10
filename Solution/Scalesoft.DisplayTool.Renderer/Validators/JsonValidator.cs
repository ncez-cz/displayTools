using System.Text.Json;

namespace Scalesoft.DisplayTool.Renderer.Validators;

public class JsonValidator
{
    public static bool IsValidJson(string str)
    {
        try
        {
            JsonSerializer.Deserialize<object>(str);
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }
}