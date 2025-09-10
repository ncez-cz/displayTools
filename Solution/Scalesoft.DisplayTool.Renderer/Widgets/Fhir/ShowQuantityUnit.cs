using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class ShowQuantityUnit(string path = ".") : Widget
{
    public override async Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer, RenderContext context)
    {
        List<ParseError> errors = [];

        var unitString = navigator.SelectSingleNode($"{path}/f:unit/@value").Node?.Value;
        var unitSystem = navigator.SelectSingleNode($"{path}/f:system/@value").Node?.Value;
        var unitCode = navigator.SelectSingleNode($"{path}/f:code/@value").Node?.Value;

        var display = unitString ?? unitCode;

        if (string.IsNullOrEmpty(display))
        {
            return string.Empty;
        }

        if (context.RenderMode == RenderMode.Documentation)
        {
            return string.IsNullOrEmpty(unitCode)
                ? navigator.SelectSingleNode($"{path}/f:unit/@value").GetFullPath()
                : navigator.SelectSingleNode($"{path}/f:code/@value").GetFullPath();
        }

        if (display == "1")
        {
            return display;
        }

        var result = await new CodedValue(unitCode, unitSystem, display).Render(navigator, renderer, context);

        errors.AddRange(result.Errors);
        if (errors.MaxSeverity() >= ErrorSeverity.Fatal || !result.HasValue)
        {
            return errors;
        }

        return new RenderResult(result.Content, errors);
    }
}