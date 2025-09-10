using System.Diagnostics.CodeAnalysis;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Encounter;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

public abstract record DetailItem(Widget Body, bool IsHideable);

public abstract record DetailItemWithHeader(Widget Header, Widget Body, bool IsHideable) : DetailItem(Body, IsHideable);

public record CollapsibleDetail(
    Widget Header,
    Widget Body,
    List<Widget>? Footer = null,
    Widget? ModalContent = null,
    bool IsHideable = true
) : DetailItemWithHeader(Header, Body, IsHideable);

public record NameValuePairDetail(
    Widget Header,
    Widget Body,
    bool IsHideable = false
) : DetailItemWithHeader(Header, Body, IsHideable);

public record TextDetail(
    Widget Header,
    Widget Body,
    string? CustomClass = null,
    bool IsHideable = false
) : DetailItemWithHeader(Header, Body, IsHideable);

public record RawDetail(
    Widget Content,
    bool IsHideable = true
) : DetailItem(Content, IsHideable);

public class StructuredDetails
{
    public List<DetailItem> Content { get; private set; }

    public StructuredDetails()
    {
        Content = [];
    }

    public StructuredDetails(IEnumerable<DetailItem> content)
    {
        Content = content.ToList();
    }

    public void Add(DetailItem item)
    {
        Content.Add(item);
    }

    public void AddCollapser(
        Widget header,
        Widget body,
        List<Widget>? footer = null,
        Widget? narrativeContent = null
    )
    {
        Content.Add(new CollapsibleDetail(header, body, footer, narrativeContent));
    }

    public void Concat(StructuredDetails other)
    {
        Content.AddRange(other.Content);
    }

    public List<Widget> Build() => [new LazyWidget(() => Build(this))];

    [return: NotNullIfNotNull(nameof(structuredDetails))]
    public static List<Widget>? Build(StructuredDetails? structuredDetails, bool hideNarrative = true)
    {
        if (structuredDetails == null)
        {
            return null;
        }

        var sortedDetailItems = structuredDetails.Content
            .OrderBy(detailItem =>
            {
                var type = detailItem.Body.GetType();
                return type == typeof(Narrative) || type == typeof(EncounterCard) ? 1 : 0;
            })
            .ToList();

        List<Widget> detailsWithWrapper = [];
        foreach (var detailItem in sortedDetailItems)
        {
            if (detailItem is DetailItemWithHeader detailWithHeader)
            {
                if (detailWithHeader.Header is not (DisplayLabel or ConstantText))
                {
                    throw new InvalidOperationException(
                        $"Unsupported key type: {detailWithHeader.Header.GetType().Name}. Only DisplayLabel and ConstantText are supported.");
                }
            }

            Widget fullCollapser = detailItem switch
            {
                CollapsibleDetail c => new Collapser(
                    [c.Header], [], [c.Body],
                    isCollapsed: c.Body is Narrative or EncounterCard && sortedDetailItems.Count > 1,
                    footer: c.Footer,
                    customClass: c.Body is Narrative && hideNarrative ? "narrative-print-collapser" : string.Empty,
                    iconPrefix: c.ModalContent != null ? [c.ModalContent] : null),

                NameValuePairDetail nvp => new NameValuePair(nvp.Header, nvp.Body),

                TextDetail t => new Container([
                    new TextContainer(TextStyle.Bold, t.Header),
                    t.Body,
                ], ContainerType.Div, t.CustomClass),

                RawDetail r => r.Content,

                _ => throw new ArgumentOutOfRangeException()
            };

            if (detailItem.IsHideable)
            {
                fullCollapser = new HideableDetails(ContainerType.Div, fullCollapser);
            }

            detailsWithWrapper.Add(fullCollapser);
        }

        return detailsWithWrapper;
    }
}