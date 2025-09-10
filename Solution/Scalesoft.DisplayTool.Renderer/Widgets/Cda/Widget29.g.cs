using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class Widget29 : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Variable("medDeviceImplant", "n1:participant[@typeCode='DEV']/n1:participantRole/n1:playingDevice/n1:code"), 
new Variable("medDeviceId", "n1:participant[@typeCode='DEV']/n1:participantRole/n1:id"), 
new Variable("medDeviceImplantDate", "n1:effectiveTime"), 
new Choose([
new When("not(n1:supply/@nullFlavor)", [
new TableRow([
new TableCell([
new WidgetWithVariables(new ShowEHdsiMedicalDeviceWidget(), [
new Variable("node", "$medDeviceImplant"),
]), 
],
TableCellType.Data), 
new TableCell([
new ChangeContext("n1:participant[@typeCode='DEV']/n1:participantRole/n1:id", new Widget30()), 
],
TableCellType.Data), 
new TableCell([
new WidgetWithVariables(new ShowIvlTsWidget(), [
new Variable("node", "$medDeviceImplantDate"),
]), 
],
TableCellType.Data), 
]), 
]),
], [
new TableRow([
new TableCell([
new WidgetWithVariables(new ShowEHdsiNullFlavorWidget(), [
new Variable("code", "n1:supply/@nullFlavor"),
]), 
],
TableCellType.Data, 2), 
]), 
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
