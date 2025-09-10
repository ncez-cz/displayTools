using Scalesoft.DisplayTool.Renderer.Widgets;

namespace Scalesoft.DisplayTool.Renderer.Utils;

public static class WidgetSorter
{
    public static List<DateSortableWidget> Sort(List<DateSortableWidget> widgets)
    {
        return widgets
            .OrderBy(activity => activity.SortDate ?? DateTime.MaxValue)
            .ToList();
    }
}