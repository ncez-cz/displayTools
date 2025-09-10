using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class Widget35 : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Variable("problemCondition", "."), 
new Variable("probOnSetDate", "../../../n1:effectiveTime/n1:low"), 
new Variable("diagnosisAssertionStatus", "../n1:entryRelationship[@typeCode='SUBJ']/n1:observation/n1:templateId[@root='1.3.6.1.4.1.12559.11.10.1.3.1.3.49']/../n1:value"), 
new Choose([
new When("@nullFlavor and not(@nullFlavor='OTH')", [
new TableRow([
new TableCell([
new WidgetWithVariables(new ShowEHdsiNullFlavorWidget(), [
new Variable("code", "./@nullFlavor"),
]), 
],
TableCellType.Data, 5), 
]), 
]),
], [
new TableRow([
new TableCell([
new WidgetWithVariables(new ShowEHdsiIllnessandDisorderWidget(), [
new Variable("node", "$problemCondition"),
]), 
new Choose([
new When("not($problemCondition/@nullFlavor)", [
new ConstantText(@" ("), 
new Text("$problemCondition/@code")
, 
new ConstantText(@")"), 
]),
new When("$problemCondition/@nullFlavor='OTH'", [
new TextContainer(TextStyle.Italic, [
new ConstantText(@" ("), 
new Text("$problemCondition/n1:translation/@code")
, 
new Condition("$problemCondition/n1:translation/@codeSystemName", [
new ConstantText(@" - "), 
new Text("$problemCondition/n1:translation/@codeSystemName")
, 
])
, 
new ConstantText(@")"), 
]), 
]),
], [
]), 
],
TableCellType.Data), 
new TableCell([
new WidgetWithVariables(new ShowTsWidget(), [
new Variable("node", "$probOnSetDate"),
]), 
],
TableCellType.Data), 
new TableCell([
new WidgetWithVariables(new ShowEHdsiCertaintyWidget(), [
new Variable("node", "$diagnosisAssertionStatus"),
]), 
],
TableCellType.Data), 
new TableCell([
new ConcatBuilder("../n1:entryRelationship/n1:act/n1:templateId[@root='1.3.6.1.4.1.12559.11.10.1.3.1.3.48']/..", (i) => [
new ChangeContext("n1:performer", new Widget24()), 
])
, 
],
TableCellType.Data), 
new TableCell([
new ConcatBuilder("../n1:reference[@typeCode='REFR']", (i) => [
new ChangeContext("n1:externalDocument", new Widget23()), 
])
, 
],
TableCellType.Data), 
]), 
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
