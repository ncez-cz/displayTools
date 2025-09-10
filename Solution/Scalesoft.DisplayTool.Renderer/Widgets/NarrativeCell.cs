using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

public class NarrativeCell(
    bool displayModal = true,
    TableCellType cellType = TableCellType.Data,
    string narrativePath = "f:text"
) : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var widget = new TableCell([
            new If(_ => displayModal,
                new NarrativeModal(narrativePath)
            ),
        ], cellType, containerClass: "narrative-cell");

        return widget.Render(navigator, renderer, context);
    }
}