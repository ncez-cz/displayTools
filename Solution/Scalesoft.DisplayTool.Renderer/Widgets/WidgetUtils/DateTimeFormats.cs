using Scalesoft.DisplayTool.Renderer.Utils.Language;

namespace Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;

public static class DateTimeFormats
{
    private static readonly Dictionary<LanguageOptions, Dictionary<DateFormatType, string>> m_values =
        new()
        {
            {
                LanguageOptions.Czech, new Dictionary<DateFormatType, string>
                {
                    { DateFormatType.Year, "yyyy" },
                    { DateFormatType.MonthYear, "MMM yyyy" },
                    { DateFormatType.DayMonthYear, "d.M.yyyy" },
                    { DateFormatType.DayMonthYearTimezone, "d.M.yyyy UTCzzz" },
                    { DateFormatType.MinuteHourDayMonthYear, "d.M.yyyy HH:mm" },
                    { DateFormatType.MinuteHourDayMonthYearTimezone, "d.M.yyyy HH:mm UTCzzz" },
                    { DateFormatType.SecondMinuteHourDayMonthYear, "d.M.yyyy HH:mm:ss" },
                    { DateFormatType.SecondMinuteHourDayMonthYearTimezone, "d.M.yyyy HH:mm:ss UTCzzz" },
                }
            },
            {
                LanguageOptions.EnglishGreatBritain, new Dictionary<DateFormatType, string>
                {
                    { DateFormatType.Year, "yyyy" },
                    { DateFormatType.MonthYear, "MMM yyyy" },
                    { DateFormatType.DayMonthYear, "dd.MM.yyyy" },
                    { DateFormatType.DayMonthYearTimezone, "dd.MM.yyyy UTCzzz" },
                    { DateFormatType.MinuteHourDayMonthYear, "dd.MM.yyyy HH:mm" },
                    { DateFormatType.MinuteHourDayMonthYearTimezone, "dd.MM.yyyy HH:mm UTCzzz" },
                    { DateFormatType.SecondMinuteHourDayMonthYear, "dd.MM.yyyy HH:mm:ss" },
                    { DateFormatType.SecondMinuteHourDayMonthYearTimezone, "dd.MM.yyyy HH:mm:ss UTCzzz" },
                }
            },
        };

    public static string? GetFormat(Language language, DateFormatType type)
    {
        var langFormats = m_values.GetValueOrDefault(language.Primary) ?? m_values.GetValueOrDefault(language.Fallback);
        return langFormats?.GetValueOrDefault(type);
    }
}