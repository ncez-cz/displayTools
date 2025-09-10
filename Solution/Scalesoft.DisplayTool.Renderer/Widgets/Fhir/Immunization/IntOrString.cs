using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Immunization;

public class IntOrString(string prefix) : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        var widget = new Choose([
            new When($"f:{prefix}PositiveInt", new AbsentDataTextCheck($"f:{prefix}PositiveInt", new Text())),
            new When($"f:{prefix}String", new AbsentDataTextCheck($"f:{prefix}String", new Text())),
        ]);

        return widget.Render(navigator, renderer, context);
    }
}