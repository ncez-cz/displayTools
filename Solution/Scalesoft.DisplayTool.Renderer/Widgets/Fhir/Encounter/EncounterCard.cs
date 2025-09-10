using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Encounter;

public class EncounterCard(XmlDocumentNavigator navigator, bool displayAsCollapser = true, bool showNarrative = true)
    : Widget
{
    public override async Task<RenderResult> Render(
        XmlDocumentNavigator _,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var headerInfo = new Container([
            new ConstantText(Labels.Encounter),
            new EnumIconTooltip("f:status", "http://hl7.org/fhir/ValueSet/encounter-status",
                new DisplayLabel(LabelCodes.Status))
        ], ContainerType.Div, "d-flex align-items-center gap-1");

        var badge = new Badge(new ConstantText("Základní informace"));
        var basicInfo = new Container([
            new NameValuePair(
                new ConstantText("Zařazení"),
                new ChangeContext("f:class", new Coding())
            ),
            new Condition("f:statusHistory",
                new NameValuePair(
                    new ConstantText("Historie stavů"),
                    new ItemListBuilder("f:statusHistory", ItemListType.Unordered, _ =>
                    [
                        new Optional("f:status",
                            new EnumLabel(".", "http://hl7.org/fhir/ValueSet/encounter-status")),
                        new ConstantText(" - "),
                        new Optional("f:period", new ShowPeriod())
                    ])
                )
            ),
            new Optional("f:subject",
                new NameValuePair(
                    new ConstantText("Pacient/Skupina"),
                    new AnyReferenceNamingWidget()
                )
            ),
            new Condition("f:appointment",
                new NameValuePair(
                    new ConstantText("Schůzka"),
                    new ItemListBuilder("f:appointment", ItemListType.Unordered, _ =>
                        [new AnyReferenceNamingWidget()])
                )
            ),
            new Condition("f:reasonCode",
                new NameValuePair(
                    new ConstantText("Důvod"),
                    new CommaSeparatedBuilder("f:reasonCode", _ => [new CodeableConcept()])
                )
            ),
            new Optional("f:period",
                new NameValuePair(
                    new ConstantText("Perioda"),
                    new ShowPeriod()
                )
            ),
            new Optional("f:length",
                new NameValuePair(
                    new DisplayLabel(LabelCodes.Duration),
                    new ShowDuration()
                )
            )
        ]);

        var actorsBadge = new Badge(new ConstantText("Zainteresované strany"));
        var actorsInfo = new Container([
            new Optional("f:serviceProvider",
                new NameValuePair(
                    new ConstantText("Organizace"),
                    new AnyReferenceNamingWidget()
                )
            ),
            new Condition("f:account",
                new NameValuePair(
                    new ConstantText("Účet"),
                    new ItemListBuilder("f:account", ItemListType.Unordered, _ =>
                        [new AnyReferenceNamingWidget()])
                )
            ),
            new Condition("f:participant",
                new NameValuePair(
                    new ConstantText("Seznam účastníků"),
                    new ItemListBuilder("f:participant", ItemListType.Unordered, _ =>
                    [
                        new AnyReferenceNamingWidget("f:individual"),
                        new Optional("f:type",
                            new ConstantText(" - "),
                            new TextContainer(TextStyle.Italic, [
                                new CommaSeparatedBuilder(".", _ => [new CodeableConcept()]),
                            ])
                        )
                    ])
                )
            )
        ]);

        var additionalInfoBadge = new Badge(new ConstantText("Dodatečné informace"));
        var additionalInfo = new Container([
            new Condition("f:identifier",
                new NameValuePair(
                    new ConstantText("Identifikátor kontaktu"),
                    new CommaSeparatedBuilder("f:identifier", _ => [new ShowIdentifier()])
                )
            ),
            new Optional("f:priority",
                new NameValuePair(
                    new ConstantText("Priorita"),
                    new CodeableConcept()
                )
            ),
            new Condition("f:type",
                new NameValuePair(
                    new ConstantText("Typ"),
                    new CommaSeparatedBuilder("f:type", _ => [new CodeableConcept()])
                )
            ),
            new Optional("f:serviceType",
                new NameValuePair(
                    new ConstantText("Druh služby"),
                    new CodeableConcept()
                )
            ),
            // Související zdroje
            new Optional("f:partOf",
                new NameValuePair(
                    new ConstantText("Nadzáznam"),
                    new AnyReferenceNamingWidget()
                )
            ),
            new Condition("f:basedOn",
                new NameValuePair(
                    new ConstantText("Na základě"),
                    new ItemListBuilder("f:basedOn", ItemListType.Unordered, _ =>
                        [new AnyReferenceNamingWidget()])
                )
            ),
            new Condition("f:episodeOfCare",
                new NameValuePair(
                    new ConstantText("Epizoda péče"),
                    new ItemListBuilder("f:episodeOfCare", ItemListType.Unordered, _ =>
                        [new AnyReferenceNamingWidget()])
                )
            ),
            new Condition("f:reasonReference",
                new NameValuePair(
                    new ConstantText("Důvod"),
                    new ItemListBuilder("f:reasonReference", ItemListType.Unordered, _ =>
                        [new AnyReferenceNamingWidget()])
                )
            ),
        ]);
        var locationBadge = new Badge(new ConstantText("Lokace"));
        var locationInfo = new Container([
            new ItemListBuilder("f:location", ItemListType.Unordered, _ =>
                [
                    new NameValuePair(
                        new ConstantText("Místo"),
                        new AnyReferenceNamingWidget("f:location")
                    ),
                    new Optional("f:status",
                        new NameValuePair(
                            new DisplayLabel(LabelCodes.Status),
                            new EnumLabel(".", "http://hl7.org/fhir/ValueSet/location-status")
                        )
                    ),
                    new Optional("f:physicalType",
                        new NameValuePair(
                            new ConstantText("Druh místa"),
                            new CodeableConcept()
                        )
                    ),
                    new Optional("f:period",
                        new NameValuePair(
                            new DisplayLabel(LabelCodes.Duration),
                            new ShowPeriod()
                        )
                    )
                ]
            )
        ]);

        List<Widget> complete =
        [
            badge,
            basicInfo,
            new Condition("f:serviceProvider or f:account or f:participant",
                new ThematicBreak(),
                actorsBadge,
                actorsInfo
            ),
            new Condition(
                "f:identifier or f:priority or f:type or f:serviceType or f:partOf or f:basedOn or f:episodeOfCare or f:reasonReference",
                new ThematicBreak(),
                additionalInfoBadge,
                additionalInfo
            ),
            new Condition("f:location",
                new ThematicBreak(),
                locationBadge,
                locationInfo
            )
        ];

        if (!displayAsCollapser && showNarrative)
        {
            complete.Add(
                new NarrativeCollapser()
            );
        }

        Widget result =
            displayAsCollapser
                ? new Collapser([headerInfo], [], complete,
                    footer: navigator.EvaluateCondition("f:text") && showNarrative
                        ?
                        [
                            new NarrativeCollapser()
                        ]
                        : null,
                    iconPrefix: [new If(_ => showNarrative, new NarrativeModal())],
                    idSource: navigator, isCollapsed: true)
                : new Concat(complete);

        return await result.Render(navigator, renderer, context);
    }
}