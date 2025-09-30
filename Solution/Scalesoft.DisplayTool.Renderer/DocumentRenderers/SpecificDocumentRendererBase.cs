using System.Text;
using Scalesoft.DisplayTool.Renderer.DocumentRenderers.Tools;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Validators;

namespace Scalesoft.DisplayTool.Renderer.DocumentRenderers;

public abstract class SpecificDocumentRendererBase : ISpecificDocumentRenderer
{
    private readonly DocumentValidatorProvider m_documentValidatorProvider;
    private readonly HtmlToPdfConverter m_htmlToPdfConverter;

    public abstract InputFormat InputFormat { get; }

    public abstract Task<DocumentResult> RenderAsync(
        byte[] fileContent,
        OutputFormat outputFormat,
        DocumentOptions options,
        DocumentType documentType,
        RenderMode renderMode = RenderMode.Standard,
        LevelOfDetail levelOdDetail = LevelOfDetail.Simplified
    );

    protected SpecificDocumentRendererBase(DocumentValidatorProvider documentValidatorProvider, HtmlToPdfConverter htmlToPdfConverter)
    {
        m_documentValidatorProvider = documentValidatorProvider;
        m_htmlToPdfConverter = htmlToPdfConverter;
    }

    protected IDocumentValidator GetValidator()
    {
        return m_documentValidatorProvider.GetValidator(InputFormat);
    }

    protected async Task<byte[]> CreateOutputDocumentAsync(byte[] fileContent, string htmlContent, OutputFormat outputFormat)
    {
        var renderedDocumentContent = outputFormat switch
        {
            OutputFormat.Html => Encoding.UTF8.GetBytes(htmlContent),
            OutputFormat.Pdf => await m_htmlToPdfConverter.ConvertHtmlToPdf(htmlContent, fileContent, InputFormat),
            _ => throw new NotSupportedException($"Unsupported output format: {outputFormat}"),
        };
        
        return renderedDocumentContent;
    } 

    protected DocumentResult CreateResultForError(string errorMessage)
    {
        return new DocumentResult
        {
            Content = [],
            Errors = [errorMessage],
            Warnings = new List<string>(),
            IsRenderedSuccessfully = false,
        };
    }
}