using System.Text;
using Scalesoft.DisplayTool.Renderer.Models;

namespace Scalesoft.DisplayTool.Renderer.Validators.Fhir;

public class FhirInternalJsonValidator : IDocumentValidator
{
    public InputFormat InputFormat => InputFormat.FhirJson;

    public async Task<ValidationResultModel> ValidateDocumentAsync(byte[] document, string? validator)
    {
        var jsonString = Encoding.UTF8.GetString(document);
        var isValid = JsonValidator.IsValidJson(jsonString);
        
        return await Task.FromResult(new ValidationResultModel
        {
            ErrorMessage = isValid ? null : "Chyba: Dokument JSON není validní. Zkontrolujte správnost syntaxe a struktury",
        });
    }
}