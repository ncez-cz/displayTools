using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class ShowStrengthWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Variable("numerator", "$node/n1:numerator"), 
new Variable("denominator", "$node/n1:denominator"), 
new Variable("numeratorValue", "$numerator/@value"), 
new Variable("numeratorUnit", [ 
new WidgetWithVariables(new ShowEHdsiUnitWidget(), [
new Variable("code", "$numerator/@unit"),
]), 
]), 
new Variable("denominatorValue", "$denominator/@value"), 
new Variable("medStrengthOriginalText", "$node/n1:translation/n1:originalText"), 
new Variable("denominatorUnit", [ 
new WidgetWithVariables(new SupportUcumAnnotationsWidget(), [
new Variable("value", "$denominator/@unit"),
]), 
]), 
new Choose([
new When("($numerator/@nullFlavor)", [
new WidgetWithVariables(new ShowEHdsiNullFlavorWidget(), [
new Variable("code", "$numerator/@nullFlavor"),
]), 
]),
new When("($denominator/@nullFlavor)", [
new Text("$numeratorValue")
, 
new ConstantText(@" "), 
new Text("$numeratorUnit")
, 
new ConstantText(@" "), 
new ConstantText(@"
                /
                "), 
new ConstantText(@" "), 
new WidgetWithVariables(new ShowEHdsiNullFlavorWidget(), [
new Variable("code", "$denominator/@nullFlavor"),
]), 
]),
new When("$denominatorUnit='1'", [
new Text("$numeratorValue")
, 
new ConstantText(@" "), 
new Text("$numeratorUnit")
, 
new ConstantText(@" "), 
new WidgetWithVariables(new ShowEHdsiDisplayLabelWidget(), [
new Variable("code", "'53'"),
]), 
]),
new When("not($numeratorValue) and not($denominatorValue)", [
new ConstantText(@"
                /
            "), 
]),
new When("not($denominatorValue)", [
new Text("$numeratorValue")
, 
new ConstantText(@" "), 
new Text("$numeratorUnit")
, 
new ConstantText(@" "), 
new ConstantText(@"
                /
            "), 
]),
], [
new Text("$numeratorValue")
, 
new ConstantText(@" "), 
new Text("$numeratorUnit")
, 
new ConstantText(@" "), 
new ConstantText(@"
                /
                "), 
new ConstantText(@" "), 
new Text("$denominatorValue")
, 
new ConstantText(@" "), 
new Text("$denominatorUnit")
, 
]), 
new Condition("$medStrengthOriginalText", [
new TableRow([
new TableCell([
new Tooltip([], [
new Container([
new Text("$medStrengthOriginalText")
, 
], ContainerType.Div), 
new Container([
new ConstantText(@"Additional info"), 
], ContainerType.Span), 
]), 
],
TableCellType.Data), 
]), 
])
, 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
