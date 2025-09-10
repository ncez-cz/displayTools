using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.MedicationResources;

public class DoseAndRate : Widget
{
    
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        List<Widget> tree =
        [
            new CommaSeparatedBuilder("f:doseAndRate", _ => [
                new Container([
                    new Optional("f:type", new CodeableConcept()),
                    new Choose([
                        new When("f:doseRange", new ShowRange("f:doseRange")),
                        new When("f:doseQuantity", new ConstantText(" "), new ShowQuantity("f:doseQuantity")),
                    ]),
                    new Choose([
                        new When("f:rateRatio", new Optional("f:rateRatio", new LineBreak(), new ShowRatio())),
                        new When("f:rateRange", new Optional("f:rateRange", new LineBreak(), new ShowRatio())),
                        new When("f:rateQuantity", new Optional("f:rateQuantity", new LineBreak(), new ShowRatio())),
                    ]),
                ], ContainerType.Span, idSource: new IdentifierSource(true))
            ]),
        ];
        
        return tree.RenderConcatenatedResult(navigator, renderer, context);  
    }
}