using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils.Language;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Allergy;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Consent;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Encounter;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Immunization;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.MedicalDevice;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.PatientSection;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.PlanOfCare;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Pregnancy;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class CompositionIps : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        List<Widget> widgets =
        [
            new Container([
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
                new ShowSingleReference(x =>
                {
                    if (x.ResourceReferencePresent)
                    {
                        return [new Patient(x.Navigator)];
                    }

                    return [new ConstantText(x.ReferenceDisplay)];
                }, "f:subject"),

                new FhirSection(
                    LoincSectionCodes.Allergies,
                    (x, _) => new AllergiesAndIntolerances(x),
                    titleAbbreviations: SectionTitleAbbreviations.Allergies,
                    severity: Severity.Secondary
                ),
                new FhirSection(
                    LoincSectionCodes.Problems,
                    (x, _) => new Conditions(x, new DisplayLabel(LabelCodes.ActiveProblem)),
                    titleAbbreviations: SectionTitleAbbreviations.ActiveProblems
                ),
                new FhirSection(
                    LoincSectionCodes.HistoryOfMedicalDevices,
                    (x, _) => new DeviceUseStatement(x),
                    titleAbbreviations: SectionTitleAbbreviations.HistoryOfMedicalDeviceUse
                ),
                new FhirSection(
                    LoincSectionCodes.ProceduresHx,
                    (x, _) => new Procedures(x),
                    titleAbbreviations: SectionTitleAbbreviations.HistoryOfProcedures
                ),
                new FhirSection(
                    LoincSectionCodes.Medications,
                    (x, type) => new AnyResource(x, type),
                    titleAbbreviations: SectionTitleAbbreviations.HistoryOfMedicationUse
                ),
                new FhirSection(
                    LoincSectionCodes.FunctionalStatus,
                    (x, type) => new AnyResource(x, type),
                    titleAbbreviations: SectionTitleAbbreviations.FunctionalStatus
                ),
                new FhirSection(
                    LoincSectionCodes.VitalSigns,
                    (x, type) => new AnyResource(x, type, LoincSectionCodes.VitalSigns),
                    titleAbbreviations: SectionTitleAbbreviations.VitalSigns
                ),
                new ParentSection(
                    new ConstantText("Osobní anamnéza"),
                    titleAbbreviations: SectionTitleAbbreviations.PersonalHistory,
                    [
                        new FhirSection(
                            LoincSectionCodes.PastIllnessHx,
                            (x, _) => new Conditions(x, new DisplayLabel(LabelCodes.InactiveProblem)),
                            titleAbbreviations: SectionTitleAbbreviations.HistoryOfPastIllness,
                            severity: Severity.Secondary
                        ),
                        new FhirSection(
                            LoincSectionCodes.Immunizations,
                            (x, _) => new Immunizations(x),
                            titleAbbreviations: SectionTitleAbbreviations.Immunizations
                        ),
                    ]
                ),
                new FhirSection(
                    LoincSectionCodes.SocialHistory,
                    (x, type) => new AnyResource(x, type),
                    titleAbbreviations: SectionTitleAbbreviations.SocialHistory
                ),
                new ParentSection(
                    new ConstantText("Gynekologická anamnéza"),
                    titleAbbreviations: SectionTitleAbbreviations.GynecologicalHistory,
                    [
                        new FhirSection(
                            LoincSectionCodes.PregnancyHx,
                            (x, _) => new FhirIpsPregnancy(x),
                            titleAbbreviations: SectionTitleAbbreviations.HistoryOfPregnancies
                        ),
                    ]
                ),
                new ParentSection(
                    new ConstantText("Údaje poskytované pacientem"),
                    titleAbbreviations: SectionTitleAbbreviations.PatientProvidedData,
                    [
                        new FhirSection(
                            LoincSectionCodes.AdvanceDirectives,
                            (x, _) => new Consents(x),
                            titleAbbreviations: SectionTitleAbbreviations.AdvanceDirectives
                        ),
                    ]
                ),
                new FhirSection(
                    LoincSectionCodes.SignificantResults,
                    (x, type) => new AnyResource(x, type),
                    titleAbbreviations: SectionTitleAbbreviations.RelevantDiagnosticTests
                ),
                new FhirSection(
                    LoincSectionCodes.PlanOfCare,
                    (x, _) => new FhirCarePlan(x),
                    titleAbbreviations: SectionTitleAbbreviations.HealthMaintenanceCarePlan
                ),
                new FhirFooter(),
            ], idSource: navigator),
        ];
        return widgets.RenderConcatenatedResult(navigator, renderer, context);
    }
}