using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class ShowDuration(string path = ".") : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer, RenderContext context)
    {
        if (context.RenderMode == RenderMode.Documentation)
        {
            return Task.FromResult<RenderResult>(navigator.SelectSingleNode(path).GetFullPath());
        }

        Widget[] widgetTree =
        [
            new ShowQuantity(path) // duration is just a quantity with additional constraints on code and system
        ];

        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}