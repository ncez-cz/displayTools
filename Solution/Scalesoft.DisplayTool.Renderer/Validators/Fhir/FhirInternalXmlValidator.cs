using System.Text;
using Scalesoft.DisplayTool.Renderer.Models;

namespace Scalesoft.DisplayTool.Renderer.Validators.Fhir;

public class FhirInternalXmlValidator : IDocumentValidator
{
    public InputFormat InputFormat => InputFormat.FhirXml;

    public async Task<ValidationResultModel> ValidateDocumentAsync(byte[] document, string? validator)
    {
        var xmlString = Encoding.UTF8.GetString(document);
        var isValid = XmlValidator.IsValidXml(xmlString);
        
        return await Task.FromResult(new ValidationResultModel
        {
            ErrorMessage = isValid ? null : "Chyba: Dokument XML není validní. Zkontrolujte správnost syntaxe a struktury",
        });
    }
}