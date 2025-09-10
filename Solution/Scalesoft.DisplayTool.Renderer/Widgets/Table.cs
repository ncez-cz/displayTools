using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

public class Table : Widget
{
    private readonly IList<Widget> m_rows;
    private readonly bool m_striped;
    private readonly IdentifierSource? m_idSource;
    private readonly IdentifierSource? m_visualIdSource;

    public Table(IList<Widget> rows, bool striped = false, IdentifierSource? idSource = null, IdentifierSource? visualIdSource = null)
    {
        m_rows = rows;
        m_striped = striped;
        m_idSource = idSource;
        m_visualIdSource = visualIdSource ?? idSource;
    }

    // table can have colgroups and xslt elements like choose, thus accepting only rows is not possible, also thead/tbody widgets should be considered
    public override async Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        var renderResult = await RenderInternal(navigator, renderer, m_rows, context);
        if (renderResult.IsFatal)
        {
            return renderResult.Errors;
        }

        var results = renderResult.GetValidContents();
        var viewModel = new ViewModel { Rows = results, Striped = m_striped };
        HandleIds(context, navigator, viewModel, m_idSource, m_visualIdSource);
        
        return await renderer.RenderTable(viewModel);
    }

    public class ViewModel : ViewModelBase    {
        public required List<string> Rows { get; set; }
        public bool Striped { get; set; }
    }
}