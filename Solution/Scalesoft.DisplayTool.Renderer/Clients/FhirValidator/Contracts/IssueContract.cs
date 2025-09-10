namespace Scalesoft.DisplayTool.Renderer.Clients.FhirValidator.Contracts;

public class IssueContract
{
    public string? Source { get; set; }
    public int Line  { get; set; }
    public int Col { get; set; }
    public string? Location { get; set; }
    public string? Message { get; set; }
    public string? Type { get; set; }
    public string? Level { get; set; }
    public string? Html { get; set; }
    public bool SlicingHint { get; set; }
    public bool Signpost  { get; set; }
    public bool CriticalSignPost  { get; set; }
    public bool Matched { get; set; }
    public bool IgnorableError { get; set; }
    public string? InvId { get; set; }
    public int Count { get; set; }
    
}