namespace Scalesoft.DisplayTool.Renderer.Widgets;

/// <summary>
/// Defines a widget that can be sorted based on a date.
/// </summary>
public abstract class DateSortableWidget : Widget
{
    /// <summary>
    /// Gets the date used for sorting the widget.
    /// Returns null if the widget cannot be reliably sorted by date.
    /// </summary>
    public abstract DateTime? SortDate { get; set; }
}