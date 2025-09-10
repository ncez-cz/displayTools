using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Encounter;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Observation;

public class ObservationCard(bool skipIdPopulation = false, bool hideObservationType = false) : Widget
{
    public const string PerformerFunctionExtensionUrl =
        "http://hl7.org/fhir/StructureDefinition/event-performerFunction";

    public const string ClinicallyRelevantTimeExtensionUrl =
        "https://hl7.cz/fhir/lab/StructureDefinition/cz-lab-clinically-relevant-time";

    public const string SupportingInfoExtensionUrl =
        "http://hl7.org/fhir/StructureDefinition/workflow-supportingInfo";

    public const string LabTestKitExtensionUrl =
        "http://hl7.eu/fhir/laboratory/StructureDefinition/observation-deviceLabTestKit";

    public const string CertifiedRefMaterialCodeableExtensionUrl =
        "http://hl7.eu/fhir/laboratory/StructureDefinition/observation-certifiedRefMaterialCodeable";

    public const string CertifiedRefMaterialIdentifierExtensionUrl =
        "http://hl7.eu/fhir/laboratory/StructureDefinition/observation-certifiedRefMaterialIdentifer";

    public const string TriggeredByExtensionUrl =
        "http://hl7.org/fhir/5.0/StructureDefinition/extension-Observation.triggeredBy";

    public override async Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var infrequentProperties =
            InfrequentProperties.Evaluate<ObservationInfrequentProperties>([navigator]);

        var collapsibleContent = new StructuredDetails();

        var headerInfo = new Container([
            new Container([
                new ConstantText("Pozorování"),
                new ConstantText(" ("),
                new ChangeContext("f:code", new CodeableConcept()),
                new ConstantText(")"),
            ], ContainerType.Span),
            new EnumIconTooltip("f:status", "http://hl7.org/fhir/ValueSet/observation-status",
                new DisplayLabel(LabelCodes.Status))
        ], ContainerType.Div, "d-flex align-items-center gap-1");

        var badge = new Badge(new ConstantText("Základní informace"));
        var basicInfo = new Container([
            new If(_ => infrequentProperties.Contains(ObservationInfrequentProperties.Category) && !hideObservationType,
                new NameValuePair(
                    new ConstantText("Druh pozorování"),
                    new CommaSeparatedBuilder("f:category", _ => [new CodeableConcept()])
                )
            ),
            new If(_ => infrequentProperties.Contains(ObservationInfrequentProperties.Effective),
                new NameValuePair(
                    [
                        new ConstantText("Datum"),
                        new OpenTypeChangeContext("effective",
                            new Optional($"f:extension[@url='{ClinicallyRelevantTimeExtensionUrl}']",
                                new ConstantText(" ("),
                                new ChangeContext("f:valueCoding", new Coding()),
                                new ConstantText(")")
                            )
                        ),
                    ],
                    [new Chronometry("effective")]
                )
            ),
            new If(_ => infrequentProperties.Contains(ObservationInfrequentProperties.Subject),
                new NameValuePair(
                    new ConstantText("Subjekt"),
                    new AnyReferenceNamingWidget("f:subject")
                )
            ),
            new If(_ => infrequentProperties.Contains(ObservationInfrequentProperties.Focus),
                new NameValuePair(
                    new ConstantText("Zaměřeno na"),
                    new ConcatBuilder("f:focus",
                        _ => [new AnyReferenceNamingWidget()], new LineBreak())
                )
            ),
            new If(_ => infrequentProperties.Contains(ObservationInfrequentProperties.BasedOn),
                new NameValuePair(
                    new ConstantText("Založeno na"),
                    new ConcatBuilder("f:basedOn",
                        _ => [new AnyReferenceNamingWidget()], new LineBreak())
                )
            ),
            new If(_ => infrequentProperties.Contains(ObservationInfrequentProperties.PartOf),
                new NameValuePair(
                    new ConstantText("Součástí"),
                    new ConcatBuilder("f:partOf",
                        _ => [new AnyReferenceNamingWidget()], new LineBreak()
                    )
                )
            ),
        ]);

        var additionalBadge = new Badge(new ConstantText("Další informace"));
        var additionalInfo = new Container([
            new If(_ => infrequentProperties.Contains(ObservationInfrequentProperties.SupportingInfo),
                new NameValuePair(
                    new ConstantText("Podpůrné informace"),
                    new ConcatBuilder(
                        $"f:extension[@url='{SupportingInfoExtensionUrl}']/f:valueReference",
                        _ => [new AnyReferenceNamingWidget()], new LineBreak()
                    )
                )
            ),
            new If(_ => infrequentProperties.Contains(ObservationInfrequentProperties.Issued),
                new NameValuePair(
                    new ConstantText("Vydáno"),
                    new ShowDateTime("f:issued")
                )
            ),
            new If(_ => infrequentProperties.Contains(ObservationInfrequentProperties.DerivedFrom),
                new NameValuePair(
                    new ConstantText("Odvozeno z"),
                    new ConcatBuilder("f:derivedFrom",
                        _ => [new AnyReferenceNamingWidget()], new LineBreak()
                    )
                )
            ),
        ]);

