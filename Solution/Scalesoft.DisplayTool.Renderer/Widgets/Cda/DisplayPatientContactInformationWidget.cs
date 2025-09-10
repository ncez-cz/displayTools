using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class DisplayPatientContactInformationWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        List<Widget> widgetTree =
        [
            new Container([
                new WidgetWithVariables(new ShowContactInformationWidget(), [
                    new Variable("assignedPerson", "$patientRole"),
                    new Variable("contactInfoRoot", "$patientRole"),
                ]),
            ], ContainerType.Div),
        ];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}