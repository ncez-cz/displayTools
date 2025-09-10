using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Dasta;

public class DastaMknOrphaCodedValue(string codeSelect) : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        var node = navigator.SelectSingleNode(codeSelect);
        var codeSystem = node.SelectSingleNode("dsip:kod_syst").Node?.Value;
        var code = node.SelectSingleNode("dsip:kod").Node?.Value;
        var codeText = node.SelectSingleNode("dsip:kod_text").Node?.Value;

        var widget = new CodedValue(code, codeSystem, codeText, displayCodeSystem:true);

        return widget.Render(navigator, renderer, context);
    }
}
