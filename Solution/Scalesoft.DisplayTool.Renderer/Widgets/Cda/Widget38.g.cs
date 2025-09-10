using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class Widget38 : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new TableRow([
new TableCell([
new WidgetWithVariables(new ShowIvlTsWidget(), [
new Variable("node", "n1:effectiveTime"),
]), 
],
TableCellType.Data), 
new TableCell([
new Text("n1:code/@displayName")
, 
],
TableCellType.Data), 
new TableCell([
new Choose([
new When("n1:value/@xsi:type='CD'", [
new Text("n1:value/@displayName")
, 
]),
new When("n1:value/@xsi:type='CE'", [
new Text("n1:value/@displayName")
, 
]),
new When("n1:value/@xsi:type='PQ'", [
new WidgetWithVariables(new ShowPqWidget(), [
new Variable("node", "n1:value"),
]), 
]),
], [
]), 
],
TableCellType.Data), 
new TableCell([
new WidgetWithVariables(new ShowPerformerWidget(), [
new Variable("node", "../../n1:performer"),
]), 
],
TableCellType.Data), 
new TableCell([
new WidgetWithVariables(new ShowAuthorWidget(), [
new Variable("node", "../../n1:author"),
]), 
],
TableCellType.Data), 
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
