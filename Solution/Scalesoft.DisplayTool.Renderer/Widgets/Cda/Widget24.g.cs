using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class Widget24 : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Container([
new Table([
new Choose([
new When("n1:assignedEntity/n1:assignedPerson", [
new WidgetWithVariables(new DisplayAssignedPersonWidget(), [
new Variable("assignedPerson", "n1:assignedEntity/n1:assignedPerson"),
new Variable("contactInfoRoot", "n1:assignedEntity"),
]), 
]),
new When("n1:assignedEntity/n1:representedOrganization", [
new WidgetWithVariables(new DisplayRepresentedOrganizationWidget(), [
new Variable("representedOrganization", "n1:assignedEntity/n1:representedOrganization"),
]), 
]),
], [
]), 
]), 
], ContainerType.Div), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
