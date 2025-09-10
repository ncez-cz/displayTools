using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils;
using Scalesoft.DisplayTool.Renderer.Utils.Language;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

/// <summary>
///     Represents a section for rendering IPS (International Patient Summary) views, designed to process
///     and render coded sections based on provided navigators and associated data.
///     <br />
///     This widget expects to be called in the context of a Composition.
/// </summary>
/// <param name="codedSectionBuilder">
///     A delegate that builds the section's content.
///     If the section contains multiple resource types, codedSectionBuilder is called once for each resource type.
///     Takes a list of <see cref="XmlDocumentNavigator" />s and optionally the name of the current resource type, returns
///     a Widget.
/// </param>
/// <param name="code">Loinc code identifying this section</param>
public class FhirSection(
    string? code,
    Func<List<XmlDocumentNavigator>, string?, Widget> codedSectionBuilder,
    Func<List<XmlDocumentNavigator>, Severity?> getSeverity,
    LocalizedAbbreviations? titleAbbreviations
) : Widget
{
    
    public FhirSection(
        string code,
        Func<List<XmlDocumentNavigator>, string?, Widget> codedSectionBuilder,
        Severity? severity = null,
        LocalizedAbbreviations? titleAbbreviations = null
    )
        : this(code, codedSectionBuilder, (_) => severity, titleAbbreviations)
    {
    }

    public FhirSection() : this(null, (x, type) => new AnyResource(x, type), (_) => null, null)
    {
    }

    public override async Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var sectionNav = code == null
            ? navigator
            : navigator.SelectSingleNode(
                $"f:section[f:code/f:coding/f:system[@value='http://loinc.org'] and f:code/f:coding/f:code[@value='{code}']]"
            );
        if (sectionNav.Node == null)
        {
            return string.Empty;
        }

        var tree = new ChangeContext(
            sectionNav,
            new MultiReference(navigators =>
                new Section(
                    ".",
                    null,
                    [
                        // new Text("f:title/@value"),
                        new ChangeContext("f:code", new CodeableConcept()),
                    ],
                    BuildSectionContent(sectionNav, navigators, context, renderer),
                    idSource: sectionNav,
                    titleAbbreviations: titleAbbreviations,
                    severity: getSeverity(navigators),
                    narrativeTextPath: "f:text"
                )
            )
        );

        return await tree.Render(navigator, renderer, context);
    }

    private List<Widget> BuildSectionContent(
        XmlDocumentNavigator sectionNav,
        List<XmlDocumentNavigator> entryNavs,
        RenderContext context,
        IWidgetRenderer renderer
    )
    {
        var sectionContent = new List<Widget>(
            [
                new Condition(
                    "f:author",
                    new NameValuePair(
                        [new ConstantText("Autor sekce")],
                        [
                            new ConcatBuilder(
                                "f:author",
                                _ =>
                                [
                                    new ShowSingleReference(authNav =>
                                        {
                                            if (authNav.ResourceReferencePresent)
                                            {
                                                return
                                                [
                                                    new Container(
                                                        [new ActorsNaming()],
                                                        ContainerType.Span,
                                                        idSource: authNav.Navigator
                                                    ),
                                                ];
                                            }

                                            return [new ConstantText(authNav.ReferenceDisplay)];
                                        }
                                    ),
                                ], ", "
                            ),
                        ]
                    )
                ),
                new Optional(
                    "f:focus",
                    new NameValuePair(
                        [new ConstantText("Subjekt")],
                        [ReferenceHandler.BuildAnyReferencesNaming(sectionNav, "f:focus", context, renderer)]
                    )
                ),
            ]
        );

        if (entryNavs.Count != 0)
        {
            sectionContent.Add(
                new Container(
                    entryNavs
                        .GroupBy(n => n.Node?.Name)
                        // Process each different coded section separately
                        .Select(group =>
                            codedSectionBuilder(group.ToList(), group.Key)
                        )
                        .ToList()
                )
            );
        }
        else if (sectionNav.EvaluateCondition("f:emptyReason"))
        {
            sectionContent.Add(
                new Widgets.Alert(
                    new NameValuePair(
                        [new ConstantText("Důvod absence údajů")],
                        [new ChangeContext("f:emptyReason", new CodeableConcept())]
                    ),
                    Severity.Info
                )
            );
        }

        if (sectionNav.EvaluateCondition("f:section"))
        {
            sectionContent.Add(
                new ConcatBuilder(
                    "f:section",
                    _ =>
                    [
                        new FhirSection(),
                    ]
                )
            );
        }

        sectionContent.AddRange(
            [
                new NarrativeCollapser()
            ]
        );

        return sectionContent;
    }
}