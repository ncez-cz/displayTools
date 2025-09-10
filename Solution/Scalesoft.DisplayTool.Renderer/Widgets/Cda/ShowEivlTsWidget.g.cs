using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class ShowEivlTsWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Variable("medEvent", "$node/n1:event"), 
new Variable("medOffset", "$node/n1:offset"), 
new Variable("medOffsetWidth", "$medOffset/n1:width"), 
new Variable("medOffsetLow", "$medOffset/n1:low"), 
new Condition("$medOffsetLow", [
new Text("$medOffsetLow/@value")
, 
new ConstantText(@" "), 
new Text("$medOffsetLow/@unit")
, 
new ConstantText(@" "), 
])
, 
new WidgetWithVariables(new ShowEHdsiTimingEventWidget(), [
new Variable("node", "$medEvent"),
]), 
new Condition("$medOffsetWidth", [
new ConstantText(@" "), 
new WidgetWithVariables(new ShowEHdsiDisplayLabelWidget(), [
new Variable("code", "'31'"),
]), 
new ConstantText(@" "), 
new Text("$medOffsetWidth/@value")
, 
new ConstantText(@" "), 
new WidgetWithVariables(new ShowEHdsiUnitWidget(), [
new Variable("code", "$medOffsetWidth/@unit"),
]), 
])
, 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
