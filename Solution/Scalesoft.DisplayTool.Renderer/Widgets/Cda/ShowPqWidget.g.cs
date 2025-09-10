using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class ShowPqWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Variable("unit", [ 
new WidgetWithVariables(new SupportUcumAnnotationsWidget(), [
new Variable("value", "$node/@unit"),
]), 
]), 
new Choose([
new When("not ($node/@nullFlavor)", [
new Choose([
new When("$node/@value", [
new Text("$node/@value")
, 
new ConstantText(@" "), 
new Choose([
new When("$unit='1'", [
new WidgetWithVariables(new ShowEHdsiDisplayLabelWidget(), [
new Variable("code", "'77'"),
]), 
]),
], [
new Text("$unit")
, 
]), 
]),
], [
new Condition("$node/n1:originalText/n1:reference/@value", [
new WidgetWithVariables(new ShowUncodedElementWidget(), [
new Variable("code", "$node/n1:originalText/n1:reference/@value"),
]), 
])
, 
]), 
]),
], [
new WidgetWithVariables(new ShowEHdsiNullFlavorWidget(), [
new Variable("code", "$node/@nullFlavor"),
]), 
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
