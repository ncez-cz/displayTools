using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils.Language;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Allergy;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.CareTeam;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Consent;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Encounter;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.FamilyMemberHistory;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Immunization;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.PlanOfCare;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.PatientSection;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class CompositionHdr : Widget
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
                LoincSectionCodes.Allergies,
                (x, _) => new AllergiesAndIntolerances(x),
                titleAbbreviations: SectionTitleAbbreviations.Allergies
            ),
            new FhirSection(
                LoincSectionCodes.AdvanceDirectives,
                (x, _) => new Consents(x),
                titleAbbreviations: SectionTitleAbbreviations.AdvanceDirectives
            ),
            new FhirSection(
                LoincSectionCodes.Medications,
                (x, type) => new AnyResource(x, type),
                titleAbbreviations: SectionTitleAbbreviations.HistoryOfMedicationUse
            ),
            new FhirSection(
                LoincSectionCodes.Immunizations,
                (x, _) => new Immunizations(x),
                titleAbbreviations: SectionTitleAbbreviations.Immunizations
            ),
            new FhirSection(
                LoincSectionCodes.PastIllnessHx,
                (x, _) => new Conditions(x, new DisplayLabel(LabelCodes.InactiveProblem)),
                titleAbbreviations: SectionTitleAbbreviations.HistoryOfPastIllness
            ),
            new FhirSection(
                LoincSectionCodes.Problems,
                (x, _) => new Conditions(x, new DisplayLabel(LabelCodes.ActiveProblem)),
                titleAbbreviations: SectionTitleAbbreviations.ActiveProblems
            ),
            new FhirSection(
                LoincSectionCodes.FunctionalStatus,
                (x, type) => new AnyResource(x, type),
                titleAbbreviations: SectionTitleAbbreviations.FunctionalStatus
            ),
            new FhirSection(
                LoincSectionCodes.Alert,
                (x, type) => new AnyResource(x, type),
                titleAbbreviations: SectionTitleAbbreviations.HealthConcerns
            ),
            new FhirSection(
                LoincSectionCodes.PlanOfCare,
                (x, _) => new FhirCarePlan(x),
                titleAbbreviations: SectionTitleAbbreviations.HealthMaintenanceCarePlan
            ),
            new FhirSection(
                LoincSectionCodes.PhysicalFindings,
                (x, type) => new AnyResource(x, type),
                titleAbbreviations: SectionTitleAbbreviations.PhysicalFindings
            ),
            new FhirSection(
                LoincSectionCodes.SignificantResults,
                (x, type) => new AnyResource(x, type),
                titleAbbreviations: SectionTitleAbbreviations.RelevantDiagnosticTests
            ),
            new FhirSection(
                LoincSectionCodes.HistoryOfMedicalDevices,
                (x, type) => new AnyResource(x, type),
                titleAbbreviations: SectionTitleAbbreviations.HistoryOfMedicalDeviceUse
            ),
            new FhirSection(
                LoincSectionCodes.SignificantProcedures,
                (x, _) => new Procedures(x),
                titleAbbreviations: SectionTitleAbbreviations.HospitalDischargeProcedures
            ),
            new FhirSection(
                LoincSectionCodes.CareTeam,
                (x, _) => new CareTeams(x),
                titleAbbreviations: SectionTitleAbbreviations.PatientCareTeamInformation
            ),
            new FhirSection(
                LoincSectionCodes.AdmissionEvaluation,
                (x, type) => new AnyResource(x, type),
                titleAbbreviations: SectionTitleAbbreviations.AdmissionEvaluation
            ), // entry is Resource
            new FhirSection(
                LoincSectionCodes.DischargeDetails,
                (x, type) => new AnyResource(x, type),
                titleAbbreviations: SectionTitleAbbreviations.DischargeDetails
            ), // entry is Resource
            new FhirSection(
                LoincSectionCodes.FamilyHistory,
                (x, _) => new FamilyMembersHistory(x),
                titleAbbreviations: SectionTitleAbbreviations.HistoryOfFamilyMember
            ),
            new FhirSection(
                LoincSectionCodes.ProceduresHx,
                (x, _) => new Procedures(x),
                titleAbbreviations: SectionTitleAbbreviations.HistoryOfProcedures
            ),
            new FhirSection(
                LoincSectionCodes.DischargeMedications,
                (x, type) => new AnyResource(x, type),
                titleAbbreviations: SectionTitleAbbreviations.DischargeMedication
            ),
            new FhirSection(
                LoincSectionCodes.VitalSigns,
                (x, type) => new AnyResource(x, type, LoincSectionCodes.VitalSigns),
                titleAbbreviations: SectionTitleAbbreviations.VitalSigns
            ),
            new FhirSection(
                LoincSectionCodes.Synthesis,
                (x, type) => new AnyResource(x, type),
                titleAbbreviations: SectionTitleAbbreviations.Synthesis
            ),
            new FhirSection(
                LoincSectionCodes.SocialHistory,
                (x, type) => new AnyResource(x, type),
                titleAbbreviations: SectionTitleAbbreviations.SocialHistory
            ),
            new FhirSection(
                LoincSectionCodes.AdditionalDocuments,
                (x, type) => new AnyResource(x, type),
                titleAbbreviations: SectionTitleAbbreviations.AdditionalDocuments),
            new FhirSection(
                LoincSectionCodes.PaymentSources,
                (x, _) => new Coverages(x),
                titleAbbreviations: SectionTitleAbbreviations.PaymentSources
            ),
            new FhirSection(
                LoincSectionCodes.PhysicalExamByBodyAreas,
                (x, type) => new AnyResource(x, type),
                titleAbbreviations: SectionTitleAbbreviations.PhysicalExamination
            ),
            new FhirSection(
                LoincSectionCodes.HospitalCourse,
                (x, type) => new AnyResource(x, type),
                titleAbbreviations: SectionTitleAbbreviations.HospitalCourse
            ),
            new FhirSection(
                LoincSectionCodes.HospitalDischargeInstructions,
                (x, type) => new AnyResource(x, type),
                titleAbbreviations: SectionTitleAbbreviations.HospitalDischargeInstructions
            ),
            new FhirSection(
                LoincSectionCodes.HospitalDischargePhysicalFindings,
                (x, type) => new AnyResource(x, type),
                titleAbbreviations: SectionTitleAbbreviations.HospitalDischargePhysicalFindings
            ),
            new FhirSection(
                LoincSectionCodes.HistoryOfTravel,
                (x, type) => new AnyResource(x, type),
                titleAbbreviations: SectionTitleAbbreviations.HistoryOfTravel
            ),
            new FhirSection(
                LoincSectionCodes.HistoryGeneral,
                (x, type) => new AnyResource(x, type),
                titleAbbreviations: SectionTitleAbbreviations.GeneralHistory
            ),
            new FhirSection(
                LoincSectionCodes.Encounters,
                (x, type) => new AnyResource(x, type),
                titleAbbreviations: SectionTitleAbbreviations.Encounters
            ),
            new FhirFooter(),
        ];
        return widgets.RenderConcatenatedResult(navigator, renderer, context);
    }
}