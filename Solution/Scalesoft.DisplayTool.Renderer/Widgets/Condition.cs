using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

/// <summary>
/// Evaluates the given test. Returns the children if it's truthy, otherwise returns an empty string.
/// Equivalent of <a href="https://developer.mozilla.org/en-US/docs/Web/XSLT/Element/if">xls:if</a>
/// </summary>
/// <param name="test">XPath expression</param>
public class Condition(string test, params Widget[] children) : Widget
{
    public override async Task<RenderResult> Render(XmlDocumentNavigator data, IWidgetRenderer renderer,
        RenderContext context)
    {
        var testResult = data.EvaluateCondition(test);
        return testResult ? await children.RenderConcatenatedResult(data, renderer, context) : string.Empty;
    }
}
