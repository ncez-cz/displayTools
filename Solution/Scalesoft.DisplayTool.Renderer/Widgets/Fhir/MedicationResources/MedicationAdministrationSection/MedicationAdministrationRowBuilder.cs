using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.MedicationResources.MedicationAdministrationSection;

public class MedicationAdministrationRowBuilder(
    XmlDocumentNavigator item,
    InfrequentPropertiesData<MedicationAdministration.InfrequentPropertiesPaths> infrequentOptions,
    InfrequentPropertiesData<MedicationAdministration.InrequentDosagePropertiesPaths> dosageInfrequentOptions
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

        // context is ignored
        Widget tree = new TableRow([
            new If(
                _ => infrequentOptions.HasAnyOfGroup("MedicationCell"),
                new MedicationAdministrationMedicationCell(item)
            ),
            new If(_ => dosageInfrequentOptions.Count != 0,
                new MedicationAdministrationDosageCell(item)
            ),
            new If(
                _ => infrequentOptions.HasAnyOfGroup("ActorsCell"),
                new MedicationAdministrationActorsCell(item)
            ),
            new If(
                _ => infrequentOptions.HasAnyOfGroup("InfoCell"),
                new MedicationAdministrationAdditionalInfoCell(item)
            ),
            new If(_ => infrequentOptions.Contains(MedicationAdministration.InfrequentPropertiesPaths.Status),
                new TableCell([
                    new EnumIconTooltip("f:status", "http://terminology.hl7.org/CodeSystem/medication-admin-status",
                        new DisplayLabel(LabelCodes.Status))
                ])
            ),
            new If(_ => infrequentOptions.Contains(MedicationAdministration.InfrequentPropertiesPaths.Text),
                new NarrativeCell()
            )
        ], rowDetails, idSource: item);

        var result = await tree.Render(item, renderer, context);

        var isStatus = item.EvaluateCondition("f:status");
        if (!isStatus)
        {
            result.Errors.Add(ParseError.MissingValue(item.SelectSingleNode("f:status").GetFullPath()));
        }

        var isMedicationCodeable = item.EvaluateCondition("f:medicationCodeableConcept");
        var isMedicationReference = item.EvaluateCondition("f:medicationReference");
        if (!isMedicationCodeable || !isMedicationReference)
        {
            result.Errors.Add(ParseError.MissingValue(item.SelectSingleNode("f:*[starts-with(name(), 'medication')]")
                .GetFullPath()));
        }

        var isSubject = item.EvaluateCondition("f:subject");
        if (!isSubject)
        {
            result.Errors.Add(ParseError.MissingValue(item.SelectSingleNode("f:subject").GetFullPath()));
        }

        var isEffectiveDateTime = navigator.EvaluateCondition("f:effectiveDateTime");
        var isEffectivePeriod = navigator.EvaluateCondition("f:effectivePeriod");
        if (!isEffectiveDateTime && !isEffectivePeriod)
        {
            result.Errors.Add(ParseError.MissingValue(item.SelectSingleNode("f:*[starts-with(name(), 'effective')]")
                .GetFullPath()));
        }

        return result;
    }
}