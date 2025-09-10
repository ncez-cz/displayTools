using System.Text;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

/// <summary>
/// Processes errors of the child widget tree, always returns a successful RenderResult.
/// </summary>
public class ErrorCatcher(Widget child) : Widget
{
    public override async Task<RenderResult> Render(XmlDocumentNavigator data, IWidgetRenderer renderer,
        RenderContext context)
    {
        var result = await RenderInternal(data, renderer, context, child);

        // Temporary rendering logic
        if (result.IsFatal)
        {
            return string.Join(';', result.Errors.Select(x => x.Message));
        }

        if (result.Errors.Count == 0)
        {
            return result.GetContent(child) ?? string.Empty;
        }

        var builder = new StringBuilder();
        builder.Append(result.GetContent(child));
        foreach (var error in result.Errors)
        {
            builder.Append(error.Message);
        }

        return builder.ToString();
    }
}