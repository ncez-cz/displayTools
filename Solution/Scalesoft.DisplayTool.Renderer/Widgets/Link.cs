using System.Diagnostics.CodeAnalysis;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

public class Link : Widget
{
    private readonly IdentifierSource? m_idSource;
    private readonly IdentifierSource? m_visualIdSource;
    private Widget Content { get; }
    private string? HrefSimple { get; }
    private Widget? HrefWidget { get; }

    [MemberNotNullWhen(true, nameof(HrefSimple))]
    [MemberNotNullWhen(false, nameof(HrefWidget))]
    private bool BySimpleValue { get; }

    private string? DownloadInfo { get; }

    private string? OptionalClass { get; }

    public Link(
        Widget content,
        string hrefSimple,
        IdentifierSource? idSource = null,
        IdentifierSource? visualIdSource = null,
        string? downloadInfo = null,
        string? optionalClass = null
    )
    {
        m_idSource = idSource;
        m_visualIdSource = visualIdSource ?? idSource;
        Content = content;
        HrefSimple = hrefSimple;
        BySimpleValue = true;
        DownloadInfo = downloadInfo;
        OptionalClass = optionalClass;
    }

    public Link(
        Widget content,
        Widget hrefSimple,
        IdentifierSource? idSource = null,
        IdentifierSource? visualIdSource = null,
        string? downloadInfo = null,
        string? optionalClass = null
    )
    {
        m_idSource = idSource;
        m_visualIdSource = visualIdSource ?? idSource;
        Content = content;
        HrefWidget = hrefSimple;
        BySimpleValue = false;
        DownloadInfo = downloadInfo;
        OptionalClass = optionalClass;
    }

    public override async Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        Widget[] content;
        if (BySimpleValue)
        {
            content = [Content];
        }
        else
        {
            content = [Content, HrefWidget];
        }

        var renderResult = await RenderInternal(navigator, renderer, context, content);
        if (renderResult.IsFatal)
        {
            return renderResult.Errors;
        }

        var text = renderResult.GetContent(Content) ?? string.Empty;

        ViewModel viewModel;
        if (BySimpleValue)
        {
            viewModel = new ViewModel
            {
                Text = text,
                Href = HrefSimple,
                Download = DownloadInfo,
                CustomClass = OptionalClass,
            };
        }
        else
        {
            viewModel = new ViewModel
            {
                Text = text,
                Href = renderResult.GetContent(HrefWidget) ?? string.Empty,
                Download = DownloadInfo,
                CustomClass = OptionalClass,
            };
        }

        HandleIds(context, navigator, viewModel, m_idSource, m_visualIdSource);

        return await renderer.RenderLink(viewModel);
    }

    public class ViewModel : ViewModelBase
    {
        public required string Text { get; set; }

        public required string Href { get; set; }

        public string? Download { get; set; }
    }
}