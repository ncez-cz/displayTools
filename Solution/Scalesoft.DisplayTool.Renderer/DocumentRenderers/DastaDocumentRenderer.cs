using System.Xml;
using System.Xml.XPath;
using Microsoft.Extensions.Logging;
using Scalesoft.DisplayTool.Renderer.DocumentRenderers.Tools;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils;
using Scalesoft.DisplayTool.Renderer.Utils.Language;
using Scalesoft.DisplayTool.Renderer.Validators;
using Scalesoft.DisplayTool.Renderer.Widgets;
using Scalesoft.DisplayTool.Renderer.Widgets.Dasta;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;
using Scalesoft.DisplayTool.Shared.Translation;

namespace Scalesoft.DisplayTool.Renderer.DocumentRenderers;

public class DastaDocumentRenderer : SpecificDocumentRendererBase
{
    private readonly IWidgetRenderer m_widgetRenderer;
    private readonly ICodeTranslator m_translator;
    private readonly Language m_language;
    private readonly ILoggerFactory m_loggerFactory;

    public override InputFormat InputFormat => InputFormat.Dasta;

    public DastaDocumentRenderer(
        IWidgetRenderer widgetRenderer,
        DocumentValidatorProvider documentValidatorProvider,
        HtmlToPdfConverter htmlToPdfConverter,
        ICodeTranslator translator,
        Language language,
        ILoggerFactory loggerFactory) : base(documentValidatorProvider, htmlToPdfConverter)
    {
        m_widgetRenderer = widgetRenderer;
        m_translator = translator;
        m_language = language;
        m_loggerFactory = loggerFactory;
    }

    public override async Task<DocumentResult> RenderAsync(
        byte[] fileContent,
        OutputFormat outputFormat,
        DocumentOptions options,
        RenderMode renderMode = RenderMode.Standard,
        DocumentType? documentType = null)
    {
        if (outputFormat != OutputFormat.Html && outputFormat != OutputFormat.Pdf)
        {
            throw new NotSupportedException();
        }

        var validationResult = new ValidationResultModel();
        if (options.ValidateDocument)
        {
            validationResult = await GetValidator().ValidateDocumentAsync(fileContent, null);

            if (validationResult.ErrorMessage != null)
            {
                return CreateResultForError(validationResult.ErrorMessage);
            }
        }

        using var stream = new MemoryStream(fileContent);
        using var xr = XmlReader.Create(stream);
        var document = new XPathDocument(xr, XmlSpace.Preserve);
        var navigator = document.CreateNavigator();

        var root = new XmlDocumentNavigator(navigator);
        root.AddNamespace("ds", "urn:cz-mzcr:ns:dasta:ds4:ds_dasta");
        root.AddNamespace("dsip", "urn:cz-mzcr:ns:dasta:ds4:ds_ip");
        root.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
        root.AddNamespace("xsd", "http://www.w3.org/2001/XMLSchema");

        var renderContext = new RenderContext(
            m_translator,
            m_language,
            m_loggerFactory,
            documentType ?? DocumentType.PatientSummary,
            renderMode,
            options.PreferTranslationsFromDocument);
        
        List<Widget> widgets = [new DastaRootWidget(), new Container(new LazyWidget(()=> renderContext.RenderedIcons.Select(x => new RawText(IconHelper.GetOriginal(x))).ToList<Widget>()), optionalClass: "icon-reservoir")];

        var renderResult = await widgets.RenderConcatenatedResult(root, m_widgetRenderer, renderContext);

        var validationWidget = new ValidationResult(validationResult);
        var validationRenderResult = await validationWidget.Render(root, m_widgetRenderer, renderContext);

        var htmlContent =
            await m_widgetRenderer.WrapWithLayout(renderResult.Content, validationRenderResult.Content, renderMode);
        var renderedDocumentContent = await CreateOutputDocumentAsync(fileContent, htmlContent, outputFormat);

        var documentResult = new DocumentResult
        {
            Content = renderedDocumentContent,
            Errors = renderResult.Errors.Where(x => x.Severity >= ErrorSeverity.Fatal)
                .Select(x => x.Message ?? x.Kind.ToString()).ToList(),
            Warnings = renderResult.Errors.Where(x => x.Severity <= ErrorSeverity.Warning)
                .Select(x => x.Message ?? x.Kind.ToString()).ToList(),
            IsRenderedSuccessfully = renderResult.MaxSeverity is null or < ErrorSeverity.Fatal,
        };

        return documentResult;
    }
}