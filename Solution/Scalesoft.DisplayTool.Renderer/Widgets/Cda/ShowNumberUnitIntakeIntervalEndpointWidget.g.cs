using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class ShowNumberUnitIntakeIntervalEndpointWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Choose([
new When("$medUnitIntakeEndpoint/@value", [
new WidgetWithVariables(new ShowNumberUnitIntakeWidget(), [
new Variable("medUnitIntake", "$medUnitIntakeEndpoint/@value"),
new Variable("medUnitIntakeUnit", "$medUnitIntakeEndpoint/@unit"),
]), 
]),
new When("$medUnitIntakeGlobal/@value", [
new WidgetWithVariables(new ShowNumberUnitIntakeWidget(), [
new Variable("medUnitIntake", "$medUnitIntakeGlobal/@value"),
new Variable("medUnitIntakeUnit", "$medUnitIntakeGlobal/@unit"),
]), 
]),
new When("$medUnitIntakeEndpoint/@nullFlavor", [
new WidgetWithVariables(new ShowEHdsiNullFlavorWidget(), [
new Variable("code", "$medUnitIntakeEndpoint/@nullFlavor"),
]), 
]),
new When("$medUnitIntakeGlobal/@nullFlavor", [
new WidgetWithVariables(new ShowEHdsiNullFlavorWidget(), [
new Variable("code", "$medUnitIntakeGlobal/@nullFlavor"),
]), 
]),
], [
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
