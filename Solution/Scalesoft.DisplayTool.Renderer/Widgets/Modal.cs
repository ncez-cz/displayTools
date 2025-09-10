using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

public class Modal(
    Widget? title,
    Widget content,
    SupportedIcons openButtonIcon,
    string? openButtonCustomClass = null
) : Widget
{
    public override async Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var closeModalCross = new Icon(SupportedIcons.Cross, "corner-close-cross");
        var renderedOpenButtonIcon = IconHelper.GetInstance(openButtonIcon, context);
        var closeButton = new Button("Zavřít", style: ButtonStyle.Outline, cssClass: "m-0",
            additionalAttributes: new Dictionary<string, object>
            {
                { "data-bs-dismiss", "modal" }
            }
        );

        List<Widget> widgets = [content, closeModalCross, closeButton];

        if (title != null)
        {
            widgets.Add(title);
        }

        var result = await RenderInternal(navigator, renderer, context, widgets.ToArray());

        var renderedContent = result.GetContent(content);

        if (result.IsNull || renderedContent == null)
        {
            return RenderResult.NullResult;
        }

        var viewModel = new ViewModel
        {
            Content = renderedContent,
            Title = title != null ? result.GetContent(title) : null,
            CloseButton = result.GetContent(closeButton),
            OpenButtonIcon = renderedOpenButtonIcon,
            InputId = Id,
            OpenButtonClass = openButtonCustomClass,
            CornerCloseCross = result.GetContent(closeModalCross) ?? string.Empty
        };

        return await renderer.RenderModal(viewModel);
    }

    public class ViewModel : ViewModelBase
    {
        public required string? CloseButton { get; set; }
        public required string OpenButtonIcon { get; set; }
        public string? OpenButtonClass { get; set; }
        public required string Content { get; set; }
        public required string? Title { get; set; }
        public required uint InputId { get; set; }
        public required string CornerCloseCross { get; set; }
    }
}