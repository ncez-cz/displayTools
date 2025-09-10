using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class Widget32 : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Variable("medDose", "n1:consumable/n1:manufacturedProduct/n1:manufacturedMaterial/pharm:formCode"), 
new Variable("medUnitIntake", "n1:doseQuantity"), 
new Variable("medFrequencyIntake", "n1:effectiveTime[2]"), 
new Variable("medFrequencyIntakeType", "n1:effectiveTime[2]/@xsi:type"), 
new Variable("medRouteAdministration", "n1:routeCode"), 
new Variable("medRegimen", "n1:effectiveTime[1][@xsi:type='IVL_TS' or substring-after(@xsi:type, ':')='IVL_TS']"), 
new Variable("medCode", "n1:code/@code"), 
new Variable("medReason", "n1:entryRelationship[@typeCode='RSON']"), 
new Choose([
new When("not(./@nullFlavor)", [
new Condition("not($medCode='no-known-medications' or $medCode='no-medication-info')", [
new Variable("backgroundColor", "'#E6F2FF'"), 
new ConcatBuilder("n1:consumable/n1:manufacturedProduct/n1:manufacturedMaterial", (i) => [
new TableBody([
new TableRow([
new TableCell([
new TextContainer(TextStyle.Bold, [
new WidgetWithVariables(new ShowMedicinalProductWidget(), [
]), 
]), 
],
TableCellType.Data), 
new TableCell([
new ConcatBuilder("pharm:ingredient[@classCode='ACTI']", (i) => [
new Condition($"{i+1}=1", [
new WidgetWithVariables(new ShowActiveIngredientWidget(), [
]), 
])
, 
])
, 
],
TableCellType.Data), 
new TableCell([
new Choose([
new When("not(pharm:ingredient[@classCode='ACTI'])", [
new Text("pharm:desc")
, 
]),
], [
new ConcatBuilder("pharm:ingredient[@classCode='ACTI']", (i) => [
new Condition($"{i+1}=1", [
new Variable("medStrength", "pharm:quantity"), 
new WidgetWithVariables(new ShowStrengthWidget(), [
new Variable("node", "$medStrength"),
]), 
])
, 
])
, 
]), 
],
TableCellType.Data), 
new TableCell([
new WidgetWithVariables(new ShowEHdsiDoseFormWidget(), [
new Variable("node", "$medDose"),
]), 
],
TableCellType.Data), 
new TableCell([
new WidgetWithVariables(new ShowIvlPqWidget(), [
new Variable("node", "$medUnitIntake"),
]), 
],
TableCellType.Data), 
new TableCell([
new Choose([
new When("not ($medFrequencyIntake/@nullFlavor)", [
new WidgetWithVariables(new ShowFrequencyIntakeWidget(), [
new Variable("medFrequencyIntakeType", "$medFrequencyIntakeType"),
new Variable("medFrequencyIntake", "$medFrequencyIntake"),
]), 
]),
], [
new WidgetWithVariables(new ShowEHdsiNullFlavorWidget(), [
new Variable("code", "$medFrequencyIntake/@nullFlavor"),
]), 
]), 
],
TableCellType.Data), 
new TableCell([
new WidgetWithVariables(new ShowEHdsiRouteOfAdministrationWidget(), [
new Variable("node", "$medRouteAdministration"),
]), 
],
TableCellType.Data), 
new TableCell([
new WidgetWithVariables(new ShowIvlTsWidget(), [
new Variable("node", "$medRegimen"),
]), 
],
TableCellType.Data), 
new TableCell([
new ConcatBuilder("$medReason", (i) => [
new Choose([
new When("n1:observation", [
new WidgetWithVariables(new ShowEHdsiIllnessandDisorderWidget(), [
new Variable("node", "n1:observation/n1:value"),
]), 
]),
], [
new WidgetWithVariables(new ShowIdWidget(), [
new Variable("id", "n1:act/n1:id"),
]), 
]), 
])
, 
],
TableCellType.Data), 
]), 
new ConcatBuilder("pharm:ingredient[@classCode='ACTI']", (i) => [
new Condition($"{i+1}!=1", [
new TableRow([
new TableCell([
],
TableCellType.Data), 
new TableCell([
new WidgetWithVariables(new ShowActiveIngredientWidget(), [
]), 
],
TableCellType.Data), 
new TableCell([
new Variable("medStrength", "pharm:quantity"), 
new WidgetWithVariables(new ShowStrengthWidget(), [
new Variable("node", "$medStrength"),
]), 
],
TableCellType.Data), 
new TableCell([
],
TableCellType.Data), 
new TableCell([
],
TableCellType.Data), 
new TableCell([
],
TableCellType.Data), 
new TableCell([
],
TableCellType.Data), 
new TableCell([
],
TableCellType.Data), 
new TableCell([
],
TableCellType.Data), 
]), 
])
, 
])
, 
]), 
])
, 
])
, 
]),
], [
new TableRow([
new TableCell([
new WidgetWithVariables(new ShowEHdsiNullFlavorWidget(), [
new Variable("code", "./@nullFlavor"),
]), 
],
TableCellType.Data, 8), 
]), 
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
