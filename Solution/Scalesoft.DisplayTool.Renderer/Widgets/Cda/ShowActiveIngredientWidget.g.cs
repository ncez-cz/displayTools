using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class ShowActiveIngredientWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Variable("medActiveIngredientNode", "pharm:ingredientSubstance"), 
new Variable("medActiveIngredientNodeCode", "$medActiveIngredientNode/pharm:code"), 
new Variable("medActiveIngredient", "$medActiveIngredientNodeCode"), 
new Variable("medActiveIngredientID", "$medActiveIngredientNodeCode/@code"), 
new Variable("medStrength", "pharm:quantity"), 
new Choose([
new When("not ($medActiveIngredientNodeCode/@nullFlavor)", [
new Choose([
new When("$medActiveIngredient", [
new WidgetWithVariables(new ShowEHdsiActiveIngredientWidget(), [
new Variable("node", "$medActiveIngredientNodeCode"),
]), 
new WidgetWithVariables(new ShowEHdsiSubstanceWidget(), [
new Variable("node", "$medActiveIngredientNodeCode"),
]), 
new ConstantText(@" ("), 
new Text("$medActiveIngredientID")
, 
new ConstantText(@")"), 
]),
], [
new Condition("$medActiveIngredientNodeCode/n1:originalText/n1:reference/@value", [
new WidgetWithVariables(new ShowUncodedElementWidget(), [
new Variable("code", "$medActiveIngredientNodeCode/n1:originalText/n1:reference/@value"),
]), 
])
, 
]), 
]),
], [
new Choose([
new When("$medActiveIngredientNode/pharm:name", [
new Text("$medActiveIngredientNode/pharm:name")
, 
]),
], [
new WidgetWithVariables(new ShowEHdsiNullFlavorWidget(), [
new Variable("code", "$medActiveIngredientNodeCode/@nullFlavor"),
]), 
]), 
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
