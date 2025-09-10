using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

public class TextContainer : Widget
{
    private readonly TextStyle m_style;
    private readonly IList<Widget> m_content;
    private readonly IdentifierSource? m_idSource;
    private readonly IdentifierSource? m_visualIdSource;
    private readonly string? m_optionalClass;

    public TextContainer(
        TextStyle style,
        IList<Widget> content,
        IdentifierSource? idSource = null,
        IdentifierSource? visualIdSource = null,
        string? optionalClass = null
    )
    {
        m_style = style;
        m_content = content;
        m_optionalClass = optionalClass;
        m_idSource = idSource;
        m_visualIdSource = visualIdSource ?? idSource;
    }

    public TextContainer(
        TextStyle style,
        Widget content,
        IdentifierSource? idSource = null,
        IdentifierSource? visualIdSource = null,
        string? optionalClass = null
    )
        : this(style, [content], idSource, visualIdSource, optionalClass)
    {
    }

    public override async Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var contentResult = await m_content.RenderConcatenatedResult(navigator, renderer, context);
        if (!contentResult.HasValue)
        {
            return contentResult;
        }

        var viewModel = new ViewModel
        {
            Content = contentResult.Content,
            Style = m_style,
            CustomClass = m_optionalClass,
        };
        HandleIds(context, navigator, viewModel, m_idSource, m_visualIdSource);

        var view = await renderer.RenderTextContainer(viewModel);
        return new RenderResult(view, contentResult.Errors);
    }

    public class ViewModel : ViewModelBase
    {
        public required string Content { get; set; }

        public required TextStyle Style { get; set; }
    }
}