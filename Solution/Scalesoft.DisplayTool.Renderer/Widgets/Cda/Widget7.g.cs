using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class Widget7 : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new TableRow([
new TableCell([
],
TableCellType.Data), 
new TableCell([
new WidgetWithVariables(new ShowIvlTsWidget(), [
new Variable("node", "../n1:effectiveTime"),
]), 
],
TableCellType.Data), 
new TableCell([
new Choose([
new When("not(../n1:value/@nullFlavor)", [
new WidgetWithVariables(new ShowEHdsiCurrentPregnancyStatusWidget(), [
new Variable("node", "../n1:value"),
]), 
]),
], [
new WidgetWithVariables(new ShowEHdsiNullFlavorWidget(), [
new Variable("code", "../n1:value/@nullFlavor"),
]), 
]), 
],
TableCellType.Data), 
new TableCell([
new WidgetWithVariables(new ShowTsWidget(), [
new Variable("node", "../n1:entryRelationship[@typeCode='COMP']/n1:observation/n1:value"),
]), 
],
TableCellType.Data), 
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
