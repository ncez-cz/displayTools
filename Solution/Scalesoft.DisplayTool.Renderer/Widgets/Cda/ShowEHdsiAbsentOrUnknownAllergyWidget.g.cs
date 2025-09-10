using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class ShowEHdsiAbsentOrUnknownAllergyWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Tooltip([], [
new Choose([
new When("($node/@code='no-allergy-info')", [
new Container([
new WidgetWithVariables(new ShowEHdsiDisplayLabelWidget(), [
new Variable("code", "'133'"),
]), 
], ContainerType.Span), 
]),
new When("($node/@code='no-known-allergies')", [
new Container([
new WidgetWithVariables(new ShowEHdsiDisplayLabelWidget(), [
new Variable("code", "'134'"),
]), 
], ContainerType.Span), 
]),
new When("($node/@code='no-known-medication-allergies')", [
new Container([
new WidgetWithVariables(new ShowEHdsiDisplayLabelWidget(), [
new Variable("code", "'135'"),
]), 
], ContainerType.Span), 
]),
new When("($node/@code='no-known-environmental-allergies')", [
new Container([
new WidgetWithVariables(new ShowEHdsiDisplayLabelWidget(), [
new Variable("code", "'136'"),
]), 
], ContainerType.Span), 
]),
new When("($node/@code='no-known-food-allergies')", [
new Container([
new WidgetWithVariables(new ShowEHdsiDisplayLabelWidget(), [
new Variable("code", "'137'"),
]), 
], ContainerType.Span), 
]),
], [
]), 
new WidgetWithVariables(new ShowCodedElementWidget(), [
new Variable("node", "$node"),
new Variable("xmlFile", "'1.3.6.1.4.1.12559.11.10.1.3.1.42.47.xml'"),
new Variable("codeSystem", "'2.16.840.1.113883.5.1150.1'"),
]), 
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
