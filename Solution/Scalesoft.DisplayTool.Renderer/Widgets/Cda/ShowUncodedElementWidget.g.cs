using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class ShowUncodedElementWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Variable("refText", "$code"), 
new Variable("refAttrText", "//*[@ID=substring($refText,2)]"), 
new Variable("refAttrText1", "//*[@id=substring($refText,2)]"), 
new Variable("refAttrText3", "//*[@id=$refText]"), 
new Variable("refAttrText4", "//*[@ID=$refText]"), 
new Choose([
new When("$refAttrText", [
new Text("$refAttrText/.")
, 
]),
new When("$refAttrText1", [
new Text("$refAttrText1/.")
, 
]),
new When("$refAttrText3", [
new Text("$refAttrText3/.")
, 
]),
new When("$refAttrText4", [
new Text("$refAttrText4/.")
, 
]),
], [
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
