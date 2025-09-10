using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class ShowCodeWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Variable("this-codeSystem", [ 
new Text("$code/@codeSystem")
, 
]), 
new Variable("this-code", [ 
new Text("$code/@code")
, 
]), 
new Choose([
new When("$code/n1:originalText", [
new Text("$code/n1:originalText")
, 
]),
new When("$code/@displayName", [
new Text("$code/@displayName")
, 
]),
], [
new Text("$this-code")
, 
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
