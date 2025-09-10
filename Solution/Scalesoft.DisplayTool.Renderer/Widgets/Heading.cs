using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

public class Heading : Widget
{
    private readonly Widget[] m_content;
    private readonly HeadingSize m_size;
    private readonly string m_customClass;
    private readonly IdentifierSource? m_idSource;
    private readonly IdentifierSource? m_visualIdSource;

    public Heading(Widget[] content, HeadingSize size = HeadingSize.H1, string customClass = "", IdentifierSource? idSource = null, IdentifierSource? visualIdSource = null)
    {
        m_content = content;
        m_size = size;
        m_customClass = customClass;
        m_idSource = idSource;
        m_visualIdSource = visualIdSource ?? idSource;
    }

    public override async Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        var contentResult = await m_content.RenderConcatenatedResult(navigator, renderer, context);
        if (!contentResult.HasValue)
        {
            return contentResult;
        }

        var viewModel = new ViewModel
        {
            Content = contentResult.Content,
            Size = m_size,
            CustomClass = m_customClass,
        };
        HandleIds(context, navigator, viewModel, m_idSource, m_visualIdSource);
        var view = await renderer.RenderHeading(viewModel);
        return new RenderResult(view, contentResult.Errors);
    }

    public class ViewModel : ViewModelBase    {
        public required string Content { get; set; }
        
        public required HeadingSize Size { get; set; }
    }
}
