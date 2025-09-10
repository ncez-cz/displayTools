using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class ShowEHdsiProcedureWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new WidgetWithVariables(new ShowCodedElementWidget(), [
new Variable("node", "$node"),
new Variable("xmlFile", "'1.3.6.1.4.1.12559.11.10.1.3.1.42.10.xml'"),
new Variable("codeSystem", "'2.16.840.1.113883.6.96'"),
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
