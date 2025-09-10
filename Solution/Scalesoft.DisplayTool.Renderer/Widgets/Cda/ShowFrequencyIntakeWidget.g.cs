using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class ShowFrequencyIntakeWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Choose([
new When("$medFrequencyIntakeType='TS' or substring-after($medFrequencyIntakeType, ':')='TS'", [
new WidgetWithVariables(new ShowTsWidget(), [
new Variable("node", "$medFrequencyIntake"),
]), 
]),
new When("$medFrequencyIntakeType='IVL_TS' or substring-after($medFrequencyIntakeType, ':')='IVL_TS'", [
new WidgetWithVariables(new ShowIvlTsWidget(), [
new Variable("node", "$medFrequencyIntake"),
]), 
]),
new When("$medFrequencyIntakeType='PIVL_TS' or substring-after($medFrequencyIntakeType, ':')='PIVL_TS'", [
new WidgetWithVariables(new ShowPivlTsWidget(), [
new Variable("node", "$medFrequencyIntake"),
]), 
]),
new When("$medFrequencyIntakeType='EIVL_TS' or substring-after($medFrequencyIntakeType, ':')='EIVL_TS'", [
new WidgetWithVariables(new ShowEivlTsWidget(), [
new Variable("node", "$medFrequencyIntake"),
]), 
]),
new When("$medFrequencyIntakeType='SXPR_TS' or substring-after($medFrequencyIntakeType, ':')='SXPR_TS'", [
new ConcatBuilder("$medFrequencyIntake/n1:comp", (i) => [
new WidgetWithVariables(new FrequencyCompWidget(), [
]), 
])
, 
]),
], [
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
