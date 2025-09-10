using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.MedicationResources;

public class Ingredient : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        List<Widget> tree =
        [
            new CommaSeparatedBuilder("f:ingredient", (_, _, nav) => [
                new Container([
                    new Choose([
                        new When("f:itemCodeableConcept",
                            new Optional("f:itemCodeableConcept", new CodeableConcept()),
                            new Optional("f:strength", new ConstantText(" "), new ConstantText("("), new ShowRatio(), new ConstantText(")"))),
                        new When("f:itemReference",
                            new LineBreak(),
                            ShowSingleReference.WithDefaultDisplayHandler(x => [new Container([new Substance(x)], idSource: x)], "f:itemReference", "Substance"),
                            ShowSingleReference.WithDefaultDisplayHandler(x => [new Container([new MedicationInfo(x)], idSource: x)], "f:itemReference", "Medication"),
                            new Optional("f:strength", new ConstantText(" "),new ConstantText("("), new ShowRatio(), new ConstantText(")"))
                        ),
                    ]),
                ], idSource: nav)
            ]),
        ];
        
        return tree.RenderConcatenatedResult(navigator, renderer, context);  
    }
}
