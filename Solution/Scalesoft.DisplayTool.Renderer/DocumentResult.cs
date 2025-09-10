namespace Scalesoft.DisplayTool.Renderer;

public class DocumentResult
{
    public required byte[] Content { get; set; }

    public bool IsRenderedSuccessfully { get; set; }

    public List<string> Errors { get; set; } = [];

    public List<string> Warnings { get; set; } = [];
}