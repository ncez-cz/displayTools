using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

public class Row(
    IEnumerable<Widget> children,
    string? childContainerClasses = null,
    string? flexContainerClasses = null,
    bool? wrapChildren = false,
    ContainerType containerType = ContainerType.Div
) : FlexList(children, FlexDirection.Row, childContainerClasses, flexContainerClasses, wrapChildren,
    containerType: containerType);