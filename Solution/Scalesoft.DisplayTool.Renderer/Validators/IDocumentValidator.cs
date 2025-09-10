using Scalesoft.DisplayTool.Renderer.Models;

namespace Scalesoft.DisplayTool.Renderer.Validators;

public interface IDocumentValidator
{
    public InputFormat InputFormat { get; }

    public Task<ValidationResultModel> ValidateDocumentAsync(byte[] document, string? validator);
}