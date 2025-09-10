namespace Scalesoft.DisplayTool.Renderer.Utils;
public class TelecomMetadata(int? rank, int originalIndex)
{
    public int? Rank { get; } = rank;
    public int OriginalIndex { get; } = originalIndex;
}
