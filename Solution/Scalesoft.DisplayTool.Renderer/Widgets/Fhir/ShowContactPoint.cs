using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class ShowContactPoint(string telecomPath = "f:telecom") : Widget
{
    public override async Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        List<ParseError> errors = [];

        //Write out all the telecom contacts provided, but cross out the old ones?
        var telecomItems = navigator.SelectAllNodes(telecomPath)
            .Select((nav, index) =>
            {
                var order = index + 1;
                try
                {
                    if (!ResourceValidation.IsNodeCurrent(nav))
                    {
                        return null;
                    }
                }
                catch (FormatException e)
                {
                    errors.Add(
                        new ParseError
                        {
                            Kind = ErrorKind.InvalidValue,
                            Message = e.Message,
                            Path = nav.GetFullPath(),
                            Severity = ErrorSeverity.Warning
                        }
                    );
                }


                int? rankValue = null;
                var rankAttr = nav.SelectSingleNode("f:rank/@value").Node;
                if (rankAttr != null && int.TryParse(rankAttr.Value, out var parsedRank))
                {
                    rankValue = parsedRank;
                }

                return new TelecomMetadata(rankValue, order);
            })
            .WhereNotNull()
            .ToList();

        var sortedItems = telecomItems
            .OrderBy(item => item.Rank == null ? 1 : 0)
            .ThenBy(item => item.Rank)
            .ToList();

        var widgets = new List<Widget>();
        foreach (var item in sortedItems)
        {
            //Maybe when the telecom has no value don't display it at all?
            var telecom = $"{telecomPath}[{item.OriginalIndex}]";
            var widget =
                new ChangeContext(telecom,
                    new Container([
                        new Concat([
                            new Optional("f:system",
                                new EnumLabel("@value", "http://hl7.org/fhir/ValueSet/contact-point-system")
                            ),
                            new Condition("f:system and f:use",
                                new ConstantText(" ")
                            ),
                            new Optional("f:use",
                                new Concat([
                                    new TextContainer(TextStyle.Muted, [
                                        new ConstantText("("),
                                        new EnumLabel("@value", "http://hl7.org/fhir/ValueSet/contact-point-use"),
                                        new ConstantText(")")
                                    ])
                                ], string.Empty)
                            ),
                            new Condition("(f:system | f:use) and f:value",
                                new ConstantText(": ")
                            ),
                            new Optional("f:value", new Text("@value", optionalClass: "text-nowrap")),
                            new LineBreak()
                        ], string.Empty)
                    ], ContainerType.Span)
                );
            widgets.Add(widget);
        }

        var result = await widgets.RenderConcatenatedResult(navigator, renderer, context);
        errors.AddRange(result.Errors);

        if (!result.HasValue || errors.MaxSeverity() >= ErrorSeverity.Fatal)
        {
            return errors;
        }

        return new RenderResult(result.Content, errors);
    }
}