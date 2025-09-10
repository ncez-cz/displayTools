using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class ShowAssignedEntityWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Choose([
new When("$asgnEntity/n1:assignedPerson/n1:name", [
new WidgetWithVariables(new ShowNameWidget(), [
new Variable("name", "$asgnEntity/n1:assignedPerson/n1:name"),
]), 
new Condition("$asgnEntity/n1:representedOrganization/n1:name", [
new ConstantText(@" of "), 
new Text("$asgnEntity/n1:representedOrganization/n1:name")
, 
])
, 
]),
new When("$asgnEntity/n1:representedOrganization", [
new Text("$asgnEntity/n1:representedOrganization/n1:name")
, 
]),
], [
new ConcatBuilder("$asgnEntity/n1:id", (i, count) => [
new WidgetWithVariables(new ShowIdWidget(), [
]), 
new Choose([
new When($"{i}!={count-1}", [
new ConstantText(@", "), 
]),
], [
new LineBreak(), 
]), 
])
, 
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
