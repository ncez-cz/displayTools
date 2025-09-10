using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Encounter;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class Conditions(
    List<XmlDocumentNavigator> items,
    Widget problemColumnLabel,
    bool skipIdPopulation = false
)
    : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var infrequentProperties =
            InfrequentProperties.Evaluate<ConditionInfrequentProperties>(items);

        var table = new Table(
            [
                new TableHead([
                    new TableRow([
                        new TableCell([problemColumnLabel], TableCellType.Header),
                        new If(_ => infrequentProperties.Contains(ConditionInfrequentProperties.Category),
                            new TableCell([new ConstantText("Typ problému")], TableCellType.Header)
                        ),
                        new If(_ => infrequentProperties.Contains(ConditionInfrequentProperties.Onset),
                            new TableCell([new DisplayLabel(LabelCodes.OnsetDate)], TableCellType.Header)
                        ),
                        new If(_ => infrequentProperties.Contains(ConditionInfrequentProperties.Abatement),
                            new TableCell([new ConstantText("Datum vyřešení/remise")], TableCellType.Header)
                        ),
                        new If(
                            _ => infrequentProperties.ContainsAnyOf(ConditionInfrequentProperties.BodySite,
                                ConditionInfrequentProperties.BodySiteExtension),
                            new TableCell([new DisplayLabel(LabelCodes.BodySite)], TableCellType.Header)
                        ),
                        new If(
                            _ => infrequentProperties.ContainsAnyOf(ConditionInfrequentProperties.ClinicalStatus,
                                ConditionInfrequentProperties.Severity),
                            new TableCell([new ConstantText("Další")], TableCellType.Header)
                        ),
                        new If(_ => infrequentProperties.Contains(ConditionInfrequentProperties.Text),
                            new NarrativeCell(false, TableCellType.Header)
                        ),
                    ])
                ]),
                ..items.Select(x => new TableBody([new ConditionRow(x, infrequentProperties, skipIdPopulation)])),
            ],
            true
        );
        return table.Render(navigator, renderer, context);
    }
}

public class ConditionRow(
    XmlDocumentNavigator item,
    InfrequentPropertiesData<ConditionInfrequentProperties> infrequentProperties,
    bool skipIdPopulation = false
) : Widget
{
    public override async Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var collapsibleRow = new StructuredDetails();

        if (item.EvaluateCondition("f:encounter"))
        {
            var encounterNarrative = ReferenceHandler.GetSingleNodeNavigatorFromReference(item,
                "f:encounter", "f:text");

            collapsibleRow.AddCollapser(new ConstantText(Labels.Encounter),
                ShowSingleReference.WithDefaultDisplayHandler(nav => [new EncounterCard(nav, false, false)],
                    "f:encounter"),
                encounterNarrative != null
                    ?
                    [
                        new NarrativeCollapser(encounterNarrative.GetFullPath())
                    ]
                    : null,
                encounterNarrative != null
                    ? new NarrativeModal(encounterNarrative.GetFullPath())
                    : null
            );
        }

        if (item.EvaluateCondition("f:text"))
        {
            collapsibleRow.AddCollapser(new DisplayLabel(LabelCodes.OriginalNarrative), new Narrative("f:text"));
        }

        var row = new TableRow([
            new TableCell([new CommaSeparatedBuilder("f:code", _ => [new CodeableConcept()])]),
            new If(_ => infrequentProperties.Contains(ConditionInfrequentProperties.Category),
                new TableCell([new CommaSeparatedBuilder("f:category", _ => [new CodeableConcept()])])
            ),
            new If(_ => infrequentProperties.Contains(ConditionInfrequentProperties.Onset),
                new TableCell([new Chronometry("onset")])
            ),
            new If(_ => infrequentProperties.Contains(ConditionInfrequentProperties.Abatement),
                new TableCell([new Chronometry("abatement")])
            ),
            new If(
                _ => infrequentProperties.ContainsAnyOf(ConditionInfrequentProperties.BodySite,
                    ConditionInfrequentProperties.BodySiteExtension),
                new TableCell(
                [
                    new Choose([
                        new When(
                            "f:extension[@url='http://hl7.org/fhir/StructureDefinition/bodySite']/f:valueReference",
                            ShowSingleReference.WithDefaultDisplayHandler(
                                nav => [new Container([new BodyStructure()], idSource: nav)],
                                "f:extension[@url='http://hl7.org/fhir/StructureDefinition/bodySite']/f:valueReference")),
                        new When("f:bodySite", new ChangeContext("f:bodySite", new CodeableConcept())),
                    ]),
                ])
            ),
            new If(
                _ => infrequentProperties.ContainsAnyOf(ConditionInfrequentProperties.ClinicalStatus,
                    ConditionInfrequentProperties.Severity),
                new TableCell([
                    new Concat([
                        new CommaSeparatedBuilder("f:clinicalStatus",
                            _ => [new CodeableConceptIconTooltip(new DisplayLabel(LabelCodes.ClinicalStatus))]),
                        new CommaSeparatedBuilder("f:severity",
                            _ => [new CodeableConceptIconTooltip(new DisplayLabel(LabelCodes.Severity))])
                    ])
                ])
            ),
            new If(_ => infrequentProperties.Contains(ConditionInfrequentProperties.Text),
                new NarrativeCell()
            )
        ], collapsibleRow, idSource: skipIdPopulation ? null : new IdentifierSource(item));

        var result = await row.Render(item, renderer, context);
        return result;
    }
}

public enum ConditionInfrequentProperties
{
    Category,
    BodySite,

    [Extension("http://hl7.org/fhir/StructureDefinition/bodySite")]
    BodySiteExtension,
    [OpenType("onset")] Onset,
    [OpenType("abatement")] Abatement,
    [EnumValueSet("")] ClinicalStatus,
    [EnumValueSet("")] Severity,
    Text
}