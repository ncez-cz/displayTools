namespace Scalesoft.DisplayTool.Renderer.Clients.FhirValidator.Contracts;

public class FhirValidatorResponse
{
    public List<OutcomeContract>? Outcomes { get; set; }
    public string? SessionId { get; set; }
    public string? ErrorMessage { get; set; }
}