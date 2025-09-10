using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

public class NarrativeCollapser(string narrativeXPath = "f:text") : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var widget = new HideableDetails(ContainerType.Div,
            new Collapser(
                [new DisplayLabel(LabelCodes.OriginalNarrative)],
                [],
                [new Narrative(narrativeXPath)],
                true,
                customClass: "narrative-print-collapser"
            )
        );
        return widget.Render(navigator, renderer, context);
    }
}