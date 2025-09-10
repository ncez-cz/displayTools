using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class FrequencyCompWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Variable("compType", "./@xsi:type"), 
new Variable("comp", "."), 
new WidgetWithVariables(new ShowFrequencyIntakeWidget(), [
new Variable("medFrequencyIntakeType", "$compType"),
new Variable("medFrequencyIntake", "$comp"),
]), 
new LineBreak(), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
