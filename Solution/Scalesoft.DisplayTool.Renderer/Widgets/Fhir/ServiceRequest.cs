using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Encounter;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class ServiceRequest : Widget
{
    public override async Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var infrequentProperties =
            InfrequentProperties.Evaluate<ServiceRequestInfrequentProperties>([navigator]);

        var headerInfo = new Container([
            new Container([
                new ConstantText("Žádost o službu"),
                new If(_ => infrequentProperties.Contains(ServiceRequestInfrequentProperties.Code),
                    new ConstantText(" ("),
                    new ChangeContext("f:code", new CodeableConcept()),
                    new ConstantText(")")
                ),
            ], ContainerType.Span),
            new EnumIconTooltip("f:status", "http://hl7.org/fhir/request-status",
                new DisplayLabel(LabelCodes.Status))
        ], ContainerType.Div, "d-flex align-items-center gap-1");

        var badge = new Badge(new ConstantText("Základní informace"));
        var basicInfo = new Container([
            new NameValuePair(
                new ConstantText("Záměr"),
                new EnumLabel("f:intent", "http://hl7.org/fhir/ValueSet/request-intent")
            ),
            new If(_ => infrequentProperties.Contains(ServiceRequestInfrequentProperties.Category),
                new NameValuePair(
                    new ConstantText("Kategorie"),
                    new CommaSeparatedBuilder("f:category", _ => [new CodeableConcept()])
                )
            ),
            new If(_ => infrequentProperties.Contains(ServiceRequestInfrequentProperties.Priority),
                new NameValuePair(
                    new ConstantText("Priorita"),
                    new EnumLabel("f:priority", "http://hl7.org/fhir/ValueSet/request-priority")
                )
            ),
            new If(_ => infrequentProperties.Contains(ServiceRequestInfrequentProperties.BodySite),
                new NameValuePair(
                    new DisplayLabel(LabelCodes.BodySite),
                    new ChangeContext("f:bodySite", new CodeableConcept())
                )
            ),
            new If(_ => infrequentProperties.Contains(ServiceRequestInfrequentProperties.Occurrence),
                new NameValuePair(
                    new ConstantText("Výskyt"),
                    new Chronometry("occurrence")
                )
            ),
            new If(_ => infrequentProperties.Contains(ServiceRequestInfrequentProperties.AuthoredOn),
                new NameValuePair(
                    new ConstantText("Datum a čas žádosti"),
                    new ShowDateTime("f:authoredOn")
                )
            ),
            new If(
                _ => infrequentProperties.Contains(ServiceRequestInfrequentProperties.DoNotPerform) &&
                     navigator.EvaluateCondition("f:doNotPerform[@value='true']"),
                new NameValuePair(
                    new ConstantText("Zákaz"),
                    new ShowDoNotPerform()
                )
            ),
            new NameValuePair(
                new ConstantText("Předmět"),
                new AnyReferenceNamingWidget("f:subject")
            ),
        ]);

        var additionalBadge = new Badge(new ConstantText("Další informace"));
        var additionalInfo = new Container([
            new If(_ => infrequentProperties.Contains(ServiceRequestInfrequentProperties.Replaces),
                new NameValuePair(
                    new ConstantText("Nahrazuje"),
                    new CommaSeparatedBuilder("f:replaces",
                        _ => [new AnyReferenceNamingWidget(showOptionalDetails: false)])
                )
            ),
            new If(_ => infrequentProperties.Contains(ServiceRequestInfrequentProperties.BasedOn),
                new NameValuePair(
                    new ConstantText("Založeno na"),
                    new CommaSeparatedBuilder("f:basedOn",
                        _ => [new AnyReferenceNamingWidget(showOptionalDetails: false)])
                )
            ),
            new If(_ => infrequentProperties.Contains(ServiceRequestInfrequentProperties.OrderDetail),
                new NameValuePair(
                    new ConstantText("Podrobnosti objednávky"),
                    new CommaSeparatedBuilder("f:orderDetail", _ => [new CodeableConcept()])
                )
            ),
            new If(_ => infrequentProperties.Contains(ServiceRequestInfrequentProperties.Quantity),
                new NameValuePair(
                    new ConstantText("Množství"),
                    new OpenTypeElement(null, "quantity") // Quantity | Ratio | Range
                )
            ),
            new If(_ => infrequentProperties.Contains(ServiceRequestInfrequentProperties.AsNeeded),
                new NameValuePair(
                    new ConstantText("Dle potřeby"),
                    new OpenTypeElement(null, "asNeeded") // 	boolean | CodeableConcept
                )
            ),
            new If(_ => infrequentProperties.Contains(ServiceRequestInfrequentProperties.Insurance),
                new NameValuePair(
                    new ConstantText("Pojištění"),
                    new CommaSeparatedBuilder("f:insurance",
                        _ => [new AnyReferenceNamingWidget(showOptionalDetails: false)])
                )
            ),
            new If(_ => infrequentProperties.Contains(ServiceRequestInfrequentProperties.SupportingInfo),
                new NameValuePair(
                    new ConstantText("Podpůrné informace"),
                    new CommaSeparatedBuilder("f:supportingInfo",
                        _ => [new AnyReferenceNamingWidget(showOptionalDetails: false)])
                )
            ),
            new If(_ => infrequentProperties.Contains(ServiceRequestInfrequentProperties.Specimen),
                new NameValuePair(
                    new ConstantText("Vzorky"),
                    new CommaSeparatedBuilder("f:specimen",
                        _ => [new AnyReferenceNamingWidget(showOptionalDetails: false)])
                )
            ),
            new If(_ => infrequentProperties.Contains(ServiceRequestInfrequentProperties.PatientInstruction),
                new NameValuePair(
                    new ConstantText("Pokyny pro pacienta"),
                    new Text("f:patientInstruction/@value")
                )
            ),
            new If(_ => infrequentProperties.Contains(ServiceRequestInfrequentProperties.RelevantHistory),
                new NameValuePair(
                    new ConstantText("Relevantní historie"),
                    new CommaSeparatedBuilder("f:relevantHistory",
                        _ => [new AnyReferenceNamingWidget(showOptionalDetails: false)])
                )
            ),
        ]);


        var serviceBadge = new Badge(new ConstantText("Požadavek"));
        var serviceInfo = new Container([
            new If(_ => infrequentProperties.Contains(ServiceRequestInfrequentProperties.Code),
                new NameValuePair(
                    new ConstantText("Žádaná služba"),
                    new ChangeContext("f:code", new CodeableConcept())
                )
            ),
            new If(
                _ => infrequentProperties.ContainsAnyOf(ServiceRequestInfrequentProperties.ReasonCode,
                    ServiceRequestInfrequentProperties.ReasonReference),
                new NameValuePair(
                    new ConstantText("Důvod žádosti"),
                    new CommaSeparatedBuilder("f:reasonCode|f:reasonReference",
                        (_, _, x) =>
                        {
                            return x.Node?.Name switch
                            {
                                "reasonCode" => [new CodeableConcept()],
                                "reasonReference" => [new AnyReferenceNamingWidget()],
                                _ => throw new InvalidOperationException()
                            };
                        }
                    )
                )
            ),
            new If(_ => infrequentProperties.Contains(ServiceRequestInfrequentProperties.Requester),
                new NameValuePair(
                    new ConstantText("Žadatel"),
                    new AnyReferenceNamingWidget("f:requester")
                )
            ),
            new If(_ => infrequentProperties.Contains(ServiceRequestInfrequentProperties.PerformerType),
                new NameValuePair(
                    new ConstantText("Typ vyřizovatele"),
                    new ChangeContext("f:performerType", new CodeableConcept())
                )
            ),
            new If(_ => infrequentProperties.Contains(ServiceRequestInfrequentProperties.Performer),
                new NameValuePair(
                    new ConstantText("Vyřizovatelé"),
                    new ListBuilder("f:performer", FlexDirection.Column,
                        _ => [new AnyReferenceNamingWidget(showOptionalDetails: false)])
                )
            ),
            new If(
                _ => infrequentProperties.ContainsAnyOf(ServiceRequestInfrequentProperties.LocationCode,
                    ServiceRequestInfrequentProperties.LocationReference),
                new NameValuePair(
                    new ConstantText("Místo"),
                    new CommaSeparatedBuilder("f:locationCode|f:locationReference",
                        (_, _, x) =>
                        {
                            return x.Node?.Name switch
                            {
                                "locationCode" => [new CodeableConcept()],
                                "locationReference" => [new AnyReferenceNamingWidget()],
                                _ => throw new InvalidOperationException()
                            };
                        }
                    )
                )
            )
        ]);

        var complete =
            new Collapser([headerInfo], [], [
                    badge,
                    basicInfo,
                    new If(
                        _ => infrequentProperties.ContainsAnyOf(ServiceRequestInfrequentProperties.Replaces,
                            ServiceRequestInfrequentProperties.OrderDetail, ServiceRequestInfrequentProperties.Quantity,
                            ServiceRequestInfrequentProperties.AsNeeded, ServiceRequestInfrequentProperties.Insurance,
                            ServiceRequestInfrequentProperties.SupportingInfo,
                            ServiceRequestInfrequentProperties.Specimen,
                            ServiceRequestInfrequentProperties.PatientInstruction,
                            ServiceRequestInfrequentProperties.RelevantHistory,
                            ServiceRequestInfrequentProperties.BasedOn),
                        new ThematicBreak(),
                        additionalBadge,
                        additionalInfo
                    ),
                    new If(_ => infrequentProperties.ContainsAnyOf(ServiceRequestInfrequentProperties.Performer,
                            ServiceRequestInfrequentProperties.ReasonCode,
                            ServiceRequestInfrequentProperties.Requester,
                            ServiceRequestInfrequentProperties.Performer),
                        new ThematicBreak(),
                        serviceBadge,
                        serviceInfo
                    ),
                    new If(_ => infrequentProperties.Contains(ServiceRequestInfrequentProperties.Note),
                        new ThematicBreak(),
                        new Badge(new ConstantText("Poznámky")),
                        new ListBuilder("f:note", FlexDirection.Column, _ => [new ShowAnnotationCompact()],
                            flexContainerClasses: "gap-0")
                    )
                ], footer: infrequentProperties.ContainsAnyOf(ServiceRequestInfrequentProperties.Encounter,
                    ServiceRequestInfrequentProperties.Text)
                    ?
                    [
                        new If(_ => infrequentProperties.Contains(ServiceRequestInfrequentProperties.Encounter),
                            new ShowMultiReference("f:encounter",
                                (items, _) => items.Select(Widget (x) => new EncounterCard(x)).ToList(),
                                x =>
                                [
                                    new Collapser([new ConstantText(Labels.Encounter)], [], x.ToList(),
                                        isCollapsed: true)
                                ]
                            )
                        ),
                        new If(_ => infrequentProperties.Contains(ServiceRequestInfrequentProperties.Text),
                            new NarrativeCollapser()
                        ),
                    ]
                    : null,
                iconPrefix: [new NarrativeModal()]
            );


        return await complete.Render(navigator, renderer, context);
    }
}

public enum ServiceRequestInfrequentProperties
{
    Requester,
    Performer,
    Priority,
    AuthoredOn,
    BodySite,
    [OpenType("occurrence")] Occurrence,
    ReasonCode,
    Category,
    Code,
    DoNotPerform,
    Text,
    Encounter,
    Replaces,
    OrderDetail,
    [OpenType("quantity")] Quantity,
    [OpenType("asNeeded")] AsNeeded,
    PerformerType,
    LocationCode,
    LocationReference,
    ReasonReference,
    Insurance,
    SupportingInfo,
    Specimen,
    Note,
    PatientInstruction,
    RelevantHistory,
    BasedOn
}