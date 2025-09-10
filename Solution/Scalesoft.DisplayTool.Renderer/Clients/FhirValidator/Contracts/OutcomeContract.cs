namespace Scalesoft.DisplayTool.Renderer.Clients.FhirValidator.Contracts;

public class OutcomeContract
{
    public FileInfoContract? FileInfo { get; set; }
    public List<IssueContract>? Issues { get; set; }
}