using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

public class CdaEhdsiDisplayLabel(string code) : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        navigator.Variables.TryGetValue(code, out var codeVal);
        if (codeVal is not string code1)
        {
            throw new InvalidOperationException("Expected string variables for reader languages");
        }

        var widget = new EhdsiDisplayLabel(code1);
        return widget.Render(navigator, renderer, context);
    }
}
