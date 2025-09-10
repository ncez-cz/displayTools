using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class ShowEHdsiVaccineWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new WidgetWithVariables(new ShowCodedElementWidget(), [
new Variable("node", "$node"),
new Variable("xmlFile", "'1.3.6.1.4.1.12559.11.10.1.3.1.42.28.xml'"),
new Variable("codeSystem", "$node/@codeSystem"),
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
