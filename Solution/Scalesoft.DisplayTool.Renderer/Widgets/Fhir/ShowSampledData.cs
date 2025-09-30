using System.Globalization;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class ShowSampledData(
    string path = ".",
    IdentifierSource? idSource = null,
    IdentifierSource? visualIdSource = null
) : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        if (context.RenderMode == RenderMode.Documentation)
        {
            return Task.FromResult<RenderResult>(navigator.SelectSingleNode(path).GetFullPath());
        }

        var origin = navigator
            .SelectSingleNode(
                $"{path}/f:origin/f:value/@value")
            .Node?.Value;
        var lowerLimit = navigator
            .SelectSingleNode(
                $"{path}/f:lowerLimit/@value")
            .Node?.ValueAsDouble;
        var upperLimit = navigator
            .SelectSingleNode(
                $"{path}/f:upperLimit/@value")
            .Node?.ValueAsDouble;
        var period = navigator
            .SelectSingleNode(
                $"{path}/f:period/@value")
            .Node?.Value;
        var factor = navigator
            .SelectSingleNode(
                $"{path}/f:factor/@value")
            .Node?.ValueAsDouble;
        var dimensions = navigator
            .SelectSingleNode(
                $"{path}/f:dimensions/@value")
            .Node?.ValueAsInt;
        var data = navigator
            .SelectSingleNode(
                $"{path}/f:data/@value")
            .Node?.Value;

        if (double.TryParse(origin, CultureInfo.InvariantCulture, out var parsedOrigin) &&
            double.TryParse(period, CultureInfo.InvariantCulture, out var parsedPeriod) &&
            data != null &&
            dimensions != null)
        {
            var svgGraph = new SvgGraph(parsedOrigin, parsedPeriod, (int)dimensions, data, lowerLimit, upperLimit,
                factor ?? 1, idSource, visualIdSource);
            var tree = new Container(svgGraph);
            return tree.Render(navigator, renderer, context);
        }

        return Task.FromResult<RenderResult>(string.Empty);
    }
}