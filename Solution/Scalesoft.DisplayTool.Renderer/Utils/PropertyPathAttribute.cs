using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Utils;

[AttributeUsage(AttributeTargets.Field)]
public class PropertyPathAttribute : InfrequentPropertyAttribute
{
    private string Path { get; }

    public PropertyPathAttribute(string path)
    {
        Path = path;
    }

    public override string? Evaluate(List<XmlDocumentNavigator> items)
    {
        return items.Any(x => x.EvaluateCondition(Path)) ? Path : null;
    }
}