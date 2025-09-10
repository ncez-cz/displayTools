using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

public class Card(
    Widget? title,
    Widget body,
    Func<Severity?> getSeverity,
    string? optionalClass = null,
    IdentifierSource? idSource = null,
    IdentifierSource? visualIdSource = null,
    Widget? footer = null
)
    : Widget
{
    public Card(
        Widget? title,
        Widget body,
        string? optionalClass = null,
        IdentifierSource? idSource = null,
        IdentifierSource? visualIdSource = null,
        Widget? footer = null,
        Severity? severity = null
    ) : this(title, body, () => severity, optionalClass, idSource, visualIdSource, footer)
    {
    }

    private readonly IdentifierSource? m_visualIdSource = visualIdSource ?? idSource;

    public override async Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        List<Widget> childWidgets = [body];
        if (title != null)
        {
            childWidgets.Add(title);
        }

        if (footer != null)
        {
            childWidgets.Add(footer);
        }

        var childWidgetsArray = childWidgets.ToArray();

        var childrenResult = await RenderInternal(navigator, renderer, context, childWidgetsArray);

        if (childrenResult.IsFatal)
        {
            return childrenResult.Errors;
        }

        var titleContent = title == null ? null : childrenResult.GetContent(title);
        var bodyContent = childrenResult.GetContent(body);
        var footerContent = footer == null ? null : childrenResult.GetContent(footer);

        var viewModel = new ViewModel
        {
            Title = titleContent,
            Body = bodyContent,
            Footer = footerContent,
            HideTitle = title == null,
            HideFooter = footer == null,
            CustomClass = optionalClass,
            Severity = getSeverity(),
        };
        
        HandleIds(context, navigator, viewModel, idSource, m_visualIdSource);
        var view = await renderer.RenderCard(viewModel);
        return new RenderResult(view, childrenResult.Errors);
    }

    public class ViewModel : ViewModelBase
    {
        public string? Title { get; set; }

        public string? Body { get; set; }

        public string? Footer { get; set; }
        
        public bool HideTitle { get; set; }
        
        public bool HideFooter { get; set; }
        
        public Severity? Severity { get; set; }
    }
}