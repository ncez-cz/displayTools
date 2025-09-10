using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class ShowEHdsiNullFlavorWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        List<Widget> widgetTree =
        [
            new Container([
                new ConstantText(@" "),
                new WidgetWithVariables(new ShowCodeValueWidget(), [
                    new Variable("code", "$code"),
                    new Variable("xmlFile", "'1.3.6.1.4.1.12559.11.10.1.3.1.42.37.xml'"),
                    new Variable("codeSystem", "'2.16.840.1.113883.5.1008'"),
                ]),
                new Tooltip([], [
                    new Container([
                        new WidgetWithVariables(new ShowEHdsiDisplayLabelWidget(), [
                            new Variable("code", "'146'"),
                        ]),
                    ], ContainerType.Span),
                ])
            ], ContainerType.Div, "d-flex")
        ];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}