using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

public class TableHead : Widget
{
    private readonly IList<TableRow> m_rows;
    private readonly IdentifierSource? m_idSource;
    private readonly IdentifierSource? m_visualIdSource;

    public TableHead(IList<TableRow> rows, IdentifierSource? idSource = null, IdentifierSource? visualIdSource = null)
    {
        m_rows = rows;
        m_idSource = idSource;
        m_visualIdSource = visualIdSource ?? idSource;
    }

    public override async Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        var renderResult = await RenderInternal(navigator, renderer, m_rows, context);
        if (renderResult.IsFatal)
        {
            return renderResult.Errors;
        }

        var validContents = renderResult.GetValidContents();
        var concatenatedResult = string.Join(string.Empty, validContents);
        var viewModel = new ViewModel { Content = concatenatedResult, };
        HandleIds(context, navigator, viewModel, m_idSource, m_visualIdSource);
        var result = await renderer.RenderTableHead(viewModel);
        return new RenderResult(result, renderResult.Errors);
    }

    public class ViewModel : ViewModelBase    {
        public required string Content { get; set; }
    }
}