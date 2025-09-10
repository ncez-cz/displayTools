using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Utils;

public abstract class InfrequentPropertyNegativeAttribute : Attribute
{
    public abstract bool ShouldRemove(List<XmlDocumentNavigator> items, string originalPath);
}