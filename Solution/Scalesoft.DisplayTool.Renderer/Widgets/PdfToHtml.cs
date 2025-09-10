using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

public class PdfToHtml(byte[] pdfBytes) : Widget
{
    public override async Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var view = await renderer.RenderPdf(
            new PdfModel
                { PdfBytes = pdfBytes, CollapseIcon = IconHelper.GetInstance(SupportedIcons.ChevronUp, context) }
        );
        return new RenderResult(view);
    }

    public class PdfModel
    {
        public required byte[] PdfBytes { get; set; }
        public required string CollapseIcon { get; set; }
    }
}