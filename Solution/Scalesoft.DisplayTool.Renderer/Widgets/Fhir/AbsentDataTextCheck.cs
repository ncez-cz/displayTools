using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class AbsentDataTextCheck(string path, Widget child) : Widget
{
    public override async Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        var element = navigator.SelectSingleNode(path + "/@value");
        return IsDataAbsent(navigator, path) ? await new AbsentData(path).Render(navigator, renderer, context) : await child.Render(element, renderer, context);
    }
}