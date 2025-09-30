using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.PatientSection;

public class AnimalDetails : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        Widget[] tree =
        [
            new ChangeContext("f:extension[@url='species']/f:valueCodeableConcept",
                new Container([
                    new PlainBadge(new ConstantText("Druh zvířete")),
                    new Heading([new CodeableConcept()], HeadingSize.H3)
                ])
            ),
            new Optional("f:extension[@url='breed']/f:valueCodeableConcept",
                new Container([
                    new PlainBadge(new ConstantText("Plemeno")),
                    new Heading([new CodeableConcept()], HeadingSize.H3)
                ])
            ),
        ];

        return tree.RenderConcatenatedResult(navigator, renderer, context);
    }
}