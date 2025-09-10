using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;

namespace Scalesoft.DisplayTool.Renderer.Extensions;

public static class ParseErrorExtensions
{
    public static ErrorSeverity? MaxSeverity(this IList<ParseError> errors)
    {
        if (!errors.Any())
        {
            return null;
        }

        return errors.Max(e => e.Severity);
    }
}