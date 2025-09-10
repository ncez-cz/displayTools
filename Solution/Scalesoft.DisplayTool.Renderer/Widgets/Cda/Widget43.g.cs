using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class Widget43(int i) : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Condition($"{i+1} > 1", [
new ConstantText(@", "), 
])
, 
new Choose([
new When("not(n1:value/@nullFlavor)", [
new WidgetWithVariables(new ShowEHdsiReactionAllergyWidget(), [
new Variable("node", "n1:value"),
]), 
new WidgetWithVariables(new ShowEHdsiIllnessandDisorderWidget(), [
new Variable("node", "n1:value"),
]), 
]),
], [
new WidgetWithVariables(new ShowEHdsiNullFlavorWidget(), [
new Variable("code", "n1:value/@nullFlavor"),
]), 
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
