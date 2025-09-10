using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

/// <summary>
///     Widget for rendering the following FHIR resources:
///     Practitioner, Patient, RelatedPerson, PractitionerRole, Organization.
///     Can also be used to render the "contact" BackboneElement of some resources.
/// </summary>
/// <param name="customSelectionRules">
///     This parameter is used to provide custom selection rules for the resource
///     configuration. If not provided, default rules will be used.
/// </param>
/// <param name="skipWhenInactive">
///     This parameter is used to skip rendering when the end of the "period" element is in the
///     past or "active" is false.
/// </param>
/// <param name="collapserTitle">
///     This parameter is used to provide a title for the collapser widget. If provided, the
///     widget will be wrapped in a collapser.
/// </param>
/// <param name="showNarrative">
///     This parameter dictates whether to add narrative text, keep in mind that if you turn this
///     off, it's expected you handle the narrative text some other way.
/// </param>
/// <param name="showCollapser">
///     This parameter dictates whether to put the data into a collapser with an automatically
///     selected title. If `collapserTitle` is set, this parameter is ignored.
/// </param>
public class PersonOrOrganization(
    XmlDocumentNavigator navigator,
    List<ResourceSelectionRule>? customSelectionRules = null,
    bool skipWhenInactive = false,
    Widget? collapserTitle = null,
    bool showNarrative = true,
    bool showCollapser = false,
    List<Widget>? collapserSubtitle = null
) : Widget
{
    public override async Task<RenderResult> Render(
        XmlDocumentNavigator _,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        List<ParseError> errors = [];

        var configManager = new ResourceConfiguration(customSelectionRules);

        var selectionRulesParseResult = configManager.ProcessConfigurations(navigator);
        errors.AddRange(selectionRulesParseResult.Errors);

        var selectionRules = selectionRulesParseResult.Results;

        var name = selectionRules.First(r => r.Name == ResourceNames.Name).FormattedPath;


        if (navigator.EvaluateCondition("f:period/f:end") && skipWhenInactive)
        {
            var periodEnd = navigator.SelectSingleNode("f:period/f:end/@value").Node?.ValueAsDateTime ??
                            DateTime.MaxValue;
            if (periodEnd < DateTime.Now)
            {
                return RenderResult.NullResult;
            }
        }

        if (navigator.EvaluateCondition("f:active[@value='false']") && skipWhenInactive)
        {
            return RenderResult.NullResult;
        }

        var photos = navigator.SelectAllNodes("f:photo").ToList();
        var newestPhoto = photos
            .Select(photo => new
            {
                Photo = photo,
                CreationDate = photo
                    .SelectSingleNode("f:creation/@value").Node?.ValueAsDateTime
            })
            .OrderByDescending(x => x.CreationDate.HasValue) // photos with date first
            .ThenByDescending(x => x.CreationDate) // newest first
            .Select(x => x.Photo) // back to original navigator
            .FirstOrDefault();

        List<Widget> tree =
        [
            new Container([
                new Container([
                    new Container([
                        new Container([
                            new Row([
                                new Choose([
                                    new When("f:name/@value",
                                        new Container([
                                            new Badge(new DisplayLabel(LabelCodes.Name)),
                                            new Heading([new Text("f:name/@value")], HeadingSize.H5)
                                        ])
                                    ),
                                    new When(name, new HumanName(name)),
                                ]),
                                new Optional("f:gender",
                                    new Container([
                                        new Badge(new DisplayLabel(LabelCodes.AdministrativeGender)),
                                        new LineBreak(),
                                        new EnumLabel(".", "http://hl7.org/fhir/ValueSet/administrative-gender")
                                    ])
                                ),
                            ], flexContainerClasses: "column-gap-4"),
                            new Row([
                                new If(_ => !showCollapser && navigator.EvaluateCondition("f:code"),
                                    new Container([
                                        new NameValuePair(
                                            new ConstantText("Role"),
                                            new CommaSeparatedBuilder("f:code", _ => [new CodeableConcept()])
                                        )
                                    ], optionalClass: "")
                                ),
                                new Optional("f:organization",
                                    new Container([
                                        new NameValuePair(
                                            new DisplayLabel(LabelCodes.RepresentedOrganization),
                                            new AnyReferenceNamingWidget()
                                        )
                                    ], optionalClass: "")
                                ),
                                new Condition("f:specialty",
                                    new Container([
                                        new NameValuePair(
                                            new ConstantText("Specializace"),
                                            new CommaSeparatedBuilder("f:specialty", _ => [new CodeableConcept()])
                                        )
                                    ], optionalClass: "")
                                ),
                                new Optional("f:practitioner",
                                    new Container([
                                        ShowSingleReference.WithDefaultDisplayHandler(x =>
                                            [
                                                new PersonOrOrganization(x,
                                                    collapserTitle: new ConstantText("Informace o lékaři"))
                                            ]
                                        ),
                                    ], optionalClass: "")
                                ),
                                new Optional("f:birthDate",
                                    new Container([
                                        new Badge(new DisplayLabel(LabelCodes.DateOfBirth)),
                                        new LineBreak(),
                                        new ShowDateTime()
                                    ], optionalClass: "")
                                ),
                                new Condition("f:qualification",
                                    new Container([
                                        new Badge(new ConstantText("Kvalifikace")),
                                        new ItemListBuilder(
                                            "f:qualification",
                                            ItemListType.Unordered, (_, x) =>
                                            {
                                                if (x.EvaluateCondition("f:period/f:end"))
                                                {
                                                    var periodEnd =
                                                        x.SelectSingleNode("f:period/f:end/@value").Node
                                                            ?.ValueAsDateTime ??
                                                        DateTime.MaxValue;
                                                    if (periodEnd < DateTime.Now)
                                                    {
                                                        return
                                                        [
                                                            new TextContainer(TextStyle.Strike,
                                                                new ChangeContext("f:code", new CodeableConcept()))
                                                        ];
                                                    }
                                                }

                                                var code = new ChangeContext("f:code",
                                                    new CodeableConcept()
                                                );

                                                return [code];
                                            }
                                        )
                                    ], optionalClass: "")
                                ),
                                new Condition("f:communication",
                                    new Container([
                                        new Badge(new ConstantText("Jazyky komunikace")),
                                        new ItemListBuilder(
                                            "f:communication",
                                            ItemListType.Unordered, _ =>
                                            [
                                                new Choose([
                                                    new When("f:language",
                                                        new ChangeContext("f:language", new CodeableConcept()))
                                                ], new CodeableConcept())
                                            ], orderer: elements =>
                                            {
                                                return elements
                                                    .OrderByDescending(e =>
                                                        e.EvaluateCondition("f:preferred[@value='true']"))
                                                    .ToList();
                                            }
                                        )
                                    ], optionalClass: "")
                                ),
                                new Container([
                                    new ContactInformation(),
                                ], optionalClass: ""),
                                new Condition("f:type",
                                    new Container([
                                        new Badge(new ConstantText("Druhy zařízení")),
                                        new ItemListBuilder(
                                            "f:type",
                                            ItemListType.Unordered, _ => [new CodeableConcept()]
                                        )
                                    ], optionalClass: "")
                                ),
                                new Optional("f:partOf",
                                    new Container([
                                        new Badge(new ConstantText("Součástí")),
                                        new AnyReferenceNamingWidget()
                                    ], optionalClass: "")
                                ),
                                new If(
                                    _ => navigator.EvaluateCondition("f:text") && collapserTitle == null &&
                                         !showCollapser && showNarrative,
                                    new Container([new NarrativeCollapser()], optionalClass: "")
                                )
                            ]),
                        ]),
                        new Condition("f:identifier", new Container([
                                new Container([
                                    new Badge(new ConstantText("Identifikátory")),
                                    new ListBuilder(
                                        "f:identifier",
                                        FlexDirection.Column, _ =>
                                        [
                                            new Container([
                                                new IdentifierSystemLabel(),
                                                new ConstantText(" "),
                                                new ShowIdentifier()
                                            ], ContainerType.Span)
                                        ], flexContainerClasses: "gap-0"
                                    ),
                                ])
                            ], optionalClass: "ms-auto"
                        )),
                    ], optionalClass: "d-flex column-gap-1"),
                ], optionalClass: newestPhoto == null ? "col-12" : "col-10-5"),
                new If(_ => newestPhoto != null,
                    new ChangeContext(newestPhoto!,
                        new Container([
                            new Attachment(onlyContentOrUrl: true, imageOptionalClass: "mw-100 person-photo")
                        ], ContainerType.Div, "col-1-5 d-flex justify-content-end")
                    )
                ),
            ], optionalClass: "row"),
        ];

        tree = collapserTitle != null || showCollapser
            ?
            [
                new Collapser(
                    toggleLabelTitle:
                    [
                        collapserTitle ?? new Choose([
                            new When("f:code", new CommaSeparatedBuilder("f:code", _ => [new CodeableConcept()])),
                        ], new LocalNodeName()),
                    ],
                    subtitle: collapserSubtitle,
                    title: [],
                    content: tree,
                    footer: navigator.EvaluateCondition("f:text") && showNarrative
                        ?
                        [
                            new NarrativeCollapser()
                        ]
                        : null,
                    iconPrefix: showNarrative ? [new NarrativeModal()] : null
                )
            ]
            : tree;

        var result = await tree.RenderConcatenatedResult(navigator, renderer, context);
        errors.AddRange(result.Errors);

        if (!result.HasValue || errors.MaxSeverity() >= ErrorSeverity.Fatal)
        {
            return errors;
        }

        return new RenderResult(result.Content, errors);
    }
}