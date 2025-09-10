using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

public class Parameter : Widget
{
    private readonly Widget[]? m_defaultContent;
    private readonly string? m_defaultSelect;
    private readonly string m_name;

    public Parameter(string name, Widget[] defaultContent)
    {
        m_name = name;
        m_defaultContent = defaultContent;
    }


    public Parameter(string name, string defaultSelect)
    {
        m_name = name;
        m_defaultSelect = defaultSelect;
    }

    public override async Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        if (m_defaultContent != null)
        {
            var content = await m_defaultContent.RenderConcatenatedResult(navigator, renderer, context);
            if (content.MaxSeverity >= ErrorSeverity.Fatal)
            {
                return content.Errors;
            }

            navigator.SetParameterValue(m_name, content.Content);
            return string.Empty;
        }

        if (m_defaultSelect != null)
        {
            navigator.SetParameter(m_name, m_defaultSelect);
            return string.Empty;
        }

        throw new InvalidOperationException();
    }
}
