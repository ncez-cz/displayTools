using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Alert.DetectedIssues;

public class DetectedIssuesRowBuilder(
    XmlDocumentNavigator item,
    InfrequentPropertiesData<DetectedIssue.InfrequentProperties> infrequentProperties,
    InfrequentPropertiesData<DetectedIssue.InfrequentMitigationProperties> infrequentMitigationProperties
) : Widget
{
    public override Task<RenderResult> Render(
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

        List<Widget> tableRowContent =
        [
            new If(
                _ => infrequentProperties.HasAnyOfGroup("ProblemDetailCell"),
                new DetectedIssuesProblemDetailCell(item)
            ),
            new If(_ => infrequentMitigationProperties.Count != 0,
                new DetectedIssuesMitigationCell(item)
            ),
            new If(
                _ => infrequentProperties.HasAnyOfGroup("ActorsCell"),
                new DetectedIssuesActorsCell(item)
            ),
            new If(
                _ => infrequentProperties.HasAnyOfGroup("AdditionalInfoCell"),
                new DetectedIssuesAdditionalInfoCell(item)
            ),
            new If(
                _ => infrequentProperties.ContainsAnyOf(DetectedIssue.InfrequentProperties.Status,
                    DetectedIssue.InfrequentProperties.Severity),
                new TableCell([
                    new Concat([
                        new EnumIconTooltip("f:status", "http://hl7.org/fhir/observation-status",
                            new DisplayLabel(LabelCodes.Status)),
                        new EnumIconTooltip("f:severity",
                            "https://hl7.org/fhir/R4/valueset-detectedissue-severity.html",
                            new DisplayLabel(LabelCodes.Severity))
                    ])
                ])
            ),
            new If(_ => infrequentProperties.Contains(DetectedIssue.InfrequentProperties.Text),
                new NarrativeCell()
            )
        ];

        return new TableRow(tableRowContent, rowDetails, idSource: item).Render(item, renderer, context);
    }
}