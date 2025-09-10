using System.Globalization;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class ShowDecimal(string path = ".") : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        if (IsDataAbsent(navigator, path))
        {
            return new AbsentData(path).Render(navigator, renderer, context);
        }

        if (context.RenderMode == RenderMode.Documentation)
        {
            return Task.FromResult<RenderResult>(navigator.SelectSingleNode(path).GetFullPath());
        }

        var value = navigator.SelectSingleNode($"{path}/@value").Node?.Value;
        if (value == null)
        {
            return Task.FromResult<RenderResult>(new ParseError
            {
                Kind = ErrorKind.MissingValue,
                Message = "Missing decimal value",
                Path = navigator.GetFullPath(),
                Severity = ErrorSeverity.Warning,
            });
        }

        if (decimal.TryParse(value, CultureInfo.InvariantCulture, out var result))
        {
            return Task.FromResult<RenderResult>(result.ToString(new CultureInfo(context.Language.Primary.Code)));
        }

        return Task.FromResult(new RenderResult(
            value,
            [
                new ParseError
                {
                    Kind = ErrorKind.InvalidValue, Message = "Invalid decimal format", Path = navigator.GetFullPath(),
                    Severity = ErrorSeverity.Warning,
                },
            ]));
    }
}