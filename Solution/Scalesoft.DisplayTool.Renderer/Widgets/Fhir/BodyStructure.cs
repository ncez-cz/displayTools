using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class BodyStructure : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        List<Widget> tree =
        [
            new Choose([
                    // Show structured data, fallback to narrative text
                    new When(
                        "f:morphology or f:extension[@url='http://hl7.org/fhir/5.0/StructureDefinition/extension-BodyStructure.includedStructure.laterality']/f:valueCodeableConcept or f:location",
                        new Optional(
                            "f:morphology",
                            new NameValuePair([new ConstantText("Morfologie")],
                            [
                                new CodeableConcept()
                            ])
                        ),
                        new Optional(
                            "f:extension[@url='http://hl7.org/fhir/5.0/StructureDefinition/extension-BodyStructure.includedStructure.laterality']/f:valueCodeableConcept",
                            new NameValuePair([new ConstantText("Lateralita")],
                            [
                                new CodeableConcept()
                            ])),
                        new Condition(
                            "f:location",
                            new NameValuePair([new ConstantText("Lokalizace")],
                            [
                                new ConcatBuilder("f:locationQualifier", _ => [new CodeableConcept()], " "),
                                new ConstantText(" "),
                                new ChangeContext("f:location", new CodeableConcept()),
                            ])
                        )),
                ],
                new Optional("f:text", new Narrative())),
        ];
        return tree.RenderConcatenatedResult(navigator, renderer, context);
    }
}