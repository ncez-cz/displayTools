using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class SupportUcumAnnotationsWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Choose([
new When("contains($value, '{')", [
new WidgetWithVariables(new ShowEHdsiUnitWidget(), [
new Variable("code", "substring-before($value, '{')"),
]), 
new Condition("substring-before($value, '{')", [
new RawText(@" "), 
])
, 
new Text("substring-before(substring-after($value, '{'), '}')")
, 
new Condition("substring-after($value, '}')", [
new RawText(@" "), 
])
, 
new WidgetWithVariables(new ShowEHdsiUnitWidget(), [
new Variable("code", "substring-after($value, '}')"),
]), 
]),
new When("$value='1'", [
new Text("$value")
, 
]),
new When("not($value)", [
new Text("1")
, 
]),
], [
new WidgetWithVariables(new ShowEHdsiUnitWidget(), [
new Variable("code", "$value"),
]), 
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
