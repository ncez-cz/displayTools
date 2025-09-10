using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Encounter;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Observation;

public class CzLaboratoryObservation(List<XmlDocumentNavigator> items) : Widget
{
    public const string XPathCondition =
        "f:category/f:coding[f:system/@value='http://terminology.hl7.org/CodeSystem/observation-category' and f:code/@value='laboratory'] and f:code/f:coding/f:system[@value='https://nclp.ncez.mzcr.cz/CodeSystem/NCLPPOL']";

    public override async Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var table = new Table(
            [
                new TableHead([
                    new TableRow([
                        new TableCell([new ConstantText("Metoda")], TableCellType.Header),
                        new TableCell([new ConstantText("Výsledek")], TableCellType.Header,
                            containerClass: "border-end-0"),
                        new TableCell([new NullWidget()], TableCellType.Header, containerClass: "border-start-0"),
                        new TableCell([new ConstantText("Jednotka")], TableCellType.Header),
                        new TableCell([new ConstantText("Ref. meze")], TableCellType.Header),
                        new TableCell([new ConstantText("Hodnocení")], TableCellType.Header,
                            containerClass: "border-end-0"),
                        new TableCell([new NullWidget()], TableCellType.Header, containerClass: "border-start-0"),
                    ])
                ]),
                ..items.Select(x => new CzLaboratoryRowBuilder(x)),
            ]
        );

