using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class Widget1 : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        List<Widget> widgetTree =
        [
            new WidgetWithVariables(new BasicCdaHeaderWidget(), [
            ]),
            new Heading([
                new Choose([
                    new When("/n1:ClinicalDocument/n1:code/@displayName",
                        new Text("/n1:ClinicalDocument/n1:code/@displayName")),
                    new When("string-length(/n1:ClinicalDocument/n1:title)  >= 1",
                        new Text("/n1:ClinicalDocument/n1:title")),
                ], new ConstantText(@"Clinical Document")),
            ]),
            new Button(onClick: "expandOrCollapseAllSections();", variant: ButtonVariant.CollapseSection,
                style: ButtonStyle.Outline),
            new Choose([
                new When("$documentCode='60591-5'", new WidgetWithVariables(new PsCdaWidget(), [
                ])),
                new When("$documentCode='57833-6'", new LineBreak()),
            ]),
        ];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}