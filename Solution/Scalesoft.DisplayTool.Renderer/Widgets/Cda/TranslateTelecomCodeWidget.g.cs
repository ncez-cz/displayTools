using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class TranslateTelecomCodeWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Choose([
new When("$code='tel'", [
new ConstantText(@"Tel"), 
]),
new When("$code='fax'", [
new ConstantText(@"Fax"), 
]),
new When("$code='http'", [
new ConstantText(@"Web"), 
]),
new When("$code='mailto'", [
new ConstantText(@"Mail"), 
]),
], [
new Text("$code")
, 
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
