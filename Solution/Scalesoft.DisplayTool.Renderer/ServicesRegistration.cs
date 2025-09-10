using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Scalesoft.DisplayTool.Renderer.DocumentRenderers;
using Scalesoft.DisplayTool.Renderer.DocumentRenderers.Tools;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils.Language;
using Scalesoft.DisplayTool.Renderer.Validators;
using Scalesoft.DisplayTool.Renderer.Validators.Cda;
using Scalesoft.DisplayTool.Renderer.Validators.Dasta;
using Scalesoft.DisplayTool.Renderer.Validators.Fhir;
using Scalesoft.DisplayTool.TermxTranslator;

namespace Scalesoft.DisplayTool.Renderer;

public static class ServicesRegistration
{
    public static IServiceProvider CreateServiceProvider(ILoggerFactory loggerFactory, bool useExternalValidators, PdfRendererOptions? pdfRendererOptions)
    {
        var services = new ServiceCollection();
        services.TryAdd(ServiceDescriptor.Singleton(typeof(ILogger<>), typeof(Logger<>)));
        services.AddSingleton(loggerFactory);

        services.AddScoped<ISpecificDocumentRenderer, CdaDocumentRenderer>();
        services.AddScoped<ISpecificDocumentRenderer, FhirJsonDocumentRenderer>();
        services.AddScoped<ISpecificDocumentRenderer, FhirXmlDocumentRenderer>();
        services.AddScoped<ISpecificDocumentRenderer, PdfDocumentRenderer>();
        services.AddScoped<ISpecificDocumentRenderer, DastaDocumentRenderer>();

        services.AddSingleton<IWidgetRenderer, RazorWidgetRenderer>();

        services.AddScoped<Language>();

        services.RegisterTermxTranslator();

        services.AddSingleton(pdfRendererOptions ?? new PdfRendererOptions());
        services.AddSingleton<HtmlToPdfConverter>();

        if (useExternalValidators)
        {
            services.AddSingleton<IDocumentValidator, CdaExternalValidator>();
            services.AddSingleton<IDocumentValidator, FhirExternalJsonValidator>();
            services.AddSingleton<IDocumentValidator, FhirExternalXmlValidator>();
        }
        else
        {
            services.AddSingleton<IDocumentValidator, CdaInternalXmlValidator>();
            services.AddSingleton<IDocumentValidator, FhirInternalJsonValidator>();
            services.AddSingleton<IDocumentValidator, FhirInternalXmlValidator>();
        }

        // Only internal validator is available for dasta:
        services.AddSingleton<IDocumentValidator, DastaInternalXmlValidator>();

        services.AddSingleton<DocumentValidatorProvider>();

        return services.BuildServiceProvider();
    }
}