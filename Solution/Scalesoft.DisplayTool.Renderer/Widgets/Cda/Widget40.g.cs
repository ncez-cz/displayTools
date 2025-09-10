using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class Widget40 : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Choose([
new When("not(@nullFlavor)", [
    new ChangeContext("n1:entryRelationship[@typeCode='SUBJ']/n1:observation", new Widget41()),
]),
], [
new TableRow([
new TableCell([
new WidgetWithVariables(new ShowEHdsiNullFlavorWidget(), [
new Variable("code", "./@nullFlavor"),
]), 
],
TableCellType.Data, 5), 
]), 
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
