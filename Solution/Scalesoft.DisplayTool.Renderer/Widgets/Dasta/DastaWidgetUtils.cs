namespace Scalesoft.DisplayTool.Renderer.Widgets.Dasta;

public class DastaWidgetUtils
{
    public static Widget TextOrTooltipWithText(Widget[] text, Widget[] tooltipContent, string tooltipContentPath)
    {
        return new Choose([new When(tooltipContentPath, [new Tooltip(text, tooltipContent)])], text);
    }
}
