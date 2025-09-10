using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

public class TableBody : Widget
{
    private readonly IList<Widget> m_cells;
    private readonly IdentifierSource? m_idSource;
    private readonly string? m_optionalClass;
    private readonly IdentifierSource? m_visualIdSource;

    public TableBody(
        IList<Widget> cells,
        IdentifierSource? idSource = null,
        IdentifierSource? visualIdSource = null,
        string? optionalClass = null
    )
    {
        m_cells = cells;
        m_idSource = idSource;
        m_optionalClass = optionalClass;
        m_visualIdSource = visualIdSource ?? idSource;
    }

    public override async Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var renderResult = await RenderInternal(navigator, renderer, m_cells, context);
        if (renderResult.IsFatal)
        {
            return renderResult.Errors;
        }

        var validContents = renderResult.GetValidContents();
        var concatenatedResult = string.Join(string.Empty, validContents);
        var viewModel = new ViewModel { Content = concatenatedResult, CustomClass = m_optionalClass };
        HandleIds(context, navigator, viewModel, m_idSource, m_visualIdSource);
        var result = await renderer.RenderTableBody(viewModel);

        return new RenderResult(result, renderResult.Errors);
    }

    public class ViewModel : ViewModelBase
    {
        public required string Content { get; set; }
    }
}