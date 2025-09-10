namespace Scalesoft.DisplayTool.Renderer.Widgets;

public class ViewModelBase
{
    public string? CustomClass { get; set; }
        
    public string? Id { get; set; }
    
    /// <summary>
    /// Used for visual distinction for element - when id is insufficient (i.e. grouping cells in a table row)
    /// </summary>
    public string? VisualId { get; set; }
}