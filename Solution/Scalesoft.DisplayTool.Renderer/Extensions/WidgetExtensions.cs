using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Extensions;

public static class WidgetExtensions
{
    public static async Task<RenderResult> RenderConcatenatedResult(
        this IList<Widget> widgets,
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context)
    {
        // If variables are used, tasks must be executed in order
        List<RenderResult> children = [];
        foreach (var widget in widgets)
        {
            children.Add(await widget.Render(navigator, renderer, context));
        }

        var errors = children.SelectMany(x => x.Errors).ToList();
        if (errors.MaxSeverity() >= ErrorSeverity.Fatal)
        {
            return errors;
        }

        if (children.All(x => x.IsNullResult))
        {
            return RenderResult.NullResult;
        }
        
        var content = children.Where(x => x.Content != null).Select(x => x.Content!).ToList();
        return new RenderResult(string.Join(string.Empty, content), errors);
    }
}