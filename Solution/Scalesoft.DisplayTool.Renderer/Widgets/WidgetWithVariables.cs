using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

public class WidgetWithVariables(Widget widget, Variable[] variables) : Widget
{
    public override async Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context)
    {
        // Create a copy of the original navigator to avoid polluting variables of siblings.
        var newNavigator = navigator.Clone();
        
        foreach (var variable in variables)
        {
            await variable.Render(newNavigator, renderer, context);
        }

        var rendered = await widget.Render(newNavigator, renderer, context);
        
        return rendered;
    }
}
