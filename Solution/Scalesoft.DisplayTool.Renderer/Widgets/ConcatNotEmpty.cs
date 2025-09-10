using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

/// <summary>
/// A widget that renders a collection of child widgets and concatenates their non-empty results 
/// with a separator between them.
/// </summary>
/// <remarks>
/// This widget filters out any empty or null content produced by children before joining them.
/// If all children produce empty content, this widget will render an empty string.
/// </remarks>
public class ConcatNotEmpty(
    IEnumerable<Widget> children,
    Widget separator
) : Widget
{
    public ConcatNotEmpty(IEnumerable<Widget> children, string seperator = " ") : this(children, new ConstantText(seperator))
    {
    }
    
    public override async Task<RenderResult> Render(XmlDocumentNavigator data, IWidgetRenderer renderer,
        RenderContext context)
    {
        var results = await RenderInternal(data, renderer, [..children, separator], context);
        if (results.IsFatal)
        {
            return results.Errors;
        }
        
        var content = results.GetValidContents(children);
        var renderedSeparator = results.GetContent(separator);
        List<string> nonEmptyContent = [];
        nonEmptyContent.AddRange(content.Where(str => !string.IsNullOrEmpty(str)));

        var concatenated = string.Join(renderedSeparator, nonEmptyContent);
        return new RenderResult(concatenated, results.Errors);
    }



}
