using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class Widget26 : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Choose([
new When("not(./@nullFlavor)", [
new Choose([
new When("(n1:code/@code='no-known-procedures' or n1:code/@code='no-procedure-info')", [
new TableRow([
new TableCell([
new WidgetWithVariables(new ShowEHdsiAbsentOrUnknownProcedureWidget(), [
new Variable("node", "n1:code"),
]), 
],
TableCellType.Data, 2), 
]), 
]),
], [
new TableRow([
new TableCell([
new WidgetWithVariables(new ShowEHdsiProcedureWidget(), [
new Variable("node", "n1:code"),
]), 
],
TableCellType.Data), 
new TableCell([
new ChangeContext("n1:targetSiteCode", new Widget27()), 
],
TableCellType.Data), 
new TableCell([
new WidgetWithVariables(new ShowIvlTsWidget(), [
new Variable("node", "n1:effectiveTime"),
]), 
],
TableCellType.Data), 
]), 
]), 
]),
], [
new TableRow([
new TableCell([
new WidgetWithVariables(new ShowEHdsiNullFlavorWidget(), [
new Variable("code", "./@nullFlavor"),
]), 
],
TableCellType.Data, 3), 
]), 
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
