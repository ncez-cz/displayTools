using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class ShowAssociatedEntityWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Choose([
new When("$assoEntity/n1:associatedPerson", [
new ConcatBuilder("$assoEntity/n1:associatedPerson/n1:name", (i) => [
new WidgetWithVariables(new ShowNameWidget(), [
new Variable("name", "."),
]), 
new LineBreak(), 
])
, 
]),
new When("$assoEntity/n1:scopingOrganization", [
new ConcatBuilder("$assoEntity/n1:scopingOrganization", (i) => [
new Condition("n1:name", [
new WidgetWithVariables(new ShowNameWidget(), [
new Variable("name", "n1:name"),
]), 
new LineBreak(), 
])
, 
new Condition("n1:standardIndustryClassCode", [
new Text("n1:standardIndustryClassCode/@displayName")
, 
new ConstantText(@" code:"), 
new Text("n1:standardIndustryClassCode/@code")
, 
])
, 
])
, 
]),
new When("$assoEntity/n1:code", [
new WidgetWithVariables(new ShowCodeWidget(), [
new Variable("code", "$assoEntity/n1:code"),
]), 
]),
new When("$assoEntity/n1:id", [
new Text("$assoEntity/n1:id/@extension")
, 
new ConstantText(@" "), 
new Text("$assoEntity/n1:id/@root")
, 
]),
], [
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
