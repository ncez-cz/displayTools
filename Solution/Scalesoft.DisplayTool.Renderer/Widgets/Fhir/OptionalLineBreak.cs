using System.Xml.XPath;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class OptionalLineBreak(string[] xPathsToCheck) : Widget
{
    public override async Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        if (xPathsToCheck.Select(t => navigator.SelectSingleNode(t).Node).OfType<XPathNavigator>().Any())
        {
            return await renderer.RenderLineBreak();
        }

        return string.Empty;
    }
}