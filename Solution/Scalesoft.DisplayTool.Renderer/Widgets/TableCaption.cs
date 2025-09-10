using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

public class TableCaption : Widget
{
    private readonly IEnumerable<Widget> m_content;
    private readonly IdentifierSource? m_idSource;
    private readonly IdentifierSource? m_visualIdSource;

    public TableCaption(IEnumerable<Widget> content, IdentifierSource? idSource = null, IdentifierSource? visualIdSource = null)
    {
        m_content = content;
        m_idSource = idSource;
        m_visualIdSource = visualIdSource ?? idSource;
    }

    public override async Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        var renderResult = await RenderInternal(navigator, renderer, m_content, context);
        if (renderResult.IsFatal)
        {
            return renderResult.Errors;
        }

        var validContents = renderResult.GetValidContents();
        var concatenatedResult = string.Join(string.Empty, validContents);
        var viewModel = new ViewModel { Content = concatenatedResult};
        HandleIds(context, navigator, viewModel, m_idSource, m_visualIdSource);
        var result = await renderer.RenderTableCaption(viewModel);
        return new RenderResult(result, renderResult.Errors);
    }
    
    public class ViewModel : ViewModelBase    {
        public required string Content { get; set; }
    }
}
