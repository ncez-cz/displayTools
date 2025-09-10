using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class Widget5 : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Variable("systolicValue", "n1:component/n1:observation/n1:templateId[@root='1.3.6.1.4.1.19376.1.5.3.1.4.13.2']/../n1:code[@code='8480-6']/../n1:value"), 
new Variable("diastolicValue", "n1:component/n1:observation/n1:templateId[@root='1.3.6.1.4.1.19376.1.5.3.1.4.13.2']/../n1:code[@code='8462-4']/../n1:value"), 
new Choose([
new When("not(n1:observation/@nullFlavor)", [
new TableRow([
new TableCell([
new WidgetWithVariables(new ShowTsWidget(), [
new Variable("node", "n1:effectiveTime"),
]), 
],
TableCellType.Data), 
new TableCell([
new WidgetWithVariables(new ShowPqWidget(), [
new Variable("node", "$systolicValue"),
]), 
],
TableCellType.Data), 
new TableCell([
new WidgetWithVariables(new ShowPqWidget(), [
new Variable("node", "$diastolicValue"),
]), 
],
TableCellType.Data), 
]), 
]),
], [
new TableRow([
new TableCell([
new WidgetWithVariables(new ShowEHdsiNullFlavorWidget(), [
new Variable("code", "n1:observation/@nullFlavor"),
]), 
],
TableCellType.Data, 3), 
]), 
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
