using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class ShowCodedElementWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Choose([
new When("not($node/@nullFlavor)", [
new Choose([
new When("$node/@code", [
new WidgetWithVariables(new ShowCodeValueWidget(), [
new Variable("code", "$node/@code"),
new Variable("xmlFile", "$xmlFile"),
new Variable("codeSystem", "$codeSystem"),
]), 
]),
], [
new Condition("$node/n1:originalText/n1:reference/@value", [
new WidgetWithVariables(new ShowUncodedElementWidget(), [
new Variable("code", "$node/n1:originalText/n1:reference/@value"),
]), 
])
, 
]), 
]),
], [
new WidgetWithVariables(new HandleNullFlavorWidget(), [
new Variable("node", "$node"),
]), 
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