        var resultBadge = new Badge(new ConstantText("Výsledek"));
        var resultInfo = new Container([
            new If(_ => infrequentProperties.Contains(ObservationInfrequentProperties.Value),
                new NameValuePair(
                    new DisplayLabel(LabelCodes.Result),
                    new OpenTypeElement(collapsibleContent)
                )
            ),
            new If(_ => infrequentProperties.Contains(ObservationInfrequentProperties.Method),
                new NameValuePair(
                    new ConstantText("Metoda"),
                    new ChangeContext("f:method", new CodeableConcept())
                )
            ),
            new If(_ => infrequentProperties.Contains(ObservationInfrequentProperties.Performer),
                new NameValuePair(
                    new DisplayLabel(LabelCodes.Performer),
                    new ConcatBuilder("f:performer", _ =>
                    [
                        new Container([
                            new AnyReferenceNamingWidget(),
                            new Condition($"f:extension[@url='{PerformerFunctionExtensionUrl}']",
                                new ConstantText(" - "),
                                new TextContainer(TextStyle.Italic,
                                    new CommaSeparatedBuilder(
                                        $"f:extension[@url='{PerformerFunctionExtensionUrl}']/f:valueCodeableConcept",
                                        _ => new CodeableConcept()))
                            )
                        ], ContainerType.Span)
                    ], new LineBreak())
                )
            ),
            new If(_ => infrequentProperties.Contains(ObservationInfrequentProperties.Interpretation),
                new NameValuePair(
                    new ConstantText("Interpretace"),
                    new CommaSeparatedBuilder("f:interpretation", _ => [new CodeableConcept()])
                )
            ),
            new If(_ => infrequentProperties.Contains(ObservationInfrequentProperties.BodySite),
                new NameValuePair(
                    new DisplayLabel(LabelCodes.BodySite),
                    new ChangeContext("f:bodySite", new CodeableConcept())
                )
            ),
            new If(_ => infrequentProperties.Contains(ObservationInfrequentProperties.Specimen),
                new NameValuePair(
                    new ConstantText("Vzorek"),
                    new AnyReferenceNamingWidget("f:specimen")
                )
            ),
            new If(_ =>
                    infrequentProperties.ContainsAnyOf(ObservationInfrequentProperties.CertifiedRefMaterialCodeable,
                        ObservationInfrequentProperties.CertifiedRefMaterialIdentifier),
                new NameValuePair(
                    [
                        new ConstantText("Certifikovaný referenční materiál")
                    ],
                    [
                        new CommaSeparatedBuilder(
                            $"f:extension[@url='{CertifiedRefMaterialCodeableExtensionUrl}']/f:valueCodeableConcept",
                            _ => [new CodeableConcept()]),
                        new If(
                            _ => infrequentProperties.ContainsAllOf(
                                ObservationInfrequentProperties.CertifiedRefMaterialIdentifier,
                                ObservationInfrequentProperties.CertifiedRefMaterialCodeable), new ConstantText(", ")),
                        new CommaSeparatedBuilder(
                            $"f:extension[@url='{CertifiedRefMaterialIdentifierExtensionUrl}']/f:valueIdentifier",
                            _ => [new ShowIdentifier()])
                    ]
                )
            ),
        ]);

        var deviceBadge = new Badge(new ConstantText("Zařízení"));
        var deviceInfo =
            new If(_ =>
                    infrequentProperties.Contains(ObservationInfrequentProperties.LabTestKitExtension) ||
                    infrequentProperties.Contains(ObservationInfrequentProperties.Device),
                new ListBuilder(
                    $"f:extension[@url='{LabTestKitExtensionUrl}']/f:valueReference|f:device",
                    FlexDirection.Column, _ =>
                    [
                        ShowSingleReference.WithDefaultDisplayHandler(x =>
                        [
                            new Condition(
                                "f:manufacturer or f:deviceName or f:modelNumber or f:serialNumber or f:specialization or f:expirationDate",
                                new Card(
                                    new Concat([
                                        new Choose([
                                                new When("f:deviceName/f:name/@value",
                                                    new Row([
                                                        new Text("f:deviceName/f:name/@value"),
                                                        new NarrativeModal()
                                                    ], flexContainerClasses: "align-items-center")
                                                )
                                            ], new Row([
                                                new ConstantText("Zařízení"),
                                                new NarrativeModal()
                                            ], flexContainerClasses: "align-items-center")
                                        ),
                                        new EnumIconTooltip("f:status",
                                            "http://hl7.org/fhir/ValueSet/device-status-reason",
                                            new ConstantText("Stav zařízení")
                                        )
                                    ]),
                                    new Concat([
                                        new DeviceTextInfo(".", true)
                                    ]), idSource: x
                                )
                            )
                        ]),
                    ]
                )
            );

