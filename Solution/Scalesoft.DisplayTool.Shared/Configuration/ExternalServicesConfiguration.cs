namespace Scalesoft.DisplayTool.Shared.Configuration;

public class ExternalServicesConfiguration
{
    public required TranslationSourceConfiguration TranslationSource { get; init; }
    public required DocumentConverterConfiguration DocumentConverter { get; init; }
    public required DocumentValidationConfiguration DocumentValidation { get; init; }
}

public class TranslationSourceConfiguration
{
    public string? BaseUrl { get; init; }
}

public class DocumentConverterConfiguration
{
    public required string BaseUrl { get; init; }
    public bool UseConverterForPatientSummary { get; init; }
}

public class DocumentValidationConfiguration
{
    // If any validator url is not specified, an internal validator is used instead.
    public string? FhirBaseUrl { get; init; }
    public string? CdaBaseUrl { get; init; }
}