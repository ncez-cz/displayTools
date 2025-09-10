using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class ShowMedicinalProductWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Variable("generalizedMaterialKindCode", "pharm:asSpecializedKind/pharm:generalizedMaterialKind/pharm:code"), 
new Choose([
new When("$generalizedMaterialKindCode and not($generalizedMaterialKindCode/@nullFlavor)", [
new WidgetWithVariables(new ShowEHdsiActiveIngredientWidget(), [
new Variable("node", "$generalizedMaterialKindCode"),
]), 
]),
], [
new Choose([
new When("n1:name and not (n1:name/@nullFlavor)", [
new Text("n1:name")
, 
]),
], [
new Condition("pharm:asContent/pharm:containerPackagedMedicine/pharm:name and not (pharm:asContent/pharm:containerPackagedMedicine/pharm:name/@nullFlavor)", [
new Text("pharm:asContent/pharm:containerPackagedMedicine/pharm:name")
, 
])
, 
]), 
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
