using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

/// <summary>
/// A widget that selects and renders another widget based on the current node's string value,
/// using a predefined mapping from values (typically enum values as strings) to widgets.
/// 
/// - If a match is found in the dictionary, the corresponding widget is rendered.
/// - If no match is found and a fallback widget is provided, it renders that instead.
/// - If no match and no fallback, it renders the value of the matching node.
/// </summary>
public class ShowEnum(Dictionary<string, Widget> valuesDictionary, string xpath = ".", Widget? fallbackDisplay = null)
    : Widget
{
    public ShowEnum(
        Dictionary<string, string> valuesDictionary,
        string xpath = ".",
        Widget? fallbackDisplay = null,
        TextStyle textStyle = TextStyle.Regular
    )
        : this(
            valuesDictionary.ToDictionary(kvp => kvp.Key,
                Widget (kvp) => new TextContainer(textStyle, new ConstantText(kvp.Value))), xpath, fallbackDisplay)
    {
    }

    public override async Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var nodeNavigator = navigator.SelectSingleNode(xpath);

        var nodeValue = nodeNavigator.Node?.Value;

        if (nodeValue == null)
        {
            return RenderResult.NullResult;
        }

        if (context.RenderMode == RenderMode.Documentation)
        {
            return nodeNavigator.GetFullPath();
        }

        if (valuesDictionary.TryGetValue(nodeValue, out var widget))
        {
            return await widget.Render(navigator, renderer, context);
        }

        if (fallbackDisplay == null)
        {
            return nodeValue;
        }

        return await fallbackDisplay.Render(navigator, renderer, context);
    }
}