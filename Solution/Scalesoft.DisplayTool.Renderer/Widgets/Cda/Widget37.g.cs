using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class Widget37 : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new TableRow([
new TableCell([
new WidgetWithVariables(new ShowTsWidget(), [
new Variable("node", "n1:code[@code='34530-6']/../n1:effectiveTime"),
]), 
],
TableCellType.Data), 
new TableCell([
new Text("n1:code[@code='34530-6']/@displayName")
, 
],
TableCellType.Data), 
new TableCell([
new WidgetWithVariables(new ShowEHdsiBloodGroupWidget(), [
new Variable("node", "n1:code[@code='34530-6']/../n1:value"),
]), 
],
TableCellType.Data), 
new TableCell([
],
TableCellType.Data), 
new TableCell([
],
TableCellType.Data), 
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
