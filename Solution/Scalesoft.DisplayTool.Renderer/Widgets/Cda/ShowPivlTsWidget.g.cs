using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class ShowPivlTsWidget : Widget
{ 
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Variable("medPhase", "$node/n1:phase"), 
new Variable("medPeriod", "$node/n1:period"), 
new Variable("medPhaseWidth", "$medPhase/n1:width"), 
new Variable("medPhaseLow", "$medPhase/n1:low"), 
new Choose([
new When("$node/@institutionSpecified='true'", [
new Choose([
new When("(1 div $medPeriod/@value) >= 1", [
new Text("round(1 div $medPeriod/@value)")
, 
new ConstantText(@" "), 
new WidgetWithVariables(new ShowEHdsiDisplayLabelWidget(), [
new Variable("code", "'190'"),
]), 
new ConstantText(@" "), 
new Condition("$medPeriod/@unit", [
new WidgetWithVariables(new ShowEHdsiUnitWidget(), [
new Variable("code", "$medPeriod/@unit"),
]), 
])
, 
]),
], [
new ConstantText(@"1 "), 
new WidgetWithVariables(new ShowEHdsiDisplayLabelWidget(), [
new Variable("code", "'190'"),
]), 
new ConstantText(@" "), 
new Text("$medPeriod/@value")
, 
new ConstantText(@" "), 
new Condition("$medPeriod/@unit", [
new WidgetWithVariables(new ShowEHdsiUnitWidget(), [
new Variable("code", "$medPeriod/@unit"),
]), 
])
, 
]), 
]),
], [
new WidgetWithVariables(new ShowEHdsiDisplayLabelWidget(), [
new Variable("code", "'27'"),
]), 
new ConstantText(@" "), 
new Text("$medPeriod/@value")
, 
new ConstantText(@" "), 
new Condition("$medPeriod/@unit", [
new WidgetWithVariables(new ShowEHdsiUnitWidget(), [
new Variable("code", "$medPeriod/@unit"),
]), 
])
, 
]), 
new Condition("$medPhaseWidth", [
new ConstantText(@" "), 
new WidgetWithVariables(new ShowEHdsiDisplayLabelWidget(), [
new Variable("code", "'31'"),
]), 
new ConstantText(@" "), 
new Text("$medPhaseWidth/@value")
, 
new ConstantText(@"
             
            "), 
new WidgetWithVariables(new ShowEHdsiUnitWidget(), [
new Variable("code", "$medPhaseWidth/@unit"),
]), 
])
, 
new Condition("$medPhaseLow", [
new WidgetWithVariables(new ShowEHdsiDisplayLabelWidget(), [
new Variable("code", "'6'"),
]), 
new WidgetWithVariables(new ShowTsWidget(), [
new Variable("node", "$medPhaseLow"),
]), 
new ConstantText(@"
             
        "), 
])
, 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
