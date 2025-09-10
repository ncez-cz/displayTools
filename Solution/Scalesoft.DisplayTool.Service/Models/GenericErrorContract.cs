namespace Scalesoft.DisplayTool.Service.Models;

public class GenericErrorContract
{
    public string? Title { get; set; }

    public int Status { get; set; }

    public string? TraceId { get; set; }
}