using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

public class If(Func<XmlDocumentNavigator, bool> predicate, params Widget[] children)
    : Widget
{
    private Widget[] m_elseChildren = [];

    public If Else(params Widget[] elseChildren)
    {
        m_elseChildren = elseChildren;
        return this;
    }

    public override async Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        var testResult = predicate(navigator);
        return testResult 
            ? await children.RenderConcatenatedResult(navigator, renderer, context) 
            : await m_elseChildren.RenderConcatenatedResult(navigator, renderer, context);
    }

}