        var componentResultBadge = new Badge(new ConstantText("Složky výsledku"));
        var componentResultInfo = new Container([
            new ListBuilder("f:component", FlexDirection.Row, (i, nav) =>
            {
                var componentInfrequentProperties =
                    InfrequentProperties.Evaluate<ObservationInfrequentProperties>([nav]);

                var collapsibleContentComponent = new StructuredDetails();

                var componenetResultValue =
                    new If(_ => componentInfrequentProperties.Contains(ObservationInfrequentProperties.Value),
                        new NameValuePair(
                            new DisplayLabel(LabelCodes.Result),
                            new OpenTypeElement(collapsibleContentComponent)
                        )
                    );

                var card = new Card(null, new Container([
                        new NameValuePair(
                            new ConstantText("Kód"),
                            new ChangeContext("f:code", new CodeableConcept())
                        ),
                        componenetResultValue,
                        new If(
                            _ => componentInfrequentProperties.Contains(ObservationInfrequentProperties.Interpretation),
                            new NameValuePair(
                                new ConstantText("Interpretace"),
                                new CommaSeparatedBuilder("f:interpretation", _ => [new CodeableConcept()])
                            )
                        ),
                        new If(
                            _ => componentInfrequentProperties.Contains(ObservationInfrequentProperties.ReferenceRange),
                            new ReferenceRanges()
                        )
                    ]), footer: collapsibleContentComponent.Content.Count > 0
                        ? new Concat(collapsibleContentComponent.Build())
                        : null
                );

                return [card];
            })
        ]);

        if (infrequentProperties.Contains(ObservationInfrequentProperties.Text))
        {
            collapsibleContent.AddCollapser(new DisplayLabel(LabelCodes.OriginalNarrative), new Narrative("f:text"));
        }

        if (infrequentProperties.Contains(ObservationInfrequentProperties.Encounter))
        {
            var encounterNarrative = ReferenceHandler.GetSingleNodeNavigatorFromReference(navigator,
                "f:encounter", "f:text");

            collapsibleContent.AddCollapser(
                header: new ConstantText(Labels.Encounter),
                body: ShowSingleReference.WithDefaultDisplayHandler(
                    nav => [new EncounterCard(nav, false, false)],
                    "f:encounter"
                ),
                footer: encounterNarrative != null ? [new NarrativeCollapser(encounterNarrative.GetFullPath())] : null,
                narrativeContent: encounterNarrative != null ? new NarrativeModal(encounterNarrative.GetFullPath()) : null
            );
        }

