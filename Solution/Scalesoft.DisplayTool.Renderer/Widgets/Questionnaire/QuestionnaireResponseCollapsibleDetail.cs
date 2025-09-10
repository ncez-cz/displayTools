using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Encounter;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Questionnaire;

public static class QuestionnaireResponseCollapsibleDetail
{
    public static StructuredDetails Build(XmlDocumentNavigator navigator)
    {
        var collapsibleRow = new StructuredDetails();

        if (navigator.EvaluateCondition("f:item"))
        {
            collapsibleRow.AddCollapser(
                new ConstantText("Položky"),
                new Condition("f:item", new QuestionnaireResponseItems())
            );
        }

        if (navigator.EvaluateCondition("f:encounter"))
        {
            var encounterNarrative = ReferenceHandler.GetSingleNodeNavigatorFromReference(navigator,
                "f:encounter", "f:text");

            collapsibleRow.AddCollapser(
                new ConstantText(Labels.Encounter),
                ShowSingleReference.WithDefaultDisplayHandler(nav => [new EncounterCard(nav, false, false)],
                    "f:encounter"),
                encounterNarrative != null
                    ?
                    [
                        new NarrativeCollapser(encounterNarrative.GetFullPath())
                    ]
                    : null,
                narrativeContent: encounterNarrative != null
                    ? new NarrativeModal(encounterNarrative.GetFullPath())
                    : null
            );
        }

        if (navigator.EvaluateCondition("f:text"))
        {
            collapsibleRow.AddCollapser(new DisplayLabel(LabelCodes.OriginalNarrative), new Narrative("f:text"));
        }

        return collapsibleRow;
    }
}

public class QuestionnaireResponseItems : Widget
{
    public override async Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var items = navigator.SelectAllNodes("f:item | f:answer/f:item").ToList();
        var infrequentProperties =
            InfrequentProperties.Evaluate<QuestionnaireItemInfrequentProperties>(items);


        var rows = new ConcatBuilder("f:item | f:answer/f:item",
            _ => [new QuestionnaireResponseItem(infrequentProperties)]);


        var tree = new Table([
            new TableHead([
                new TableRow([
                    new TableCell([new ConstantText("Číslo otázky")], TableCellType.Header),
                    new If(_ => infrequentProperties.Contains(QuestionnaireItemInfrequentProperties.Text),
                        new TableCell([new ConstantText("Otázka")], TableCellType.Header)),
                    new If(_ => infrequentProperties.Contains(QuestionnaireItemInfrequentProperties.Answer),
                        new TableCell([new ConstantText("Odpověď")], TableCellType.Header)),
                ]),
            ]),
            rows,
        ]);

        return await tree.Render(navigator, renderer, context);
    }
}

public class QuestionnaireResponseItem(
    InfrequentPropertiesData<QuestionnaireItemInfrequentProperties> infrequentProperties
) : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var hasChildren = navigator.EvaluateCondition("f:item or f:answer/f:item");

        var collapsibleRow = new StructuredDetails();
        if (hasChildren)
        {
            collapsibleRow.Add(
                new CollapsibleDetail(
                    new ConstantText("Podzáznamy"),
                    new QuestionnaireResponseItems()
                )
            );
        }

        var tree = new TableRow(
            [
                new TableCell([new Text("f:linkId/@value")]),
                new If(_ => infrequentProperties.Contains(QuestionnaireItemInfrequentProperties.Text),
                    new TableCell([new Text("f:text/@value")])),
                new If(_ => infrequentProperties.Contains(QuestionnaireItemInfrequentProperties.Answer),
                    new TableCell([
                        new Optional("f:answer",
                            new Container([new OpenTypeElement(null)],
                                idSource: new
                                    IdentifierSource())) // boolean | Decimal | Integer | Date | DateTime | Time | String | Uri | Attachment | Coding | Quantity | Reference(Any)
                    ])),
            ],
            collapsibleRow);

        return tree.Render(navigator, renderer, context);
    }
}

public enum QuestionnaireItemInfrequentProperties
{
    Text,
    Answer,
}