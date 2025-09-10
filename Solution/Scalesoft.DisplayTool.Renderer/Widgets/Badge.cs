using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

public class Badge(
    Widget content,
    Severity? severity = null,
    IdentifierSource? idSource = null,
    IdentifierSource? visualIdSource = null,
    string? optionalClass = null
)
    : Widget
{
    private readonly IdentifierSource? m_visualIdSource = visualIdSource ?? idSource;

    public override async Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var childrenResult = await RenderInternal(navigator, renderer, context, content);

        if (childrenResult.IsFatal)
        {
            return childrenResult.Errors;
        }

        var badgeContent = childrenResult.GetContent(content) ?? string.Empty;

        var viewModel = new ViewModel
        {
            Text = badgeContent,
            Severity = severity,
            CustomClass = optionalClass,
        };

        HandleIds(context, navigator, viewModel, idSource, m_visualIdSource);
        var view = await renderer.RenderBadge(viewModel);

        return new RenderResult(view, childrenResult.Errors);
    }

    public class ViewModel : ViewModelBase
    {
        public required string Text { get; set; }

        public Severity? Severity { get; set; }
    }
}