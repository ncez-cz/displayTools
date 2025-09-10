using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class RootWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new GlobalContextWidget(),
new ChangeContext("n1:ClinicalDocument", new Widget1()),
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
