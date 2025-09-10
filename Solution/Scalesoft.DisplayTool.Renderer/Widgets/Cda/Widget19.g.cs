using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class Widget19 : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Variable("vaccination", "n1:consumable/n1:manufacturedProduct/n1:manufacturedMaterial/n1:code"), 
new Variable("vaccinationsBrandName", "n1:consumable/n1:manufacturedProduct/n1:manufacturedMaterial/n1:name"), 
new Variable("vaccinationsDate", "n1:effectiveTime"), 
new Variable("vaccinationsPosition", "n1:entryRelationship/n1:observation[@classCode='OBS'][@moodCode='EVN']/n1:code[@codeSystem='2.16.840.1.113883.6.1'][@code ='30973-2']"), 
new Variable("vaccinationMarketingAuthorizationHolder", "n1:consumable/n1:manufacturedProduct/n1:manufacturerOrganization"), 
new Variable("vaccinationBatchNumber", "n1:consumable/n1:manufacturedProduct/n1:manufacturedMaterial/n1:lotNumberText"), 
new Choose([
new When("not(./@nullFlavor)", [
new TableRow([
new TableCell([
new WidgetWithVariables(new ShowEHdsiVaccineWidget(), [
new Variable("node", "$vaccination"),
]), 
],
TableCellType.Data), 
new TableCell([
new Choose([
new When("$vaccinationsBrandName/@nullFlavor", [
new WidgetWithVariables(new ShowEHdsiNullFlavorWidget(), [
new Variable("code", "$vaccinationsBrandName/@nullFlavor"),
]), 
]),
], [
new Text("$vaccinationsBrandName")
, 
]), 
],
TableCellType.Data), 
new TableCell([
new WidgetWithVariables(new ShowTsWidget(), [
new Variable("node", "$vaccinationsDate"),
]), 
new ConstantText(@"
                         
                    "), 
],
TableCellType.Data), 
new TableCell([
new ConcatBuilder("n1:participant", (i) => [
new Choose([
new When("n1:participantRole/n1:code/@nullFlavor", [
new WidgetWithVariables(new ShowEHdsiNullFlavorWidget(), [
new Variable("code", "n1:participantRole/n1:code/@nullFlavor"),
]), 
]),
], [
new WidgetWithVariables(new ShowEHdsiIllnessandDisorderWidget(), [
new Variable("node", "n1:participantRole/n1:code"),
]), 
]), 
])
, 
],
TableCellType.Data), 
new TableCell([
new Choose([
new When("$vaccinationMarketingAuthorizationHolder/n1:name/@nullFlavor", [
new WidgetWithVariables(new ShowEHdsiNullFlavorWidget(), [
new Variable("code", "$vaccinationMarketingAuthorizationHolder/n1:name/@nullFlavor"),
]), 
]),
], [
new Text("$vaccinationMarketingAuthorizationHolder/n1:name")
, 
]), 
],
TableCellType.Data), 
new TableCell([
new Choose([
new When("$vaccinationsPosition/@nullFlavor", [
new WidgetWithVariables(new ShowEHdsiNullFlavorWidget(), [
new Variable("code", "$vaccinationsPosition/@nullFlavor"),
]), 
]),
], [
new Text("$vaccinationsPosition/../n1:value/@value")
, 
]), 
],
TableCellType.Data), 
new TableCell([
new Choose([
new When("$vaccinationBatchNumber/@nullFlavor", [
new WidgetWithVariables(new ShowEHdsiNullFlavorWidget(), [
new Variable("code", "$vaccinationBatchNumber/@nullFlavor"),
]), 
]),
], [
new Text("$vaccinationBatchNumber")
, 
]), 
],
TableCellType.Data), 
new TableCell([
new ConcatBuilder("n1:performer", (i) => [
new Choose([
new When("n1:assignedEntity/@nullFlavor", [
new WidgetWithVariables(new ShowEHdsiNullFlavorWidget(), [
new Variable("code", "n1:assignedEntity/@nullFlavor"),
]), 
]),
], [
new ConcatBuilder("n1:assignedEntity/n1:representedOrganization/n1:name", (i) => [
new Text("text()")
, 
])
, 
]), 
new ConstantText(@"
                             
                        "), 
])
, 
],
TableCellType.Data), 
new TableCell([
new ConcatBuilder("n1:performer", (i) => [
new Choose([
new When("n1:assignedEntity/@nullFlavor", [
new WidgetWithVariables(new ShowEHdsiNullFlavorWidget(), [
new Variable("code", "n1:assignedEntity/@nullFlavor"),
]), 
]),
], [
new WidgetWithVariables(new ShowNameWidget(true), [
new Variable("name", "n1:assignedEntity/n1:assignedPerson/n1:name"),
]), 
]), 
new ConstantText(@"
                             
                        "), 
])
, 
],
TableCellType.Data), 
new TableCell([
new ConcatBuilder("n1:performer", (i) => [
new Choose([
new When("n1:assignedEntity/@nullFlavor", [
new WidgetWithVariables(new ShowEHdsiNullFlavorWidget(), [
new Variable("code", "n1:assignedEntity/@nullFlavor"),
]), 
]),
], [
new Text("n1:assignedEntity/n1:representedOrganization/n1:addr")
, 
]), 
new ConstantText(@"
                             
                        "), 
])
, 
],
TableCellType.Data), 
new TableCell([
new Choose([
new When("n1:statusCode/@code='completed'", [
    new Icon(SupportedIcons.Check)
]),
], [
    new Icon(SupportedIcons.Cross)
]), 
],
TableCellType.Data), 
]), 
]),
], [
new TableRow([
new TableCell([
new WidgetWithVariables(new ShowEHdsiNullFlavorWidget(), [
new Variable("code", "n1:substanceAdministration/@nullFlavor"),
]), 
],
TableCellType.Data, 3), 
]), 
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
