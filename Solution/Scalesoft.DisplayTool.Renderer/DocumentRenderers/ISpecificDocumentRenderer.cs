using Scalesoft.DisplayTool.Renderer.Models.Enums;

namespace Scalesoft.DisplayTool.Renderer.DocumentRenderers;

public interface ISpecificDocumentRenderer
{
    public InputFormat InputFormat { get; }

    public Task<DocumentResult> RenderAsync(byte[] fileContent, OutputFormat outputFormat, DocumentOptions options, RenderMode renderMode = RenderMode.Standard, DocumentType? documentType = null);
}