using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class CodeableConceptIconTooltip(Widget title) : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var codings = navigator.SelectAllNodes("f:coding").ToList();
        var anyHasCode = codings.Any(x => x.EvaluateCondition("f:code/@value"));

        if (!anyHasCode)
        {
            return new CodeableConcept().Render(navigator, renderer, context);
        }

        var widget = new ConcatBuilder("f:coding", (_, _, nav) =>
        {
            var system = nav.SelectSingleNode("f:system/@value").Node?.Value ?? string.Empty;

            return
            [
                new EnumIconTooltip("f:code/@value", system, title, new Coding(hideSystem: true))
            ];
        });

        return widget.Render(navigator, renderer, context);
    }
}