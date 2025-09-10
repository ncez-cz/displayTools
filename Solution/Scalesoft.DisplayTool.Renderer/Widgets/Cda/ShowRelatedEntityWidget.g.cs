using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class ShowRelatedEntityWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Choose([
new When("$relatedEntity/n1:relatedPerson/n1:name", [
new WidgetWithVariables(new ShowNameWidget(), [
new Variable("name", "$relatedEntity/n1:relatedPerson/n1:name"),
]), 
]),
], [
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