        var complete =
            new Collapser([headerInfo], [], [
                    badge,
                    basicInfo,
                    new If(
                        _ => infrequentProperties.ContainsAnyOf(ObservationInfrequentProperties.SupportingInfo,
                            ObservationInfrequentProperties.Issued, ObservationInfrequentProperties.DerivedFrom),
                        new ThematicBreak(),
                        additionalBadge,
                        additionalInfo
                    ),
                    new If(_ => infrequentProperties.ContainsAnyOf(ObservationInfrequentProperties.Value,
                            ObservationInfrequentProperties.Method, ObservationInfrequentProperties.Performer),
                        new ThematicBreak(),
                        resultBadge,
                        resultInfo
                    ),
                    new If(_ => infrequentProperties.ContainsAnyOf(ObservationInfrequentProperties.Device,
                            ObservationInfrequentProperties.LabTestKitExtension),
                        new ThematicBreak(),
                        deviceBadge,
                        deviceInfo
                    ),
                    new If(_ => infrequentProperties.Contains(ObservationInfrequentProperties.ReferenceRange),
                        new ThematicBreak(),
                        new ReferenceRanges()
                    ),
                    new If(_ => infrequentProperties.Contains(ObservationInfrequentProperties.Component),
                        new ThematicBreak(),
                        componentResultBadge,
                        componentResultInfo
                    ),
                    InfrequentProperties.Condition(
                        infrequentProperties,
                        ObservationInfrequentProperties.TriggeredByExtension,
                        new ThematicBreak()
                    ),
                    InfrequentProperties.Optional(
                        new LineBreak(),
                        infrequentProperties,
                        ObservationInfrequentProperties.TriggeredByExtension,
                        new Badge(new ConstantText("Vyvoláno na základě")),
                        new Condition(
                            "f:extension[@url='observation']",
                            new NameValuePair(
                                new ConstantText("Pozorování"),
                                new CommaSeparatedBuilder(
                                    "f:extension[@url='observation']/f:valueReference",
                                    _ => new AnyReferenceNamingWidget()
                                )
                            )
                        ),
                        new Condition(
                            "f:extension[@url='type']",
                            new NameValuePair(
                                new ConstantText("Typ"),
                                new CommaSeparatedBuilder(
                                    "f:extension[@url='type']/f:valueCode",
                                    _ => new EnumLabel(".", "http://hl7.org/fhir/ValueSet/observation-triggeredbytype")
                                )
                            )
                        ),
                        new Condition(
                            "f:extension[@url='reason']",
                            new NameValuePair(
                                new ConstantText("Typ"),
                                new CommaSeparatedBuilder(
                                    "f:extension[@url='reason']/f:valueString",
                                    _ => new Text("@value")
                                )
                            )
                        )
                    ),
                    new If(_ => infrequentProperties.Contains(ObservationInfrequentProperties.HasMember),
                        new ThematicBreak(),
                        new Badge(new ConstantText("Podzáznamy")),
                        new Container([
                            new ShowMultiReference("f:hasMember", displayResourceType: false)
                        ])
                    ),
                    new If(_ => infrequentProperties.Contains(ObservationInfrequentProperties.Note),
                        new ThematicBreak(),
                        new Badge(new ConstantText("Poznámky")),
                        new LineBreak(),
                        new ConcatBuilder("f:note", _ => [new ShowAnnotationCompact()], new LineBreak())
                    ),
                ],
                footer: collapsibleContent.Content.Count > 0
                    ? collapsibleContent.Build()
                    : null,
                iconPrefix: [new NarrativeModal()],
                idSource: skipIdPopulation ? null : new IdentifierSource(navigator));
        return await complete.Render(navigator, renderer, context);
    }

    private class ReferenceRanges : Widget
    {
        public override Task<RenderResult> Render(
            XmlDocumentNavigator navigator,
            IWidgetRenderer renderer,
            RenderContext context
        )
        {
            var referenceRangeBadge = new Badge(new ConstantText("Referenční rozsahy"));
            var referenceRangeInfo = new Container([
                new ListBuilder("f:referenceRange", FlexDirection.Row, _ =>
                    [
                        new Card(null, new Container([
                            new Condition("f:low or f:high",
                                new NameValuePair(
                                    new ConstantText("Rozsah"),
                                    new ShowRange()
                                )
                            ),
                            new Optional("f:type",
                                new NameValuePair(
                                    new ConstantText("Typ"),
                                    new CodeableConcept()
                                )
                            ),
                            new Condition("f:appliesTo",
                                new NameValuePair(
                                    new ConstantText("Platí pro"),
                                    new CommaSeparatedBuilder("f:appliesTo", _ => [new CodeableConcept()])
                                )
                            ),
                            new Optional("f:age",
                                new NameValuePair(
                                    new ConstantText("Příslušný věk"),
                                    new ShowRange()
                                )
                            ),
                            new Optional("f:text",
                                new NameValuePair(
                                    new ConstantText("Rozsah textových referencí"),
                                    new Text("@value")
                                )
                            ),
                        ]))
                    ]
                )
            ]);

            return ((Widget[]) [referenceRangeBadge, referenceRangeInfo]).RenderConcatenatedResult(navigator, renderer,
                context);
        }
    }
}

public enum ObservationInfrequentProperties
{
    [OpenType("effective")] Effective,
    [OpenType("value")] Value,
    Performer,
    Subject,
    Category,
    Method,
    BasedOn,
    PartOf,
    Interpretation,
    Note,
    BodySite,
    Specimen,
    ReferenceRange,
    HasMember,
    DerivedFrom,
    Component,
    Text,
    Encounter,


    [Extension(ObservationCard.LabTestKitExtensionUrl)]
    LabTestKitExtension,
    Device,

    [Extension(ObservationCard.CertifiedRefMaterialCodeableExtensionUrl)]
    CertifiedRefMaterialCodeable,

    [Extension(ObservationCard.CertifiedRefMaterialIdentifierExtensionUrl)]
    CertifiedRefMaterialIdentifier,

    [Extension(ObservationCard.SupportingInfoExtensionUrl)]
    SupportingInfo,

    Focus,
    Issued,

    [Extension(ObservationCard.TriggeredByExtensionUrl)]
    TriggeredByExtension,
}