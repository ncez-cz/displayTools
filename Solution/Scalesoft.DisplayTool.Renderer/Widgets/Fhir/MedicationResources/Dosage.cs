using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.MedicationResources;

public class Dosage(XmlDocumentNavigator item, string xpath = "f:dosageInstruction") : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var globalInfrequentProperties =
            InfrequentProperties.Evaluate<InfrequentPropertiesPaths>(item.SelectAllNodes(xpath).ToList());

        Widget dosageInstructionsTableCell = new ConcatBuilder(xpath, (_, _, nav) =>
        {
            var infrequentOptions =
                InfrequentProperties.Evaluate<InfrequentPropertiesPaths>([nav]);

            List<Widget> widgets =
            [
                new Row(
                    [
                        new Concat(
                        [
                            new If(_ => infrequentOptions.Contains(InfrequentPropertiesPaths.Text),
                                new ChangeContext("f:text", new Text("@value"))
                            ),
                            new If(_ => infrequentOptions.Contains(InfrequentPropertiesPaths.Route),
                                new NameValuePair(
                                    new DisplayLabel(LabelCodes.AdministrationRoute),
                                    new ChangeContext("f:route", new CodeableConcept())
                                )),
                            new If(_ => infrequentOptions.Contains(InfrequentPropertiesPaths.Timing),
                                new NameValuePair(
                                    new ConstantText("Dávkování"),
                                    new CommaSeparatedBuilder("f:timing", _ => [new ShowTiming()])
                                )),
                            new If(_ => infrequentOptions.Contains(InfrequentPropertiesPaths.AsNeeded),
                                children:
                                [
                                    new TextContainer(TextStyle.Bold,
                                        [
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
                                            ),
                                        ]
                                    ),
                                ]
                            ),
                        ]),
                        new Concat([
                            new If(_ => infrequentOptions.Contains(InfrequentPropertiesPaths.DoseAndRate),
                                new NameValuePair(
                                    new ConstantText("Podané množství"),
                                    new DoseAndRate()
                                )),
                            new If(_ => infrequentOptions.Contains(InfrequentPropertiesPaths.Sequence),
                                new NameValuePair(
                                    new ConstantText("Pořadí"),
                                    new Text("f:sequence/@value")
                                )),
                            new If(_ => infrequentOptions.Contains(InfrequentPropertiesPaths.AdditionalInstruction),
                                new NameValuePair(
                                    new ConstantText("Doplňující informace"),
                                    new CommaSeparatedBuilder("f:additionalInstruction",
                                        _ => [new CodeableConcept()])
                                )),
                        ]),
                    ],
                    flexContainerClasses: "column-gap-2",
                    childContainerClasses: "dosage-container",
                    wrapChildren: true
                ),
            ];

            return widgets.ToArray();
        }, new LineBreak());

        if (globalInfrequentProperties.Count == 0)
        {
            dosageInstructionsTableCell =
                new TextContainer(TextStyle.Muted, [new ConstantText("Informace nejsou k dispozici")]);
        }

        return dosageInstructionsTableCell.Render(item, renderer, context);
    }

    public enum InfrequentPropertiesPaths
    {
        Sequence,
        Timing,
        Route,
        DoseAndRate,
        [OpenType("asNeeded")] AsNeeded,
        AdditionalInstruction,
        Text,
    }
}