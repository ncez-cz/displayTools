using System.Diagnostics.CodeAnalysis;
using Scalesoft.DisplayTool.Renderer.Models.Enums;

namespace Scalesoft.DisplayTool.Renderer.Models;

public class RenderResult
{
    private RenderResult()
    {
    }

    private RenderResult(string? value)
    {
        Content = value;
    }

    public static RenderResult NullResult => new RenderResult(null);

    public RenderResult(string content, List<ParseError>? errors = null)
    {
        Content = content;
        Errors = errors ?? [];
    }

    public static implicit operator RenderResult(string value)
    {
        return new RenderResult { Content = value };
    }

    public static implicit operator RenderResult(ParseError error)
    {
        return new RenderResult { Errors = [error] };
    }

    public static implicit operator RenderResult(List<ParseError> errors)
    {
        return new RenderResult { Errors = errors };
    }

    public string? Content { get; init; }

    public List<ParseError> Errors { get; init; } = [];


    [MemberNotNullWhen(true, nameof(Content))]
    public bool HasValue => Content != null;

    [MemberNotNullWhen(true, nameof(Errors))]
    [MemberNotNullWhen(false, nameof(Content))]
    public bool HasErrors => Errors?.Count > 0;

    public ErrorSeverity? MaxSeverity => HasErrors ? Errors.Max(e => e.Severity) : null;
    
    public bool IsNullResult => Content == null && Errors.Count == 0;
}
