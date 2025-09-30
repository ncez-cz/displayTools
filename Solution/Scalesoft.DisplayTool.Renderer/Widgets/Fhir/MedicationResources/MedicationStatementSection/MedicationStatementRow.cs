using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.MedicationResources.MedicationStatementSection;

public class MedicationStatementRow(
    XmlDocumentNavigator item,
    InfrequentPropertiesData<MedicationStatementInfrequentProperties> infrequentProperties
) : Widget
{
    public override async Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var rowDetails = new StructuredDetails();

        if (item.EvaluateCondition("f:context"))
        {
            rowDetails.AddCollapser(new ConstantText("Kontext"),
                ShowSingleReference.WithDefaultDisplayHandler(x => [new AnyResource(x, displayResourceType: false)], "f:context"));
        }

        if (item.EvaluateCondition("f:text"))
        {
            rowDetails.AddCollapser(new DisplayLabel(LabelCodes.OriginalNarrative), new Narrative("f:text"));
        }

        var dosageNav = navigator.SelectSingleNode("f:dosage");
        var row = new TableRow([
            new If(_ => infrequentProperties.Contains(MedicationStatementInfrequentProperties.Medication),
                new TableCell([
                    new Choose([
                        new When("f:medicationCodeableConcept",
                            new Optional("f:medicationCodeableConcept",
                                new TextContainer(TextStyle.Bold, [new CodeableConcept()]))
                        ),
                        new When("f:medicationReference",
                            ShowSingleReference.WithDefaultDisplayHandler(
                                x => [new Container([new ChangeContext(x, new Medication(false))], idSource: x)],
                                "f:medicationReference")
                        )
                    ])
                ])
            ),
            new If(_ => infrequentProperties.Contains(MedicationStatementInfrequentProperties.Dosage),
                new TableCell([
                    new Dosage(item, "f:dosage")
                ], idSource: dosageNav)
            ),
            new If(_ => infrequentProperties.Contains(MedicationStatementInfrequentProperties.Effective),
                new TableCell([new Chronometry("effective")])
            ),
            new If(_ => infrequentProperties.Contains(MedicationStatementInfrequentProperties.ReasonCode),
                new TableCell([new Optional("f:reasonCode", new CodeableConcept())])
            ),
            new If(_ => infrequentProperties.Contains(MedicationStatementInfrequentProperties.Status),
                new TableCell([
                    new Optional("f:status",
                        new EnumIconTooltip("@value", "http://hl7.org/fhir/CodeSystem/medication-statement-status",
                            new DisplayLabel(LabelCodes.Status)))
                ])
            ),
            new If(_ => infrequentProperties.Contains(MedicationStatementInfrequentProperties.Text),
                new NarrativeCell()
            )
        ], rowDetails, idSource: item);

        return await row.Render(item, renderer, context);
    }
}