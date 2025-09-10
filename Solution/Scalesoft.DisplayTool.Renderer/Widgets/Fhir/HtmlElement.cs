using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class HtmlElement(string name, List<Widget> children, List<KeyValuePair<string,string>> attributes):Widget
{
    public override async Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        var childrenResult = await children.RenderConcatenatedResult(navigator, renderer, context);
        if (childrenResult.MaxSeverity > ErrorSeverity.Fatal)
        {
            return childrenResult;
        }
        
        var attributesString = string.Join(" ", attributes.Select(a => $"{a.Key}=\"{a.Value}\""));

        return $"<{name} {attributesString}>{childrenResult.Content}</{name}>";
    }
}