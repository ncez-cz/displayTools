using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.FamilyMemberHistory;

/// <summary>
///     This Class is simplified backbone condition resource defined in Family member resource
/// </summary>
/// <param name="items"></param>
public class FamilyMemberCondition(List<XmlDocumentNavigator> items) : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var table = new Table(
            [
                new TableHead([
                    new TableRow([
                        new TableCell([new ConstantText("Problém")], TableCellType.Header),
                        new TableCell([new ConstantText("Důsledek")], TableCellType.Header),
                        new TableCell([new ConstantText("Počátek manifestace")], TableCellType.Header),
                        new If(_ => items.Any(x => x.EvaluateCondition("f:note")),
                            new TableCell([new ConstantText("Poznámka")], TableCellType.Header)),
                    ])
                ]),
                ..items.Select(x => new TableBody([new FamilyMemberConditionRowBuilder(x)])),
            ],
            true
        );

        return table.Render(navigator, renderer, context);
    }

    private class FamilyMemberConditionRowBuilder(XmlDocumentNavigator item) : Widget
    {
        public override async Task<RenderResult> Render(
            XmlDocumentNavigator navigator,
            IWidgetRenderer renderer,
            RenderContext context
        )
        {
            var rowDetails = new StructuredDetails();

            if (item.EvaluateCondition("f:text"))
            {
                rowDetails.AddCollapser(new DisplayLabel(LabelCodes.OriginalNarrative), new Narrative("f:text"));
            }

            var tableRowContent = new List<Widget>
            {
                //Condition suffered by relation
                new TableCell([
                    new Optional("f:code", new CodeableConcept()),
                ]),
                //Outcome && ContributedToDeath
                new TableCell([
                    new Optional("f:outcome", new CodeableConcept()),
                    new Optional("self::*[f:contributedToDeath and f:outcome]", new LineBreak()),
                    new Optional("f:contributedToDeath",
                        new NameValuePair([new ConstantText("Přispělo k úmrtí")],
                        [
                            new ShowBoolean(new DisplayLabel(LabelCodes.No), new DisplayLabel(LabelCodes.Yes)),
                        ])
                    ),
                ]),
                //When condition first manifested
                new TableCell([new OpenTypeElement(null, "onset")]), // Age | Range | Period | string
                new If(_ => item.EvaluateCondition("f:note"), new TableCell([
                    new ItemListBuilder("f:note", ItemListType.Unordered, _ => [new ShowAnnotationCompact()])
                ])),
            };

            var result =
                await new TableRow(tableRowContent, rowDetails, idSource: item).Render(item, renderer, context);

            var isCode = item.EvaluateCondition("f:code");
            if (!isCode)
            {
                result.Errors.Add(ParseError.MissingValue(item.SelectSingleNode("f:code").GetFullPath()));
            }

            return result;
        }
    }
}