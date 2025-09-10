using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.MedicationResources.MedicationStatementSection;

public class MedicationStatement(List<XmlDocumentNavigator> items) : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var infrequentProperties =
            InfrequentProperties.Evaluate<MedicationStatementInfrequentProperties>(items);

        var table = new Table(
            [
                new TableHead([
                    new TableRow([
                        new If(_ => infrequentProperties.Contains(MedicationStatementInfrequentProperties.Medication),
                            new TableCell([new DisplayLabel(LabelCodes.MedicalProduct)], TableCellType.Header)
                        ),
                        new If(_ => infrequentProperties.Contains(MedicationStatementInfrequentProperties.Dosage),
                            new TableCell([new ConstantText("Dávkování")], TableCellType.Header)
                        ),
                        new If(_ => infrequentProperties.Contains(MedicationStatementInfrequentProperties.Effective),
                            new TableCell([new DisplayLabel(LabelCodes.TreatmentDuration)], TableCellType.Header)
                        ),
                        new If(_ => infrequentProperties.Contains(MedicationStatementInfrequentProperties.ReasonCode),
                            new TableCell([new DisplayLabel(LabelCodes.MedicationReason)], TableCellType.Header)
                        ),
                        new If(_ => infrequentProperties.Contains(MedicationStatementInfrequentProperties.Status),
                            new TableCell([new ConstantText("Další")], TableCellType.Header)
                        ),
                        new If(_ => infrequentProperties.Contains(MedicationStatementInfrequentProperties.Text),
                            new NarrativeCell(false, TableCellType.Header)
                        )
                    ])
                ]),
                ..items.Select(x => new TableBody([new MedicationStatementRow(x, infrequentProperties)])),
            ],
            true
        );

        return table.Render(navigator, renderer, context);
    }
}

public enum MedicationStatementInfrequentProperties
{
    [OpenType("medication")] Medication,
    Dosage,
    [OpenType("effective")] Effective,
    ReasonCode,

    [EnumValueSet("http://hl7.org/fhir/CodeSystem/medication-statement-status")]
    Status,
    Text
}