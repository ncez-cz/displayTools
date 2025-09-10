using System.Text;
using Scalesoft.DisplayTool.Renderer.Models;

namespace Scalesoft.DisplayTool.Renderer.Validators.Cda;

public class CdaInternalXmlValidator : IDocumentValidator
{
    public InputFormat InputFormat => InputFormat.Cda;

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