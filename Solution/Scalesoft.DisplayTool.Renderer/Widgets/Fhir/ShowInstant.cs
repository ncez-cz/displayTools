using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class ShowInstant(string path = ".") : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer, RenderContext context)
    {
        // instant is just a datetime with additional constraints
        return new ShowDateTime(path).Render(navigator, renderer, context);
    }
}