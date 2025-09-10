using Scalesoft.DisplayTool.Renderer.Models.CDA;

namespace Scalesoft.DisplayTool.Renderer.Models;

public class ValidationResultModel
{
    public List<ValidationModel>? Validations { get; set; }
    
    public AdditionalCdaValidationModel? AdditionalCdaValidation { get; set; }
    
    public string? ErrorMessage { get; set; }
}