using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Encounter;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.MedicationResources.MedicationDispenseSection;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.MedicationResources.MedicationRequestSection;

public class MedicationRequest : Widget
{
    public override async Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var infrequentProperties = InfrequentProperties.Evaluate<InfrequentProps>([navigator]);

        var widget = new Concat([
            new Collapser(
                title: [],
                getSeverity: () =>
                {
                    if (navigator.EvaluateCondition("f:doNotPerform[@value='true']"))
                    {
                        return Severity.Gray;
                    }

                    var priorityRaw = navigator
                        .SelectSingleNode("f:priority/@value")
                        .Node?.Value
                        .ToEnum<Priority>();

                    return priorityRaw switch
                    {
                        Priority.Urgent => Severity.Secondary,
                        Priority.Asap => Severity.Warning,
                        Priority.Stat => Severity.Error,
                        _ => Severity.Primary,
                    };
                },
                toggleLabelTitle:
                [
                    new Row(
                    [
                        new TextContainer(TextStyle.Regular, [
                            new Condition("f:doNotPerform[@value='true']", new TextContainer(TextStyle.Bold, [
                                new ConstantText("Tato medikace nesmí být vydána!"),
                            ], optionalClass: "severity-error severity-color")),
                            new NameValuePair(
                                new EnumLabel("f:intent", "http://hl7.org/fhir/ValueSet/medicationrequest-intent"),
                                new OpenTypeElement(null, "medication")
                            ),
                        ], optionalClass: "d-flex gap-2"),
                        new If(
                            predicate: nav =>
                                nav.EvaluateCondition($"f:priority[@value != '{Priority.Routine.ToEnumString()}']")
                                && (infrequentProperties.Contains(InfrequentProps.DoNotPerform) == false ||
                                    nav.EvaluateCondition("f:doNotPerform[@value!='true']")
                                ),
                            children:
                            [
                                new TextContainer(TextStyle.Bold | TextStyle.Uppercase, [
                                    new EnumLabel("f:priority", "http://hl7.org/fhir/ValueSet/request-priority"),
                                ]),
                            ]
                        ),
                    ], containerType: ContainerType.Span, flexContainerClasses: "justify-content-between gap-10"),
                ],
                content:
                [
                    new Row([
                        new MedicationRequestMedicationContainer("flex-grow-0 flex-shrink-1 flex-basis-auto"),
                        new If(_ => infrequentProperties.Contains(InfrequentProps.DispenseRequest),
                            new MedicationRequestDispenseContainer("flex-grow-0 flex-shrink-1 flex-basis-auto")
                        ),
                        new If(_ => infrequentProperties.Contains(InfrequentProps.DosageInstruction),
                            new ConcatBuilder(
                                itemsPath: "f:dosageInstruction",
                                itemBuilder: (_, _, item) =>
                                [
                                    new Container(
                                    [
                                        new Badge(new ConstantText("Instrukce")),
                                        new Dosage(item, "."),
                                    ], optionalClass: "medication-request-item"),
                                ]
                            )
                        ),
                    ], flexContainerClasses: "gap-5"),
                ],
                footer:
                [
                    new If(_ => infrequentProperties.Contains(InfrequentProps.Encounter),
                        new HideableDetails(ContainerType.Div,
                            new ShowMultiReference("f:encounter",
                                (items, _) => items.Select(Widget (x) => new EncounterCard(x)).ToList(),
                                x =>
                                [
                                    new Collapser(
                                        [new ConstantText(Labels.Encounter)],
                                        [],
                                        x.ToList(),
                                        isCollapsed: true),
                                ]
                            )
                        )
                    ),
                ],
                iconPrefix:
                [
                    new TextContainer(TextStyle.Muted, [
                        new ConstantText("Zažádáno: "),
                        new ShowDateTime("f:authoredOn"),
                    ]),
                    new Container([
                        new EnumIconTooltip("f:status", "http://hl7.org/fhir/ValueSet/medicationrequest-status",
                            new DisplayLabel(LabelCodes.Status)),
                    ], optionalClass: "d-flex align-items-center"),
                    new NarrativeModal(),
                ]
            ),
        ]);

        return await widget.Render(navigator, renderer, context);
    }

    private enum InfrequentProps
    {
        DispenseRequest,
        DosageInstruction,
        Encounter,
        DoNotPerform,
    }
}

public class MedicationRequestMedicationContainer(string? optionalClass = null) : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var infrequentProperties = InfrequentProperties.Evaluate<InfrequentProps>([navigator]);

        var widget = new Container([
            new Badge(new ConstantText("Medikace")),
            new If(_ => infrequentProperties.Contains(InfrequentProps.Reason),
                new NameValuePair(
                    new DisplayLabel(LabelCodes.MedicationReason),
                    new Concat([
                        new ConcatBuilder("f:reasonCode", _ =>
                            [
                                new CodeableConcept(),
                            ]
                        ),
                        new ConcatBuilder("f:reasonReference", _ =>
                            [
                                new AnyReferenceNamingWidget(),
                            ]
                        ),
                    ])
                )
            ),
            new If(_ => infrequentProperties.Contains(InfrequentProps.Substitution),
                new NameValuePair(
                    new ConstantText("Náhrada"),
                    new Choose([
                        new When("f:substitution/f:allowedBoolean",
                            new ShowBoolean(
                                new ConstantText("Není povolena"),
                                new ConstantText("Je povolena"),
                                "f:substitution/f:allowedBoolean"
                            )
                        ),
                        new When(
                            "f:substitution/f:allowedCodeableConcept",
                            new ChangeContext("f:substitution/f:allowedCodeableConcept",
                                new CodeableConcept()
                            )
                        ),
                    ])
                )
            ),
        ], optionalClass: optionalClass);

        return widget.Render(navigator, renderer, context);
    }

    private enum InfrequentProps
    {
        Substitution,
        [OpenType("reason")] Reason,
    }
}

public class MedicationRequestDispenseContainer(string? optionalClass = null) : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var infrequentProperties =
            InfrequentProperties.Evaluate<InfrequentProps>(
            [
                navigator.SelectSingleNode("f:dispenseRequest"),
            ]);

        var widget = new Container([
                new Badge(new ConstantText("Výdej")),
                new If(
                    _ => infrequentProperties.ContainsAnyOf(InfrequentProps.Quantity, InfrequentProps.ValidityPeriod),
                    new NameValuePair(
                        new DisplayLabel(LabelCodes.Dispensation),
                        new ChangeContext("f:dispenseRequest",
                            new TextContainer(TextStyle.Regular, [
                                new ShowQuantity("f:quantity"),
                                new If(
                                    _ => infrequentProperties.ContainsAllOf(InfrequentProps.Quantity,
                                        InfrequentProps.ValidityPeriod),
                                    new LineBreak()
                                ),
                                new ShowPeriod("f:validityPeriod"),
                            ])
                        )
                    )
                ),
                new Optional("f:requester",
                    new NameValuePair(
                        new ConstantText("Žadatel"),
                        new AnyReferenceNamingWidget()
                    )
                ),
            ], optionalClass: optionalClass
        );

        return widget.Render(navigator, renderer, context);
    }

    private enum InfrequentProps
    {
        Quantity,
        ValidityPeriod,
    }
}