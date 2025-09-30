using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.MedicationResources;

public class DosageCard(string path = "f:dosage", bool removeCardMargin = false) : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var widget = new Card(
            new ConstantText("Informace o dávkování"),
            new Concat([
                new ConcatBuilder(path, (_, _, nav) =>
                [
                    new FlexList([
                        new Optional("f:text",
                            new NameValuePair(
                                [new ConstantText("Instrukce")],
                                [
                                    new Text("@value"),
                                    new LineBreak(),
                                    new ChangeContext(nav,
                                        new Condition("f:asNeededCodeableConcept | f:asNeededBoolean",
                                            new Optional("f:asNeededCodeableConcept",
                                                new ConstantText("Brát dle potřeby - "), new CodeableConcept()),
                                            new Optional("f:asNeededBoolean",
                                                children:
                                                [
                                                    new ShowBoolean(
                                                        onFalse: new ConstantText("Brát dle instrukcí"),
                                                        onTrue: new ConstantText("Brát dle potřeby")
                                                    ),
                                                ]
                                            )
                                        )
                                    ),
                                ],
                                direction: FlexDirection.Column
                            )
                        ),
                        new Optional("f:route",
                            new NameValuePair(
                                new DisplayLabel(LabelCodes.AdministrationRoute),
                                new CodeableConcept(),
                                direction: FlexDirection.Column
                            )
                        ),
                        new Optional("f:timing",
                            new FlexList([
                                new ShowTiming(nameValuePairDirection: FlexDirection.Column)
                            ], FlexDirection.Row, flexContainerClasses: "column-gap-6")
                        ),
                        new Condition("f:doseAndRate",
                            new NameValuePair(
                                new ConstantText("Podané množství"),
                                new DoseAndRate(),
                                direction: FlexDirection.Column
                            )
                        ),
                        new Optional("f:sequence",
                            new NameValuePair(
                                new ConstantText("Pořadí"),
                                new Text("@value"),
                                direction: FlexDirection.Column
                            )
                        ),
                        new Condition("f:additionalInstruction",
                            new NameValuePair(
                                new ConstantText("Doplňující informace"),
                                new CommaSeparatedBuilder("f:additionalInstruction", _ => [new CodeableConcept()]),
                                direction: FlexDirection.Column
                            )
                        ),
                    ], FlexDirection.Row, flexContainerClasses: "column-gap-6 row-gap-1"),
                ], separator: new ThematicBreak()),
            ]), optionalClass: removeCardMargin ? "m-0" : null
        );

        return widget.Render(navigator, renderer, context);
    }
}