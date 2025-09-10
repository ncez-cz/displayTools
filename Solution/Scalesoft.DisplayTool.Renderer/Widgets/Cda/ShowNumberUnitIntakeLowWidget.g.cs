using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class ShowNumberUnitIntakeLowWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new WidgetWithVariables(new ShowNumberUnitIntakeIntervalEndpointWidget(), [
new Variable("medUnitIntakeGlobal", "$medUnitIntake"),
new Variable("medUnitIntakeEndpoint", "$medUnitIntake/n1:low"),
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
