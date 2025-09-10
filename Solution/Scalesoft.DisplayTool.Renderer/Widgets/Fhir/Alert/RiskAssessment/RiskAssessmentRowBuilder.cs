using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Encounter;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Alert.RiskAssessment;

public class RiskAssessmentRowBuilder(
    XmlDocumentNavigator item,
    InfrequentPropertiesData<RiskAssessments.InfrequentProperties> infrequentProperties,
    InfrequentPropertiesData<RiskAssessments.InfrequentPredictionProperties> infrequentPredictionProperties
) : Widget
{
    public override async Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var rowDetails = new StructuredDetails();

        if (item.EvaluateCondition("f:condition"))
        {
            var conditionNavigator = ReferenceHandler.GetContentFromReferences(item, "f:condition");
            if (conditionNavigator.Any())
            {
                rowDetails.AddCollapser(
                    new ConstantText("Posuzovaný zdravotní stav"),
                    new Conditions(conditionNavigator, new DisplayLabel(LabelCodes.ActiveProblem))
                );
            }
        }

        if (item.EvaluateCondition("f:encounter"))
        {
            var encounterNarrative = ReferenceHandler.GetSingleNodeNavigatorFromReference(item,
                "f:encounter", "f:text");

            rowDetails.AddCollapser(
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

        if (item.EvaluateCondition("f:text"))
        {
            rowDetails.AddCollapser(new DisplayLabel(LabelCodes.OriginalNarrative), new Narrative("f:text"));
        }

        Widget tree = new TableRow([
            new If(_ => infrequentPredictionProperties.Count != 0,
                new RiskAssessmentPredictionCell(item)
            ),
            new If(_ => infrequentProperties.HasAnyOfGroup("BasicInfoCell"),
                new RiskAssessmentBasicInfoCell(item)
            ),
            new If(
                _ => infrequentProperties.HasAnyOfGroup("ActorsCell"),
                new RiskAssessmentActorsCell(item)
            ),
            new If(
                _ => infrequentProperties.HasAnyOfGroup("AdditionalInfoCell"),
                new RiskAssessmentAdditionalInfoCell(item)
            ),
            new If(_ => infrequentProperties.Contains(RiskAssessments.InfrequentProperties.Status),
                new TableCell([
                    new EnumIconTooltip("f:status", "http://hl7.org/fhir/observation-status",
                        new DisplayLabel(LabelCodes.Status))
                ])
            ),
            new If(_ => infrequentProperties.Contains(RiskAssessments.InfrequentProperties.Text),
                new NarrativeCell()
            )
        ], rowDetails, idSource: item);

        var result = await tree.Render(item, renderer, context);

        var isStatus = item.EvaluateCondition("f:status");
        if (!isStatus)
        {
            result.Errors.Add(ParseError.MissingValue(item.SelectSingleNode("f:status").GetFullPath()));
        }

        var isSubject = item.EvaluateCondition("f:subject");
        if (!isSubject)
        {
            result.Errors.Add(ParseError.MissingValue(item.SelectSingleNode("f:subject").GetFullPath()));
        }

        return result;
    }
}