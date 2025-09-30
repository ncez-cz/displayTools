using System.ServiceModel;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;
using Scalesoft.DisplayTool.Renderer.Clients.CdaValidator.Contracts;
using Scalesoft.DisplayTool.Renderer.ModelBasedValidationWSService;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.CDA;

namespace Scalesoft.DisplayTool.Renderer.Validators.Cda;

public class CdaExternalValidator : IDocumentValidator
{
    private readonly ModelBasedValidationWSClient m_client;
    private readonly ILogger<CdaExternalValidator> m_logger;
    private readonly string m_defaultValidator = "eHDSI - FRIENDLY CDA (L3) validation - Wave 7 (V7.2.0)";

    public CdaExternalValidator(ILogger<CdaExternalValidator> logger, ModelBasedValidationWSClient client)
    {
        m_logger = logger;
        m_client = client;
    }

    public InputFormat InputFormat => InputFormat.Cda;

    public async Task<ValidationResultModel> ValidateDocumentAsync(byte[] document, string? validator)
    {
        try
        {
            var base64Document = Convert.ToBase64String(document);

            if (!XmlValidator.IsValidXml(Encoding.UTF8.GetString(document)))
            {
                return new ValidationResultModel()
                {
                    ErrorMessage = "Chyba: Dokument XML není validní. Zkontrolujte správnost syntaxe a struktury"
                };
            }

            if (validator == null)
            {
                validator = m_defaultValidator;
            }

            var response = await m_client.validateBase64DocumentAsync(base64Document, validator);

            return MapResponse(response);
        }
        catch (Exception ex) when (ex is FaultException<SOAPException> || ex is InvalidOperationException)
        {
            var message = "Došlo k chybě při zpracovávání požadavku";
            m_logger.LogError(ex, message);
            return new ValidationResultModel()
            {
                ErrorMessage = message
            };
        }
        catch (Exception ex) when (ex is ServerTooBusyException || ex is TimeoutException)
        {
            var message = "Došlo k chybě při komunikaci s validační službou";
            m_logger.LogError(ex, message);
            return new ValidationResultModel()
            {
                ErrorMessage = message
            };
        }
    }

    private detailedResult Deserialize(string response)
    {
        var serializer = new XmlSerializer(typeof(detailedResult));
        using (var responseReader = new StringReader(response))
        {
            var result = (detailedResult)serializer.Deserialize(responseReader)!;

            return result;
        }
    }

    private ValidationResultModel MapResponse(validateBase64DocumentResponse response)
    {
        var detailedResult = Deserialize(response.DetailedResult);
        var validations = new List<ValidationModel>();
        var validationResult = new ValidationResultModel()
        {
            AdditionalCdaValidation = new AdditionalCdaValidationModel()
            {
                DocumentWellFormed = detailedResult.DocumentWellFormed.Result,
                DocumentXsdValid = detailedResult.DocumentValidXSD,
            }
        };

        foreach (var item in detailedResult.MDAValidation.Items)
        {
            if (item is Error error)
            {
                var validation = new ValidationModel()
                {
                    Type = error.Type,
                    Location = error.Location,
                    Message = error.Description,
                };
                validations.Add(validation);
            }
            else if (item is Warning warning)
            {
                var validation = new ValidationModel()
                {
                    Type = warning.Type,
                    Location = warning.Location,
                    Message = warning.Description,
                };
                validations.Add(validation);
            }
            else if (item is Note note)
            {
                var validation = new ValidationModel()
                {
                    Type = note.Type,
                    Location = note.Location,
                    Message = note.Description,
                };
                validations.Add(validation);
            }
            else if (item is Info info)
            {
                var validation = new ValidationModel()
                {
                    Type = info.Type,
                    Location = info.Location,
                    Message = info.Description,
                };
                validations.Add(validation);
            }
        }

        validationResult.Validations = validations;

        return validationResult;
    }
}