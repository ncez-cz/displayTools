using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class BasicCdaHeaderWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Variable("creationDate", "/n1:ClinicalDocument/n1:effectiveTime"), 
new Variable("lastUpdate", "/n1:ClinicalDocument/n1:documentationOf/n1:serviceEvent/n1:effectiveTime/n1:high"), 
new Variable("documentLanguageCode", "/n1:ClinicalDocument/n1:languageCode"), 
new Table([
new TableBody([
new TableRow([
new TableCell([
new WidgetWithVariables(new ShowEHdsiDisplayLabelWidget(), [
new Variable("code", "'131'"),
]), 
],
TableCellType.Header), 
new TableCell([
new WidgetWithVariables(new ShowTsWidget(), [
new Variable("node", "$creationDate"),
]), 
],
TableCellType.Data), 
new TableCell([
new WidgetWithVariables(new ShowEHdsiDisplayLabelWidget(), [
new Variable("code", "'132'"),
]), 
],
TableCellType.Header), 
new TableCell([
new WidgetWithVariables(new ShowTsWidget(), [
new Variable("node", "$lastUpdate"),
]), 
],
TableCellType.Data), 
new TableCell([
new WidgetWithVariables(new ShowEHdsiDisplayLabelWidget(), [
new Variable("code", "'117'"),
]), 
],
TableCellType.Header), 
new TableCell([
new WidgetWithVariables(new ShowEHdsiLanguageWidget(), [
new Variable("node", "$documentLanguageCode"),
]), 
],
TableCellType.Data), 
]), 
]), 
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
