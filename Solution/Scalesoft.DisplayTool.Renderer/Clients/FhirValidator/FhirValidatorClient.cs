using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Scalesoft.DisplayTool.Renderer.Clients.FhirValidator.Contracts;
using Scalesoft.DisplayTool.Renderer.Models.Exceptions;

namespace Scalesoft.DisplayTool.Renderer.Clients.FhirValidator;

public class FhirValidatorClient
{
    private readonly HttpClient m_httpClient;
    private readonly ILogger<FhirValidatorClient> m_logger;
    private readonly string m_validatorUrl = "https://validator.fhir.org/validate";
    public FhirValidatorClient(ILogger<FhirValidatorClient> logger)
    {
        m_httpClient = new HttpClient();
        m_logger = logger;
    }

    public async Task<FhirValidatorResponse> ValidateDocumentAsync(FhirValidatorRequest contentToValidate)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            var content = new StringContent(JsonSerializer.Serialize(contentToValidate, options), Encoding.UTF8,
                MediaTypeNames.Application.Json);

            var response = await m_httpClient.PostAsync(m_validatorUrl, content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<FhirValidatorResponse>(responseContent, options)!;
        }
        catch (HttpRequestException ex)
        {
            var message = "Došlo k chybě při zpracovávání požadavku";
            m_logger.LogError(ex, message);
            throw new FhirValidatorException(message);
        }
        catch (TaskCanceledException ex)
        {
            var message = "Došlo k chybě při komunikaci s validační službou";
            m_logger.LogError(ex, message);
            throw new FhirValidatorException(message);
        }
        catch (JsonException ex)
        {
            var message = "Došlo k chybě při json deserializaci";
            m_logger.LogError(ex, message);
            throw new FhirValidatorException(message);
        }
    }
}