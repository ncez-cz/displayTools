using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.MedicationResources;

public class Substance(XmlDocumentNavigator item) : Widget
{
    
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        navigator = item;
        
        List<Widget> tree =
        [
            new Optional("f:code", new CodeableConcept()),
            new Condition("f:ingredient", new ConstantText(" - ")),
            new CommaSeparatedBuilder("f:ingredient", _ => [
                new Optional("f:quantity", new ShowRatio()),
                new Choose([
                    new When("f:substanceCodeableConcept", new Optional("f:itemCodeableConcept", new CodeableConcept())),
                    new When("f:substanceReference",
                        ShowSingleReference.WithDefaultDisplayHandler(x => [new Container([new Substance(x)], idSource: x)], "f:substanceReference")
                    ),
                ]),
            ]),
        ];
        
        return tree.RenderConcatenatedResult(navigator, renderer, context);  
    }
}
