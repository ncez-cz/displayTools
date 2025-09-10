using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

public class Optional : Widget
{
    private readonly string m_path;
    private readonly Func<XmlDocumentNavigator, IList<Widget>> m_builder;

    public Optional(string path, params Widget[] children)
    {
        m_path = path;
        m_builder = _ => children;
    }

    public Optional(string path, Func<XmlDocumentNavigator, IList<Widget>> builder)
    {
        m_path = path;
        m_builder = builder;
    }

    public Optional(string path, Func<XmlDocumentNavigator, Widget> builder)
    {
        m_path = path;
        m_builder = nav => [builder(nav)];
    }

    public override async Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var element = navigator.SelectSingleNode(m_path);
        return element.Node == null
            ? RenderResult.NullResult
            : await m_builder(element).RenderConcatenatedResult(element, renderer, context);
    }
}