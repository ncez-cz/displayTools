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
        : this(code, codedSectionBuilder, _ => severity, titleAbbreviations)
    {
    }

    public FhirSection() : this(null, (x, type) => new AnyResource(x, type), _ => null, null)
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

        var narrativeModal = new NarrativeModal();

        var tree = new ChangeContext(
            sectionNav,
            new MultiReference(navigators =>
            {
                var sectionContentWithTypes = BuildSectionContent(sectionNav, navigators, navigator, context, renderer);
                var anyStructuredContent = sectionContentWithTypes.ContentType.Any(x =>
                    x is SectionElements.Entries or SectionElements.EmptyReason or SectionElements.Subsection);
                Widget[] sectionContent;
                if (anyStructuredContent)
                {
                    sectionContent = [..sectionContentWithTypes.Content, new NarrativeCollapser()];
                }
                else
                {
                    sectionContent = [..sectionContentWithTypes.Content, new NarrativeCard()];
                }

                return new Section(
                    ".",
                    null,
                    [
                        // new Text("f:title/@value"),
                        new ChangeContext("f:code", new CodeableConcept()),
                    ],
                    sectionContent,
                    idSource: sectionNav,
                    titleAbbreviations: titleAbbreviations,
                    severity: getSeverity(navigators),
                    narrativeModal: anyStructuredContent ? narrativeModal : null
                );
            })
        );

        return await tree.Render(navigator, renderer, context);
    }

    private SectionContent BuildSectionContent(
        XmlDocumentNavigator sectionNav,
        List<XmlDocumentNavigator> entryNavs,
        XmlDocumentNavigator navigator,
        RenderContext context,
        IWidgetRenderer renderer
    )
    {
        var sectionContent = new List<Widget>();
        var sectionContentType = new List<SectionElements>();

        if (sectionNav.EvaluateCondition("f:author"))
        {
            sectionContentType.Add(SectionElements.Author);
            sectionContent.Add(new NameValuePair(
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
            ));
        }

        if (sectionNav.EvaluateCondition("f:focus"))
        {
            sectionContentType.Add(SectionElements.Focus);
            sectionContent.Add(new ChangeContext(
                "f:focus",
                new NameValuePair(
                    [new ConstantText("Subjekt")],
                    [ReferenceHandler.BuildAnyReferencesNaming(sectionNav, "f:focus", context, renderer)]
                )
            ));
        }

        if (entryNavs.Count != 0)
        {
            sectionContentType.Add(SectionElements.Entries);
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
            sectionContentType.Add(SectionElements.EmptyReason);
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
            sectionContentType.Add(SectionElements.Subsection);
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

        return new SectionContent(sectionContent, sectionContentType);
    }

    private enum SectionElements
    {
        Author,
        Focus,
        Entries,
        EmptyReason,
        Subsection
    }

    private class SectionContent(List<Widget> content, List<SectionElements> contentType)
    {
        public List<Widget> Content { get; } = content;
        public List<SectionElements> ContentType { get; } = contentType;
    }
}