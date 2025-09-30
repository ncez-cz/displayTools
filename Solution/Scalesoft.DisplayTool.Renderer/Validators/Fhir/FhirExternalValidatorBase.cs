using System.Text;
using Scalesoft.DisplayTool.Renderer.Clients.FhirValidator;
using Scalesoft.DisplayTool.Renderer.Clients.FhirValidator.Contracts;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Exceptions;

namespace Scalesoft.DisplayTool.Renderer.Validators.Fhir;

public class FhirExternalValidatorBase : IDocumentValidator
{
    private readonly FhirValidatorClient m_fhirValidatorClient;
    private readonly string m_defaultFhirVersion = "4.0.1";
    private readonly List<string> m_defaultIgs = ["hl7.fhir.uv.ips#current"];
    private readonly string m_defaultLocale = "en";

    protected FhirExternalValidatorBase(InputFormat inputFormat, FhirValidatorClient fhirValidatorClient)
    {
        InputFormat = inputFormat;
        m_fhirValidatorClient = fhirValidatorClient;
    }

    public InputFormat InputFormat { get; }

    public async Task<ValidationResultModel> ValidateDocumentAsync(byte[] document, string? validator)
    {
        var file = Encoding.UTF8.GetString(document);
        var fileType = InputFormat == InputFormat.FhirXml ? "xml" : "json";

        var contentToValidate = new FhirValidatorRequest
        {
            FilesToValidate =
            [
                new FileInfoContract()
                {
                    FileName = "fhir." + fileType,
                    FileContent = file,
                    FileType = fileType,
                }
            ],
            CliContext = new CliContextContract()
            {
                Sv = validator ?? m_defaultFhirVersion,
                Ig = m_defaultIgs,
                Locale = m_defaultLocale,
            },
            SessionId = Guid.NewGuid().ToString(),
        };
        
        try
        {
            ValidateFileStructure(file);
            var fhirResponse = await m_fhirValidatorClient.ValidateDocumentAsync(contentToValidate);

            return MapResponse(fhirResponse);
        }
        catch (FhirValidatorException ex)
        {
            return new ValidationResultModel()
            {
                ErrorMessage = ex.Message
            };
        }
    }

    private void ValidateFileStructure(string file)
    {
        if (InputFormat == InputFormat.FhirXml)
        {
            if (!XmlValidator.IsValidXml(file))
            {
                throw new FhirValidatorException("Chyba: Dokument XML není validní. Zkontrolujte správnost syntaxe a struktury");
            }
        } else if (InputFormat == InputFormat.FhirJson)
        {
            if (!JsonValidator.IsValidJson(file))
            {
                throw new FhirValidatorException("Chyba: Dokument JSON není validní. Zkontrolujte správnost syntaxe a struktury");
            }
        }
    }
    
    private ValidationResultModel MapResponse(FhirValidatorResponse fhirResponse)
    {
        var validations = new List<ValidationModel>();
        var validationResult = new ValidationResultModel();

        if (fhirResponse.Outcomes != null)
        {
            foreach (var outcome in fhirResponse.Outcomes)
            {
                if (outcome.Issues != null)
                {
                    foreach (var issue in outcome.Issues)
                    {
                        var validation = new ValidationModel()
                        {
                            Type = issue.Type,
                            Location = issue.Location,
                            Message = issue.Message,
                        };
                        validations.Add(validation);
                    }
                }
            }
        }
        validationResult.Validations = validations;
        
        return validationResult;
    }
}