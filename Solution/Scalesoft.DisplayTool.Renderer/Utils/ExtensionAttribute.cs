namespace Scalesoft.DisplayTool.Renderer.Utils;

[AttributeUsage(AttributeTargets.Field)]
public class ExtensionAttribute : PropertyPathAttribute
{
    public ExtensionAttribute(string url) : base($"f:extension[@url='{url}']")
    {
    }
}