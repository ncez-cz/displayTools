using System.Globalization;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class ShowTime(string path = ".") : Widget
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

        var value = navigator.SelectSingleNode($"{path}/@value").Node?.Value;
        if (value == null)
        {
            return Task.FromResult<RenderResult>(new ParseError
            {
                Kind = ErrorKind.MissingValue,
                Message = "Missing time value",
                Path = navigator.GetFullPath(),
                Severity = ErrorSeverity.Warning,
            });
        }


        if (TimeOnly.TryParse(value, CultureInfo.InvariantCulture, out var time))
        {
            if (context.RenderMode == RenderMode.Documentation)
            {
                return Task.FromResult<RenderResult>(navigator.GetFullPath());
            }

            return Task.FromResult<RenderResult>(time.ToString("HH:mm"));
        }

        return Task.FromResult(new RenderResult(
            value,
            [
                new ParseError
                {
                    Kind = ErrorKind.InvalidValue, Message = "Invalid time format", Path = navigator.GetFullPath(),
                    Severity = ErrorSeverity.Warning,
                },
            ]));
    }
}