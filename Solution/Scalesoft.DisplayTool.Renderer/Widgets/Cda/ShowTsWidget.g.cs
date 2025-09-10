using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class ShowTsWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Choose([
new When("not($node)", [
new ConstantText(@" "), 
]),
], [
new Choose([
new When("not($node/@nullFlavor)and $node/@value", [
new WidgetWithVariables(new FormatDateTimeWidget(), [
new Variable("date", "$node/@value"),
]), 
]),
], [
new WidgetWithVariables(new ShowEHdsiNullFlavorWidget(), [
new Variable("code", "$node/@nullFlavor"),
]), 
]), 
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
