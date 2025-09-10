using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

/// <summary>
/// Evaluates tests of each option, returns the child of the first truthy option.
/// If no option is truthy returns the "otherwise" widget, if it isn't null.
/// Equivalent of <a href="https://developer.mozilla.org/en-US/docs/Web/XSLT/Element/choose">xls:choose</a>
/// </summary>
/// <seealso cref="When"/>
/// <seealso cref="Condition"/>
public class Choose(IEnumerable<When> options, params Widget[] otherwise) : Widget
{
    public override async Task<RenderResult> Render(XmlDocumentNavigator data, IWidgetRenderer renderer,
        RenderContext context)
    {
        foreach (var option in options)
        {
            if (option.Evaluate(data))
            {
                return await option.Children.RenderConcatenatedResult(data, renderer, context);
            }
        }

        return otherwise.Length == 0 ? string.Empty : await otherwise.RenderConcatenatedResult(data, renderer, context);
    }
}
