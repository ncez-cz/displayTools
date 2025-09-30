using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils.Language;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Encounter;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.PatientSection;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class CompositionImagingOrder : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        List<Widget> widgets =
        [
            new FhirHeader(),
            new ModifierExtensionCheck(),
            new Row([
                new Button(onClick: "expandOrCollapseAllSections();", variant: ButtonVariant.CollapseSection,
                    style: ButtonStyle.Outline),
                new Button(variant: ButtonVariant.ToggleDetails, style: ButtonStyle.Outline),
                new NarrativeModal(),
            ], flexContainerClasses: ""),
            new Condition("f:extension[@url='http://hl7.eu/fhir/StructureDefinition/presentedForm']",
                new Collapser([new ConstantText("Jiné formy dokumentu")], [], [
                    new CommaSeparatedBuilder(
                        "f:extension[@url='http://hl7.eu/fhir/StructureDefinition/presentedForm']",
                        _ => [new OpenTypeElement(null)])
                ], customClass: "no-print")
            ),
            new Optional("f:encounter",
                // multireference widget is used only for customising broken references builder, semantically the reference is x..1 
                new ShowMultiReference(".",
                    (items, _) => items.Select(Widget (x) => new EncounterCard(x)).ToList(),
                    x =>
                    [
                        new Collapser([new ConstantText(Labels.Encounter)], [], x.ToList(),
                            isCollapsed: true)
                    ]
                )
            ),
            new NarrativeCollapser(),

            ShowSingleReference.WithDefaultDisplayHandler(x => [new Patient(x)], "f:subject"),

            new FhirSection(
                LoincSectionCodes.RequestedImagingStudies,
                (x, type) => new AnyResource(x, type),
                titleAbbreviations: SectionTitleAbbreviations.RequestedImagingStudies
            ),
            new FhirSection(
                LoincSectionCodes.RadiologyReason,
                (x, type) => new AnyResource(x, type),
                titleAbbreviations: SectionTitleAbbreviations.RadiologyReason
            ),
            new FhirSection(
                LoincSectionCodes.Coverage,
                (x, type) => new AnyResource(x, type),
                titleAbbreviations: SectionTitleAbbreviations.Coverage
            ),
            new FhirSection(
                LoincSectionCodes.Appointment,
                (x, type) => new AnyResource(x, type),
                titleAbbreviations: SectionTitleAbbreviations.Appointment
            ),
            new FhirSection(
                LoincSectionCodes.PlanOfCare,
                (x, type) => new AnyResource(x, type),
                titleAbbreviations: SectionTitleAbbreviations.HealthMaintenanceCarePlan
            ),
            new FhirSection(
                LoincSectionCodes.ImplantComponent,
                (x, type) => new AnyResource(x, type),
                titleAbbreviations: SectionTitleAbbreviations.ImplantComponent
            ),
            new FhirSection(
                LoincSectionCodes.AdditionalDocuments,
                (x, type) => new AnyResource(x, type),
                titleAbbreviations: SectionTitleAbbreviations.AdditionalDocuments
            ),
            new FhirFooter(),
        ];
        return widgets.RenderConcatenatedResult(navigator, renderer, context);
    }
}