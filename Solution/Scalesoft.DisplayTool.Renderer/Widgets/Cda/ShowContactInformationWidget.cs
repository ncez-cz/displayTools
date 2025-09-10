using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class ShowContactInformationWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        List<Widget> widgetTree =
        [
            new Column([
                new WidgetWithVariables(new ShowContactInfoWidget(), [
                    new Variable("contact", "$contactInfoRoot"),
                ]),
            ], null, null, true),
        ];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}