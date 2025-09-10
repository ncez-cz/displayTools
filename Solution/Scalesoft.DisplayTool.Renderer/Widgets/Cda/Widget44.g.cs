using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class Widget44 : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Choose([
new When("not(n1:code/@nullFlavor)", [
new WidgetWithVariables(new ShowEHdsiAllergenNoDrugWidget(), [
new Variable("node", "n1:code"),
]), 
new WidgetWithVariables(new ShowEHdsiActiveIngredientWidget(), [
new Variable("node", "n1:code"),
]), 
]),
], [
new WidgetWithVariables(new HandleNullFlavorWidget(), [
new Variable("node", "n1:code"),
]), 
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
