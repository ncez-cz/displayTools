using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class ShowNumberUnitIntakeWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Text("$medUnitIntake")
, 
new Choose([
new When("not($medUnitIntakeUnit) or $medUnitIntakeUnit='1'", [
new ConstantText(@" "), 
new WidgetWithVariables(new ShowEHdsiDisplayLabelWidget(), [
new Variable("code", "'77'"),
]), 
]),
], [
new ConstantText(@" "), 
new WidgetWithVariables(new SupportUcumAnnotationsWidget(), [
new Variable("value", "$medUnitIntakeUnit"),
]), 
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
