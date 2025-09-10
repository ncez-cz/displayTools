using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

public class Tooltip : Widget
{
    private readonly IList<Widget> m_relatedWidgets;
    private readonly IList<Widget> m_importantTooltipContent;
    private readonly IList<Widget>? m_additionalTooltipContent;
    private readonly IdentifierSource? m_idSource;
    private readonly IdentifierSource? m_visualIdSource;
    private readonly Icon m_icon;

    public Tooltip(
        IList<Widget> relatedWidgets,
        IList<Widget> importantTooltipContent,
        IList<Widget>? additionalTooltipContent = null,
        IdentifierSource? idSource = null,
        IdentifierSource? visualIdSource = null,
        Icon? icon = null
    )
    {
        m_relatedWidgets = relatedWidgets;
        m_importantTooltipContent = importantTooltipContent;
        m_additionalTooltipContent = additionalTooltipContent;
        m_idSource = idSource;
        m_visualIdSource = visualIdSource ?? idSource;
        m_icon = icon ?? new Icon(SupportedIcons.TooltipInfo);
    }

    public override async Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var additionalTooltipContentArray = m_additionalTooltipContent ?? [];
        var renderResult = await RenderInternal(
            navigator,
            renderer,
            context,
            [..m_relatedWidgets, ..m_importantTooltipContent, ..additionalTooltipContentArray, m_icon]
        );
        if (renderResult.IsFatal)
        {
            return renderResult.Errors;
        }

        var relatedWidgetsText = string.Join(string.Empty, renderResult.GetValidContents(m_relatedWidgets));
        var importantTooltipText = string.Join(string.Empty, renderResult.GetValidContents(m_importantTooltipContent));
        var additionalTooltipText =
            string.Join(string.Empty, renderResult.GetValidContents(additionalTooltipContentArray));
        var iconRendered = renderResult.GetContent(m_icon) ?? string.Empty;
        var viewModel = new ViewModel
        {
            RelatedWidgets = relatedWidgetsText,
            ImportantTooltipText = importantTooltipText,
            AdditionalTooltipText = additionalTooltipText,
            Icon = iconRendered
        };
        HandleIds(context, navigator, viewModel, m_idSource, m_visualIdSource);

        return await renderer.RenderTooltip(viewModel);
    }

    public class ViewModel : ViewModelBase
    {
        public required string RelatedWidgets { get; set; }

        public required string ImportantTooltipText { get; set; }

        public string? AdditionalTooltipText { get; set; }

        public required string Icon { get; set; }
    }
}