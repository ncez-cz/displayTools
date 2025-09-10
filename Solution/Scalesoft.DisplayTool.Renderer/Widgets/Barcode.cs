using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;
using ZXing;
using ZXing.Common;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

public class Barcode : Widget
{
    private readonly Widget? m_widgetContent;
    private readonly string? m_simpleContent;
    private readonly string m_optionalInnerClass;
    private readonly string m_optionalOuterClass;
    private readonly int m_width;
    private readonly int m_height;
    private readonly int m_margin;
    private readonly IdentifierSource? m_idSource;
    private readonly IdentifierSource? m_visualIdSource;

    public Barcode(
        string simpleContent,
        int width = 300,
        int height = 0,
        int margin = 1,
        string? optionalInnerClass = null,
        string? optionalOuterClass = null,
        IdentifierSource? idSource = null,
        IdentifierSource? visualIdSource = null
    )
    {
        m_simpleContent = simpleContent;
        m_optionalInnerClass = optionalInnerClass ?? string.Empty;
        m_optionalOuterClass = optionalOuterClass ?? string.Empty;
        m_width = width;
        m_height = height;
        m_margin = margin;
        m_idSource = idSource;
        m_visualIdSource = visualIdSource ?? idSource;
    }

    public Barcode(
        Widget content,
        int width = 300,
        int height = 0,
        string? optionalInnerClass = null,
        string? optionalOuterClass = null,
        int margin = 1,
        IdentifierSource? idSource = null,
        IdentifierSource? visualIdSource = null
    )
    {
        m_widgetContent = content;
        m_optionalInnerClass = optionalInnerClass ?? string.Empty;
        m_optionalOuterClass = optionalOuterClass ?? string.Empty;
        m_width = width;
        m_height = height;
        m_margin = margin;
        m_idSource = idSource;
        m_visualIdSource = visualIdSource;
    }

    public override async Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        if (context.RenderMode == RenderMode.Documentation)
        {
            return navigator.GetFullPath();
        }

        string? content;

        if (m_simpleContent != null)
        {
            content = m_simpleContent;
        }
        else if (m_widgetContent != null)
        {
            var contentResult = await RenderInternal(navigator, renderer, context, m_widgetContent);

            content = contentResult.GetContent(m_widgetContent);
            if (content == null)
            {
                return RenderResult.NullResult;
            }
        }
        else
        {
            return RenderResult.NullResult;
        }



        var writer = new ScaleableWriterSvg(m_width, m_height, m_optionalInnerClass)
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new EncodingOptions
            {
                Margin = m_margin,
                PureBarcode = true
            }
        };

        var svgImage = writer.Write(content);

        if (svgImage.Content == null)
        {
            return RenderResult.NullResult;
        }

        var viewModel = new ViewModel
        {
            Content = svgImage.Content,
            CustomClass = m_optionalOuterClass
        };

        HandleIds(context, navigator, viewModel, m_idSource, m_visualIdSource);
        var result = await renderer.RenderBarcode(viewModel);
        return new RenderResult(result);
    }

    public class ViewModel : ViewModelBase
    {
        public required string Content { get; init; }
    }
}