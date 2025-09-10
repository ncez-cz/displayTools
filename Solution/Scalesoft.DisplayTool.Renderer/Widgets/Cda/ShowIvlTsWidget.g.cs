using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class ShowIvlTsWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Variable("low", "$node/n1:low"), 
new Variable("high", "$node/n1:high"), 
new Variable("width", "$node/n1:width"), 
new Variable("center", "$node/n1:center"), 
new Variable("nullFlavor", "$node/@nullFlavor"), 
new Choose([
new When("$width", [
new WidgetWithVariables(new ShowPqWidget(), [
new Variable("node", "$width"),
]), 
new Choose([
new When("$low", [
new ConstantText(@" "), 
new WidgetWithVariables(new ShowEHdsiDisplayLabelWidget(), [
new Variable("code", "'147'"),
]), 
new ConstantText(@" "), 
new WidgetWithVariables(new ShowTsWidget(), [
new Variable("node", "$low"),
]), 
]),
new When("$high", [
new ConstantText(@" "), 
new WidgetWithVariables(new ShowEHdsiDisplayLabelWidget(), [
new Variable("code", "'148'"),
]), 
new ConstantText(@" "), 
new WidgetWithVariables(new ShowTsWidget(), [
new Variable("node", "$high"),
]), 
]),
new When("$center", [
new ConstantText(@" "), 
new WidgetWithVariables(new ShowEHdsiDisplayLabelWidget(), [
new Variable("code", "'149'"),
]), 
new ConstantText(@" "), 
new WidgetWithVariables(new ShowTsWidget(), [
new Variable("node", "$center"),
]), 
]),
], [
]), 
]),
new When("$low and $high", [
new WidgetWithVariables(new ShowEHdsiDisplayLabelWidget(), [
new Variable("code", "'147'"),
]), 
new ConstantText(@" "), 
new WidgetWithVariables(new ShowTsWidget(), [
new Variable("node", "$low"),
]), 
new ConstantText(@" "), 
new WidgetWithVariables(new ShowEHdsiDisplayLabelWidget(), [
new Variable("code", "'148'"),
]), 
new ConstantText(@" "), 
new WidgetWithVariables(new ShowTsWidget(), [
new Variable("node", "$high"),
]), 
]),
new When("$low", [
new WidgetWithVariables(new ShowEHdsiDisplayLabelWidget(), [
new Variable("code", "'147'"),
]), 
new ConstantText(@" "), 
new WidgetWithVariables(new ShowTsWidget(), [
new Variable("node", "$low"),
]), 
]),
new When("$high", [
new WidgetWithVariables(new ShowEHdsiDisplayLabelWidget(), [
new Variable("code", "'148'"),
]), 
new ConstantText(@" "), 
new WidgetWithVariables(new ShowTsWidget(), [
new Variable("node", "$high"),
]), 
]),
new When("$nullFlavor", [
new WidgetWithVariables(new ShowEHdsiNullFlavorWidget(), [
new Variable("code", "$nullFlavor"),
]), 
]),
], [
new WidgetWithVariables(new ShowTsWidget(), [
new Variable("node", "$node"),
]), 
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
