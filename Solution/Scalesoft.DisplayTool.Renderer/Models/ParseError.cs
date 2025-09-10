using Scalesoft.DisplayTool.Renderer.Models.Enums;

namespace Scalesoft.DisplayTool.Renderer.Models;

public class ParseError
{
    public required ErrorKind Kind { get; set; }
    public required ErrorSeverity Severity { get; init; }
    public string? Message { get; init; }
    public required string Path { get; init; }

    public static ParseError MissingValue(string path)
    {
        return new ParseError
        {
            Path = path,
            Kind = ErrorKind.MissingValue,
            Severity = ErrorSeverity.Warning,
        };
    }
}