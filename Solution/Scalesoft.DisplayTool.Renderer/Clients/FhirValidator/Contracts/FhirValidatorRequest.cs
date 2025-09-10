namespace Scalesoft.DisplayTool.Renderer.Clients.FhirValidator.Contracts;

public class FhirValidatorRequest
{
    public CliContextContract? CliContext { get; set; }
    public List<FileInfoContract>? FilesToValidate { get; set; }
    public string? SessionId { get; set; }
}