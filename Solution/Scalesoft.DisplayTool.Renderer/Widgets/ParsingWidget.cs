namespace Scalesoft.DisplayTool.Renderer.Widgets;

public abstract class ParsingWidget(string path) : Widget
{
    public string Path { get; } = path;
}