        return await table.Render(navigator, renderer, context);
    }

    private class CzLaboratoryRowBuilder(XmlDocumentNavigator item) : Widget
    {
        public override Task<RenderResult> Render(
            XmlDocumentNavigator navigator,
            IWidgetRenderer renderer,
            RenderContext context
        )
        {
            var rowDetails = new StructuredDetails();

            Widget[] tableRowContent =
            [
                new LabResultsRow(rowDetails),
            ];

            var infrequentProperties =
                InfrequentProperties.Evaluate<CzLabObservationInfrequentProperties>([item]);


            if (infrequentProperties.TryGet(CzLabObservationInfrequentProperties.CertifiedRefMaterialCodeableExtension,
                    out var certRefMatCodedPath))
            {
                rowDetails.Add(
                    new NameValuePairDetail(
                        new ConstantText("Certifikováná referenční látka"),
                        new ListBuilder(
                            certRefMatCodedPath,
                            FlexDirection.Column,
                            _ =>
                            [
                                new Container(
                                    new ChangeContext("f:valueCodeableConcept", new CodeableConcept()),
                                    ContainerType.Span
                                )
                            ])
                    )
                );
            }

            if (infrequentProperties.TryGet(CzLabObservationInfrequentProperties.CertifiedRefMaterialIdentiferExtension,
                    out var certRefMatIdPath))
            {
                rowDetails.Add(
                    new NameValuePairDetail(
                        new ConstantText("Identifikátor certifikováné referenční látky"),
                        new ListBuilder(
                            certRefMatIdPath,
                            FlexDirection.Column,
                            _ =>
                            [
                                new ShowIdentifier(),
                            ])
                    )
                );
            }

            if (item.EvaluateCondition("f:category"))
            {
                rowDetails.Add(
                    new NameValuePairDetail(
                        new ConstantText("Klasifikace"),
                        new CommaSeparatedBuilder("f:category",
                            _ =>
                            [
                                new CodeableConcept(),
                            ])
                    )
                );
            }

            // ignore subject

            if (infrequentProperties.TryGet(CzLabObservationInfrequentProperties.Focus, out var focusPath))
            {
                rowDetails.Add(
                    new NameValuePairDetail(
                        new ConstantText("Zaměřeno na"),
                        new CommaSeparatedBuilder(focusPath,
                            _ => [new AnyReferenceNamingWidget()])
                    )
                );
            }

            rowDetails.Add(
                new NameValuePairDetail(
                    new ConstantText("Čas"),
                    new Chronometry("effective")
                )
            );

            if (infrequentProperties.TryGet(CzLabObservationInfrequentProperties.Issued, out var issuedPath))
            {
                rowDetails.Add(
                    new NameValuePairDetail(
                        new ConstantText("Zpřístupněno"),
                        new ShowInstant(issuedPath)
                    )
                );
            }

            if (infrequentProperties.TryGet(CzLabObservationInfrequentProperties.BodySite, out var bodySiteXpath))
            {
                rowDetails.Add(
                    new NameValuePairDetail(
                        new DisplayLabel(LabelCodes.BodySite),
                        new ChangeContext(bodySiteXpath, new CodeableConcept())
                    )
                );
            }

            if (infrequentProperties.TryGet(CzLabObservationInfrequentProperties.Method, out var methodXpath))
            {
                rowDetails.Add(
                    new NameValuePairDetail(
                        new ConstantText("Způsob"),
                        new ChangeContext(methodXpath, new CodeableConcept())
                    )
                );
            }

            if (infrequentProperties.TryGet(CzLabObservationInfrequentProperties.SupportingInfoExtension,
                    out var supportInfoXpath))
            {
                rowDetails.AddCollapser(new ConstantText("Podpůrné údaje"), new ListBuilder(supportInfoXpath,
                    FlexDirection.Column, _ =>
                    [
                        ShowSingleReference.WithDefaultDisplayHandler(nav => [new AnyResource(nav, displayResourceType: false)],
                            "f:valueReference"),
                    ], separator: new LineBreak(), flexContainerClasses: string.Empty)
                );
            }

            if (infrequentProperties.TryGet(CzLabObservationInfrequentProperties.TriggeredByR5Extension,
                    out var triggeredByXpath))
            {
                rowDetails.AddCollapser(new ConstantText("Vyvoláno"), new ListBuilder(triggeredByXpath,
                    FlexDirection.Column,
                    (_, supportInfoNav) =>
                    {
                        var supportedSubExtensions = new List<Widget>();
                        if (supportInfoNav.EvaluateCondition("f:extension[@url='type']"))
                        {
                            supportedSubExtensions.Add(
                                new NameValuePair(
                                    new ConstantText("Typ"),
                                    new CommaSeparatedBuilder("f:extension[@url='type']", _ =>
                                    [
                                        new EnumLabel("f:valueCode",
                                            "http://hl7.org/fhir/ValueSet/observation-triggeredbytype"),
                                    ])
                                )
                            );
                        }

                        if (supportInfoNav.EvaluateCondition("f:extension[@url='reason']"))
                        {
                            supportedSubExtensions.Add(
                                new NameValuePair(
                                    new ConstantText("Důvod"),
                                    new CommaSeparatedBuilder("f:extension[@url='reason']", _ =>
                                    [
                                        new Text("f:valueString/@value"),
                                    ])
                                )
                            );
                        }

                        if (supportInfoNav.EvaluateCondition("f:extension[@url='observation']"))
                        {
                            supportedSubExtensions.Add(new ListBuilder("f:extension[@url='observation']",
                                FlexDirection.Column, _ =>
                                [
                                    ShowSingleReference.WithDefaultDisplayHandler(
                                        nav => [new CzLaboratoryObservation([nav])],
                                        "f:valueReference"),
                                ]));
                        }

                        return supportedSubExtensions;
                    }, separator: new LineBreak(), flexContainerClasses: string.Empty));
            }

            if (infrequentProperties.TryGet(CzLabObservationInfrequentProperties.LabTestKitExtension,
                    out var labTestKitPath))
            {
                rowDetails.AddCollapser(new ConstantText("Laboratorní testovací sada"), new ConcatBuilder(
                    labTestKitPath, _ =>
                    [
                        ShowSingleReference.WithDefaultDisplayHandler(
                            _ =>
                            [
                                new Collapser([
                                    new Choose([
                                        new When("f:deviceName/f:name", new Text("f:deviceName/f:name/@value"))
                                    ], new ConstantText("Zařízení")),
                                ], [], [new DeviceTextInfo()], iconPrefix: [new NarrativeModal()])
                            ],
                            "f:valueReference"),
                    ], separator: new LineBreak())
                );
            }

            if (infrequentProperties.TryGet(CzLabObservationInfrequentProperties.BasedOn, out var basedOnPath))
            {
                rowDetails.AddCollapser(new ConstantText("Na základě"), new ListBuilder(basedOnPath,
                    FlexDirection.Column, _ =>
                    [
                        ShowSingleReference.WithDefaultDisplayHandler(nav => [new AnyResource(nav, displayResourceType: false)]),
                    ], separator: new LineBreak())
                );
            }

            if (infrequentProperties.TryGet(CzLabObservationInfrequentProperties.PartOf, out var partOfPath))
            {
                rowDetails.AddCollapser(new ConstantText("Součástí"), new ListBuilder(partOfPath, FlexDirection.Column,
                    _ =>
                    [
                        ShowSingleReference.WithDefaultDisplayHandler(nav => [new AnyResource(nav, displayResourceType: false)]),
                    ], separator: new LineBreak())
                );
            }


            rowDetails.AddCollapser(new ConstantText("Provedl"), new ListBuilder("f:performer", FlexDirection.Column,
                _ =>
                [
                    new HideableDetails(
                        new Optional(
                            "f:extension[@url='http://hl7.org/fhir/StructureDefinition/event-performerFunction']",
                            new NameValuePair(
                                new ConstantText("Funkce"),
                                new ChangeContext("f:valueCodeableConcept", new CodeableConcept())
                            )
                        )
                    ),
                    ShowSingleReference.WithDefaultDisplayHandler(nav => [new AnyResource(nav, displayResourceType: false)]),
                ], flexContainerClasses: "gap-0"));

            // ignore note


            if (infrequentProperties.TryGet(CzLabObservationInfrequentProperties.Specimen, out var specimenXpath))
            {
                rowDetails.Add(
                    new RawDetail(
                        ShowSingleReference.WithDefaultDisplayHandler(
                            nav => [new AnyResource(nav, displayResourceType: false)], specimenXpath)
                    )
                );
            }

            if (infrequentProperties.TryGet(CzLabObservationInfrequentProperties.Device, out var deviceXpath))
            {
                rowDetails.Add(
                    new TextDetail(
                        new ConstantText(""),
                        ShowSingleReference.WithDefaultDisplayHandler(
                            nav =>
                            [
                                new Collapser([
                                    new Choose([
                                        new When("f:deviceName/f:name", new Text("f:deviceName/f:name/@value"))
                                    ], new ConstantText("Zařízení")),
                                ], [], [new DeviceTextInfo()], iconPrefix: [new NarrativeModal()])
                            ], deviceXpath)
                    )
                );
            }

            if (infrequentProperties.TryGet(CzLabObservationInfrequentProperties.HasMember, out var hasMemberXpath))
            {
                rowDetails.Add(
                    new TextDetail(
                        new ConstantText("Podzáznamy"),
                        new ShowMultiReference(hasMemberXpath, displayResourceType: false),
                        "resource-container"
                    )
                );
            }

            if (infrequentProperties.TryGet(CzLabObservationInfrequentProperties.DerivedFrom, out var derivedFromXpath))
            {
                rowDetails.Add(
                    new TextDetail(
                        new ConstantText("Odvozeno z"),
                        new ShowMultiReference(derivedFromXpath, displayResourceType: false),
                        "resource-container"
                    )
                );
            }

            if (item.EvaluateCondition("f:encounter"))
            {
                var encounterNarrative = ReferenceHandler.GetSingleNodeNavigatorFromReference(item,
                    "f:encounter", "f:text");

                rowDetails.AddCollapser(new ConstantText(Labels.Encounter),
                    body: ShowSingleReference.WithDefaultDisplayHandler(nav => [new EncounterCard(nav, false, false)],
                        "f:encounter"),
                    footer: encounterNarrative != null ? [new NarrativeCollapser(encounterNarrative.GetFullPath())] : null,
                    narrativeContent: encounterNarrative != null ? new NarrativeModal(encounterNarrative.GetFullPath()) : null
                );
            }

            if (item.EvaluateCondition("f:text"))
            {
                rowDetails.AddCollapser(new DisplayLabel(LabelCodes.OriginalNarrative), new Narrative("f:text"));
            }

            List<Widget> rowWidgets =
            [
                new TableRow(tableRowContent, rowDetails, idSource: item),
            ];
            if (item.EvaluateCondition("f:component"))
            {
                foreach (var componentNav in item.SelectAllNodes("f:component"))
                {
                    var componentRowDetails = new StructuredDetails();
                    var rowContent = new ChangeContext(componentNav, new LabResultsRow(componentRowDetails));
                    var componentRow = new TableRow([rowContent], componentRowDetails,
                        optionalClass: "sub-component-row");
                    rowWidgets.Add(componentRow);
                }
            }

            return new TableBody(rowWidgets).Render(item, renderer, context);
        }
    }

    private class LabResultsRow(StructuredDetails rowDetails) : Widget
    {
        public override Task<RenderResult> Render(
            XmlDocumentNavigator navigator,
            IWidgetRenderer renderer,
            RenderContext context
        )
        {
            var interpretationCodingVal =
                navigator.SelectAllNodes("f:interpretation/f:coding/f:code/@value").Select(x => x.Node).ToList()
                    .WhereNotNull()
                    .Select(x => x.Value)
                    .ToList();
            var trendIcons = interpretationCodingVal.Select(GetTrendIcon).ToList().WhereNotNull().ToList();
            var normalityClasses = interpretationCodingVal.Select(GetNormalityClass).WhereNotNull().ToList();

            var statusIcon = new EnumIconTooltip("f:status", "http://hl7.org/fhir/observation-status",
                new DisplayLabel(LabelCodes.Status));

            Widget[] widgetTree =
            [
                new TableCell([new ChangeContext("f:code", new CodeableConcept())]),
                new TableCell([
                    // displaying multiple normality background colors makes no sense
                    new If(_ => normalityClasses.Count == 1, new LazyWidget(() =>
                    [
                        new Container(
                            new OpenTypeElement(rowDetails, hints: OpenTypeElementRenderingHints.HideQuantityUnit),
                            ContainerType.Span, optionalClass: "lab-abnormal-value " + normalityClasses.First()),
                    ])).Else(
                        new OpenTypeElement(rowDetails, hints: OpenTypeElementRenderingHints.HideQuantityUnit)
                    )
                ], optionalClass: "text-center", containerClass: "border-end-0"),
                new TableCell([
                    new If(_ => trendIcons.Count == 1,
                        new LazyWidget(() => [trendIcons.First()])), // displaying multiple trend icons makes no sense
                ], containerClass: "border-start-0"),
                new TableCell([new ShowQuantityUnit("f:valueQuantity")]),
                new TableCell([
                    new ConcatBuilder("f:referenceRange", _ => [new ShowReferenceRange()], new LineBreak())
                ]),
                new TableCell([new ConcatBuilder("f:interpretation", _ => [new CodeableConcept()], new LineBreak())],
                    containerClass: "border-end-0"),
                new TableCell([statusIcon], containerClass: "border-start-0"),
            ];

            return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
        }

        private string? GetNormalityClass(string? value)
        {
            switch (value)
            {
                case null:
                case "N":
                    return null;
                case "A":
                case "H":
                case "L":
                    return "abnormal";
                case "AA":
                case "HH":
                case "LL":
                case "HU":
                case "LU":
                    return "critical";
                default:
                    return null;
            }
        }

        private Icon? GetTrendIcon(string? value)
        {
            switch (value)
            {
                case null:
                    return null;
                case "B":
                    return new Icon(SupportedIcons.FaceSmile, "text-success-400");
                case "D":
                    return new Icon(SupportedIcons.StackedChevronDown, "text-info-400");
                case "U":
                    return new Icon(SupportedIcons.StackedChevronUp, "text-info-400");
                case "W":
                    return new Icon(SupportedIcons.FaceFrown, "text-alert-600");
                default:
                    return null;
            }
        }
    }

    public enum CzLabObservationInfrequentProperties
    {
        [Extension("http://hl7.org/fhir/StructureDefinition/workflow-supportingInfo")]
        SupportingInfoExtension,

        [Extension("http://hl7.org/fhir/5.0/StructureDefinition/extension-Observation.triggeredBy")]
        TriggeredByR5Extension,

        [Extension("http://hl7.eu/fhir/laboratory/StructureDefinition/observation-certifiedRefMaterialCodeable")]
        CertifiedRefMaterialCodeableExtension,

        [Extension("http://hl7.eu/fhir/laboratory/StructureDefinition/observation-certifiedRefMaterialIdentifer")]
        CertifiedRefMaterialIdentiferExtension,

        [Extension("http://hl7.eu/fhir/laboratory/StructureDefinition/observation-deviceLabTestKit")]
        LabTestKitExtension,

        BasedOn,

        PartOf,

        Focus,

        Issued,

        [OpenType("value")] Value,

        Interpretation,

        Note,

        BodySite,

        Method,

        Specimen,

        Device,

        HasMember,

        DerivedFrom,
    }

    private class ShowReferenceRange : Widget
    {
        public override Task<RenderResult> Render(
            XmlDocumentNavigator navigator,
            IWidgetRenderer renderer,
            RenderContext context
        )
        {
            Widget[] widgetTree =
            [
                new Condition("f:low and not(f:high)", new ConstantText(" ≥ ")),
                new Optional("f:low", new ShowQuantity()),
                new Condition("f:low and f:high", new ConstantText(" - ")),
                new Condition("not(f:low) and f:high", new ConstantText(" ≤ ")),
                new Optional("f:high", new ShowQuantity()),
                new Condition("f:low or f:high", new LineBreak()),
                new Optional("f:type", new NameValuePair([new ConstantText("Typ")], [new CodeableConcept()])),
                new Condition("f:appliesTo",
                    new NameValuePair([new ConstantText("Aplikovatelné pro")],
                        [new ConcatBuilder("f:appliesTo", _ => [new CodeableConcept()], ", ")])),
                new Optional("f:age", new NameValuePair([new ConstantText("Věk")], [new ShowRange()])),
                new Optional("f:text",
                    new NameValuePair([new ConstantText("Textová reprezentace")], [new Text("@value")])),
            ];

            return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
        }
    }
}