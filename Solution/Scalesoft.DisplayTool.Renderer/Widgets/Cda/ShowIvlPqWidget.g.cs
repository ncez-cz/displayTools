using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class ShowIvlPqWidget : Widget
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
new ConstantText(@" 
            "), 
]),
new When("$low and $high", [
new WidgetWithVariables(new ShowPqWidget(), [
new Variable("node", "$low"),
]), 
new ConstantText(@" -
                "), 
new WidgetWithVariables(new ShowPqWidget(), [
new Variable("node", "$high"),
]), 
]),
new When("$low", [
new WidgetWithVariables(new ShowPqWidget(), [
new Variable("node", "$low"),
]), 
new ConstantText(@" 
            "), 
]),
new When("$high", [
new WidgetWithVariables(new ShowPqWidget(), [
new Variable("node", "$high"),
]), 
new ConstantText(@" 
            "), 
]),
new When("$nullFlavor", [
new WidgetWithVariables(new ShowEHdsiNullFlavorWidget(), [
new Variable("code", "$nullFlavor"),
]), 
new ConstantText(@" 
            "), 
]),
], [
new WidgetWithVariables(new ShowPqWidget(), [
new Variable("node", "$node"),
]), 
new ConstantText(@" 
            "), 
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
