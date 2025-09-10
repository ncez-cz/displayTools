namespace Scalesoft.DisplayTool.Service.DataContracts;

public class DisplayToolResponse
{
    public required byte[] Content { get; set; }

    public bool IsRenderedSuccessfully { get; set; }

    public List<string> Errors { get; set; } = [];

    public List<string> Warnings { get; set; } = [];
}