using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

public class Variable : Widget
{
    private readonly Widget[]? m_content;
    private readonly string? m_select;
    private readonly string m_name;

    public Variable(string name, Widget[] defaultContent)
    {
        m_name = name;
        m_content = defaultContent;
    }


    public Variable(string name, string defaultSelect)
    {
        m_name = name;
        m_select = defaultSelect;
    }

    public override async Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        if (m_content != null)
        {
            var content = await m_content.RenderConcatenatedResult(navigator, renderer, context);
            if (content.MaxSeverity >= ErrorSeverity.Fatal)
            {
                return content.Errors;
            }

            navigator.SetVariableValue(m_name, content.Content);
            return string.Empty;
        }

        if (m_select != null)
        {
            navigator.SetVariable(m_name, m_select);
            return string.Empty;
        }

        throw new InvalidOperationException();
    }
}
