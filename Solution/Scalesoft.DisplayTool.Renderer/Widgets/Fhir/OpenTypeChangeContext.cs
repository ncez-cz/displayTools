using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class OpenTypeChangeContext(string prefix, params Widget[] children) : Widget
{
    public override async Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer, RenderContext context)
    {
        
        var valueNav = navigator.SelectSingleNode($"*[starts-with(local-name(), '{prefix}')]");
        if (valueNav.Node == null)
        {
            // Can't change context if the node is not found
            return RenderResult.NullResult;
        }
        
        return await new ChangeContext(valueNav, children).Render(navigator, renderer, context);
    }
}