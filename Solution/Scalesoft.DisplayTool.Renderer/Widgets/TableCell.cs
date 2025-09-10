using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

public class TableCell : Widget
{
    private readonly Widget[] m_content;
    private readonly TableCellType m_type;
    private readonly int? m_colspan;
    private readonly int? m_rowspan;
    private readonly IdentifierSource? m_idSource;
    private readonly string? m_optionalClass;
    private readonly string? m_containerClass;
    private readonly IdentifierSource? m_visualIdSource;

    public TableCell(
        Widget[] content,
        TableCellType type = TableCellType.Data,
        int? colspan = null,
        int? rowspan = null,
        IdentifierSource? idSource = null,
        IdentifierSource? visualIdSource = null,
        string? optionalClass = null,
        string? containerClass = null
    )
    {
        m_content = content;
        m_type = type;
        m_colspan = colspan;
        m_rowspan = rowspan;
        m_idSource = idSource;
        m_optionalClass = optionalClass;
        m_containerClass = containerClass;
        m_visualIdSource = visualIdSource ?? idSource;
    }

    public override async Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var renderResult = await RenderInternal(navigator, renderer, context, m_content);
        if (renderResult.IsFatal)
        {
            return renderResult.Errors;
        }

        string concatenatedResult;
        if (renderResult.IsNull)
        {
            concatenatedResult = string.Empty;
        }
        else
        {
            var validContents = renderResult.GetValidContents();
            concatenatedResult = string.Join(string.Empty, validContents);
        }

        var viewModel = new ViewModel
        {
            Content = concatenatedResult, Type = m_type, Colspan = m_colspan, Rowspan = m_rowspan, InputId = Id,
            CollapserIcon = IconHelper.GetInstance(SupportedIcons.CollapseTriangle, context),
            CustomClass = m_optionalClass,
            ContainerClass = m_containerClass,
        };
        HandleIds(context, navigator, viewModel, m_idSource, m_visualIdSource);
        var result = await renderer.RenderTableCell(viewModel);
        return new RenderResult(result, renderResult.Errors);
    }

    public class ViewModel : ViewModelBase
    {
        public required string Content { get; set; }

        public int? Colspan { get; set; }

        public int? Rowspan { get; set; }

        public TableCellType Type { get; set; }

        public required uint InputId { get; set; }

        public required string CollapserIcon { get; init; }

        public string? ContainerClass { get; set; }
    }
}