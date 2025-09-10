using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

/// <summary>
/// Not technically a widget. Equivalent of <a href="https://developer.mozilla.org/en-US/docs/Web/XSLT/Element/when">xls:when</a>.
/// </summary>
/// <seealso cref="Choose"/>
/// <param name="test">XPath expression</param>
public class When(string test, params Widget[] children)
{
    public Widget[] Children { get; } = children;

    public bool Evaluate(XmlDocumentNavigator data)
    {
        return data.EvaluateCondition(test);
    }
}
