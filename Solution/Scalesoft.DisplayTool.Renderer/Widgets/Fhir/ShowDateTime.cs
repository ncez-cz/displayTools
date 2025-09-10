﻿using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class ShowDateTime(string path = ".") : Widget
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
                Message = "Missing date value",
                Path = navigator.GetFullPath(),
                Severity = ErrorSeverity.Warning,
            });
        }

        // Allowed date formats in FHIR are YYYY, YYYY-MM, YYYY-MM-DD or YYYY-MM-DDThh:mm:ss+zz:zz.
        // Show a date value with the same specificity we received.
        var format = value.Length switch
        {
            < 5 => DateFormatType.Year,
            < 8 => DateFormatType.MonthYear,
            < 11 => DateFormatType.DayMonthYear,
            _ => DateFormatType.MinuteHourDayMonthYearTimezone,
        };


        if (DateTime.TryParse(value, out var date))
        {
            if (context.RenderMode == RenderMode.Documentation)
            {
                return Task.FromResult<RenderResult>(navigator.GetFullPath());
            }

            var dateFormatTarget = DateTimeFormats.GetFormat(context.Language, format);
            return Task.FromResult<RenderResult>(date.ToString(dateFormatTarget));
        }

        return Task.FromResult(new RenderResult(
            value,
            [
                new ParseError
                {
                    Kind = ErrorKind.InvalidValue, Message = "Invalid date format", Path = navigator.GetFullPath(),
                    Severity = ErrorSeverity.Warning,
                },
            ]));
    }
}