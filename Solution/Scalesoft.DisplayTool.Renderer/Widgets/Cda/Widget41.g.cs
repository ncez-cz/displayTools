using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class Widget41 : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new TableRow([
new TableCell([
new ChangeContext("n1:code", new Widget42()), 
],
TableCellType.Data), 
new TableCell([
new ConcatBuilder("n1:entryRelationship[@typeCode='MFST']/n1:observation", i=>[new Widget43(i)]), 
],
TableCellType.Data), 
new TableCell([
new ChangeContext("n1:participant[@typeCode='CSM']/n1:participantRole[@classCode='MANU']/n1:playingEntity[@classCode='MMAT']", new Widget44()), 
],
TableCellType.Data), 
new TableCell([
new WidgetWithVariables(new ShowIvlTsWidget(), [
new Variable("node", "n1:effectiveTime"),
]), 
],
TableCellType.Data), 
new TableCell([
new ChangeContext("n1:entryRelationship[@typeCode='MFST']/n1:observation/n1:entryRelationship[@typeCode='SUBJ']/n1:observation", new Widget45()), 
],
TableCellType.Data), 
new TableCell([
new ChangeContext("n1:entryRelationship[@typeCode='SUBJ']/n1:observation/n1:code[@code='82606-5']", new Widget47()), 
],
TableCellType.Data), 
new TableCell([
new ChangeContext("n1:entryRelationship[@typeCode='REFR']/n1:observation/n1:code[@code='33999-4']", new Widget46()), 
],
TableCellType.Data), 
new TableCell([
new ChangeContext("n1:entryRelationship[@typeCode='SUBJ']/n1:observation/n1:code[@code='66455-7']", new Widget48()), 
],
TableCellType.Data), 
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
