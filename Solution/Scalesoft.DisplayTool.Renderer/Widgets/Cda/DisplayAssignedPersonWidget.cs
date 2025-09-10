using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class DisplayAssignedPersonWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        List<Widget> widgetTree =
        [
            new WidgetWithVariables(new ShowNameWidget(), [
                new Variable("name", "$assignedPerson/n1:name"),
            ]),
            new WidgetWithVariables(new ShowContactInformationWidget(), [
                new Variable("contactInfoRoot", "$contactInfoRoot"),
            ]),
        ];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}