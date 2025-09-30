using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

public class TableRow : Widget
{
    private readonly IList<Widget> m_cells;
    private readonly StructuredDetails? m_collapsibleRowContents;
    private readonly IdentifierSource? m_idSource;
    private readonly string? m_optionalClass;
    private readonly IdentifierSource? m_visualIdSource;

    /// <summary>
    ///     Represents a table row with optional collapsible content.
    /// </summary>
    /// <param name="cells">List of widgets representing the row cells.</param>
    /// <param name="collapsibleRowContents">
    ///     In dictionary where the key is the <b>title for collapser</b> (e.g., "Narrative text") and the value is a widget
    ///     that will be displayed
    ///     within the collapser.
    /// </param>
    public TableRow(
        IList<Widget> cells,
        StructuredDetails? collapsibleRowContents = null,
        IdentifierSource? idSource = null,
        IdentifierSource? visualIdSource = null,
        string? optionalClass = null
    )
    {
        m_cells = cells;
        m_collapsibleRowContents = collapsibleRowContents;
        m_idSource = idSource;
        m_optionalClass = optionalClass;
        m_visualIdSource = visualIdSource ?? idSource;
    }

    // not only table cell widgets are expected, but also other xsl elements, like choose
    public override async Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        // render cells first since they can modify collapsible content
        var cellsRenderResult = await RenderInternal(navigator, renderer, context, [..m_cells]);
        if (cellsRenderResult.IsFatal)
        {
            return cellsRenderResult.Errors;
        }

        var collapsibleRowContentsWithCollapserWrapper = m_collapsibleRowContents?.Build();

        var collapsibleRowContentsArray = collapsibleRowContentsWithCollapserWrapper ?? [];
        var collapsibleDataRenderResult =
            await RenderInternal(navigator, renderer, context, [..collapsibleRowContentsArray]);
        if (collapsibleDataRenderResult.IsFatal)
        {
            return collapsibleDataRenderResult.Errors;
        }

        var cellsResult = string.Join(string.Empty,
            m_cells.Select(cell => cellsRenderResult.GetContent(cell)).WhereNotNull());
        var collapsibleContentResult = string.Join(string.Empty,
            collapsibleRowContentsArray.Select(content =>
                collapsibleDataRenderResult.GetContent(content) ?? string.Empty));

        var viewModel = new ViewModel
        {
            InputId = Id,
            Content = cellsResult,
            CollapsibleRowContent = collapsibleContentResult,
            CustomClass = m_optionalClass,
            CollapsibleRowCustomCss = m_collapsibleRowContents?.OptionalCss,
        };
        HandleIds(context, navigator, viewModel, m_idSource, m_visualIdSource);

        return await renderer.RenderTableRow(viewModel);
    }

    public class ViewModel : ViewModelBase
    {
        public required uint InputId { get; set; }

        public required string Content { get; set; }

        public string? CollapsibleRowContent { get; set; }

        public string? CollapsibleRowCustomCss { get; set; }
    }
}