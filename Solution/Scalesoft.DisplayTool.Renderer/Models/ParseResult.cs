namespace Scalesoft.DisplayTool.Renderer.Models;
public class ParseResult<T>
{
    public List<T> Results { get; set; } = [];

    public List<ParseError> Errors { get; set; } = [];

    public static implicit operator ParseResult<T>(T result)
    {
        return new ParseResult<T> { Results = [result] };
    }

    public static implicit operator ParseResult<T>(List<T> result)
    {
        return new ParseResult<T> { Results = result };
    }

    public static implicit operator ParseResult<T>(ParseError error)
    {
        return new ParseResult<T> { Errors = [error] };
    }

    public static implicit operator ParseResult<T>(List<ParseError> errors)
    {
        return new ParseResult<T> { Errors = errors };
    }
}
