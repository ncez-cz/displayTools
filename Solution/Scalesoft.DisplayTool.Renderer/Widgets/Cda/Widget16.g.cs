using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class Widget16 : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new TableRow([
new TableCell([
new WidgetWithVariables(new ShowIvlTsWidget(), [
new Variable("node", "../../n1:effectiveTime"),
]), 
],
TableCellType.Data), 
new TableCell([
new Text("n1:code/@displayName")
, 
],
TableCellType.Data), 
new TableCell([
new WidgetWithVariables(new ShowIvlTsWidget(), [
new Variable("node", "n1:effectiveTime"),
]), 
],
TableCellType.Data), 
new TableCell([
new Text("n1:value/@displayName")
, 
],
TableCellType.Data), 
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
