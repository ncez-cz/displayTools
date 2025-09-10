using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.MedicationResources.MedicationAdministrationSection;

public class MedicationAdministration(List<XmlDocumentNavigator> items) : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var infrequentOptions =
            InfrequentProperties.Evaluate<InfrequentPropertiesPaths>(items);

        var dosageNavigators = items
            .Select(x => x
                .SelectAllNodes("f:dosage"))
            .SelectMany(x => x)
            .ToList();

        var dosageInfrequentOptions =
            InfrequentProperties.Evaluate<InrequentDosagePropertiesPaths>(
                dosageNavigators);

        var table = new Table(
            [
                new TableHead([
                    new TableRow([
                        new If(
                            _ => infrequentOptions.HasAnyOfGroup("MedicationCell"),
                            new TableCell([new ConstantText("Medikace")], TableCellType.Header)
                        ),
                        new If(_ => dosageInfrequentOptions.Count != 0,
                            new TableCell([new ConstantText("Informace o dávkování")], TableCellType.Header)
                        ),
                        new If(
                            _ => infrequentOptions.HasAnyOfGroup("ActorsCell"),
                            new TableCell([new ConstantText("Zainteresované strany")], TableCellType.Header)
                        ),
                        new If(
                            _ => infrequentOptions.HasAnyOfGroup("InfoCell"),
                            new TableCell([new ConstantText("Doplňujíci informace")], TableCellType.Header)
                        ),
                        new If(_ => infrequentOptions.Contains(InfrequentPropertiesPaths.Status),
                            new TableCell([new DisplayLabel(LabelCodes.Status)], TableCellType.Header)
                        ),
                        new If(_ => infrequentOptions.Contains(InfrequentPropertiesPaths.Text),
                            new NarrativeCell(false, TableCellType.Header)
                        )
                    ])
                ]),
                ..items.Select(x => new TableBody([
                    new MedicationAdministrationRowBuilder(x, infrequentOptions, dosageInfrequentOptions)
                ])),
            ],
            true
        );

        return table.Render(navigator, renderer, context);
    }

    public enum InfrequentPropertiesPaths
    {
        [Group("MedicationCell")] MedicationCodeableConcept,
        [Group("MedicationCell")] MedicationReference,
        [Group("MedicationCell")] Request,

        [Group("ActorsCell")] Subject,
        [Group("ActorsCell")] Performer,
        [Group("ActorsCell")] PartOf,
        [Group("ActorsCell")] Device,

        [Group("InfoCell")] StatusReason,
        [Group("InfoCell")] Identifier,
        [Group("InfoCell")] Category,
        [Group("InfoCell")] ReasonCode,
        [Group("InfoCell")] ReasonReference,
        [Group("InfoCell")] Note,
        [Group("InfoCell")] Id,
        [Group("InfoCell")] Text,

        [EnumValueSet("http://terminology.hl7.org/CodeSystem/medication-admin-status")]
        Status
    }

    public enum InrequentDosagePropertiesPaths
    {
        Route,
        Text,
        Site,
        Method,
        Dose,
        RateRatio,
        RateQuantity,
    }
}