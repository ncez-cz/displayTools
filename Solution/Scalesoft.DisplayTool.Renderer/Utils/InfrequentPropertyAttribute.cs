using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Utils;

public abstract class InfrequentPropertyAttribute : Attribute
{
    public abstract string? Evaluate(List<XmlDocumentNavigator> items);
}