using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Alert;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Alert.DetectedIssues;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Alert.RiskAssessment;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Allergy;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.CareTeam;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Consent;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.DocumentReference;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Encounter;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.FunctionalStatus.ClinicallImpression;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Immunization;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.MedicalDevice;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.MedicationResources;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.MedicationResources.MedicationAdministrationSection;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.MedicationResources.MedicationDispenseSection;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.MedicationResources.MedicationRequestSection;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Observation;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.PlanOfCare;
using Scalesoft.DisplayTool.Renderer.Widgets.Questionnaire;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class AnyResource(
    List<XmlDocumentNavigator> items,
    string? resourceType,
    string? sectionCode = null,
    bool displayResourceType = true
) : Widget
{
    public AnyResource(
        XmlDocumentNavigator item,
        string? resourceType = null,
        string? sectionCode = null,
        bool displayResourceType = true
    ) : this(
        [item], resourceType ?? item.Node?.Name, sectionCode, displayResourceType)
    {
    }

    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        List<Widget> tree;
        switch (resourceType)
        {
            case "AllergyIntolerance":
                tree =
                [
                    new AllergiesAndIntolerances(items),
                ];
                break;
            case "Appointment":
                tree =
                [
                    ..OneOrManyColumns(items, _ => new Appointment())
                ];
                break;
            case "Attachment":
                tree =
                [
                    ..items.Select(x => new ChangeContext(x,
                        new Container([
                            new ItemListBuilder(".", ItemListType.Unordered, _ => [new Attachment()]),
                        ]))),
                ];
                break;
            case "Binary":
                tree =
                [
                    ..items.Select(x => new ChangeContext(x,
                        new Container([new ItemListBuilder(".", ItemListType.Unordered, _ => [new Binary()]),],
                            idSource: x))
                    ),
                ];
                break;
            case "BodyStructure":
                tree =
                [
                    ..items.Select(x => new ChangeContext(x, new Container([new BodyStructure()], idSource: x))),
                ];
                break;
            case "CarePlan":
                tree =
                [
                    new FhirCarePlan(items),
                ];
                break;
            case "CareTeam":
                tree =
                [
                    new CareTeams(items),
                ];
                break;
            case "CommunicationRequest":
                tree =
                [
                    ..OneOrManyColumns(items, _ => new CommunicationRequest())
                ];
                break;
            case "Condition":
                tree =
                [
                    new Conditions(items, new ConstantText("Problém")), // active/inactive problem
                ];
                break;
            case "Consent":
                tree =
                [
                    new Consents(items),
                ];
                break;
            case "Contract":
                tree =
                [
                    ..OneOrManyColumns(items, x => new Contract(x))
                ];
                break;
            case "Coverage":
                tree =
                [
                    new Coverages(items),
                ];
                break;
            case "DetectedIssue":
                tree =
                [
                    new DetectedIssue(items),
                ];
                break;
            case "Device":
                tree =
                [
//                    ..OneOrManyRows(items, _ => new Card(null, new DeviceTextInfo())),
                    new ShowDevice(items),
                ];
                break;
            case "DeviceRequest":
                tree =
                [
                    ..OneOrManyColumns(items, _ => new DeviceRequest())
                ];
                break;
            case "DeviceUseStatement":
                tree =
                [
                    new DeviceUseStatement(items),
                ];
                break;
            case "DocumentReference":
                tree =
                [
                    new DocumentReferences(items)
                ];
                break;
            case "DiagnosticReport":
                tree =
                [
                    ..OneOrManyColumns(items, _ => new DiagnosticReportCard())
                ];
                break;
            case "Encounter":
                tree =
                [
                    ..OneOrManyColumns(items, x => new EncounterCard(x))
                ];
                break;
            case "EpisodeOfCare":
                tree =
                [
                    ..items.Select(x => new ChangeContext(x, new Container([new EpisodeOfCare()], idSource: x))),
                ];
                break;
            case "Evidence":
                tree =
                [
                    ..OneOrManyColumns(items, x => new Evidence(x))
                ];
                break;
            case "EvidenceVariable":
                tree =
                [
                    ..OneOrManyColumns(items, x => new EvidenceVariable(x))
                ];
                break;
            // FamilyMemberHistory
            case "Flag":
                tree =
                [
                    new FlagResource(items),
                ];
                break;
            case "Goal":
                tree =
                [
                    new Goals(items),
                ];
                break;
            case "HealthcareService":
                tree =
                [
                    ..OneOrManyColumns(items, x => new HealthcareService(x))
                ];
                break;
            case "ImagingStudy":
                tree =
                [
                    ..items.Select(x => new ChangeContext(x, new Container([new ImagingStudy()], idSource: x))),
                ];
                break;
            case "Immunization":
                tree =
                [
                    new Immunizations(items),
                ];
                break;
            case "ImmunizationRecommendation":
                tree =
                [
                    ..items.Select(x =>
                        new ChangeContext(x, new Container([new ImmunizationRecommendation()], idSource: x))),
                ];
                break;
            case "ImmunizationEvaluation":
                tree =
                [
                    ..items.Select(x =>
                        new ChangeContext(x, new Container([new ImmunizationEvaluation()], idSource: x))),
                ];
                break;
            case "Media":
                tree =
                [
                    ..OneOrManyColumns(items, _ => new Media())
                ];
                break;
            case "Medication":
                tree =
                [
                    ..items.Select(x => new ChangeContext(x, new Container([new Medication()], idSource: x))),
                ];
                break;
            case "MedicationAdministration":
                tree =
                [
                    new AlternatingBackgroundColumn(
                        items.Select(Widget (x) =>
                                new ChangeContext(x,
                                    new Container([new MedicationAdministrationCard()], idSource: x)))
                            .ToList()
                    ),
                ];
                break;
            case "MedicationDispense":
                tree =
                [
                    new MedicationDispense(items)
                ];
                break;
            case "MedicationRequest":
                tree =
                [
                    ..OneOrManyColumns(items, _ => new MedicationRequest()),
                ];
                break;
            case "MedicationStatement":
                tree =
                [
                    new AlternatingBackgroundColumn(
                        items.Select(Widget (x) =>
                                new ChangeContext(x,
                                    new Container([new MedicationStatementCard()], idSource: x)))
                            .ToList()
                    ),
                ];
                break;
            case "NutritionOrder":
                tree =
                [
                    ..items.Select(x => new ChangeContext(x, new Container([new NutritionOrder()], idSource: x))),
                ];
                break;
            case "Observation":
            {
                var labObs = new List<XmlDocumentNavigator>();
                var otherObs = new List<XmlDocumentNavigator>();
                foreach (var item in items)
                {
                    if (item.EvaluateCondition(CzLaboratoryObservation.XPathCondition))
                    {
                        labObs.Add(item);
                    }
                    else
                    {
                        otherObs.Add(item);
                    }
                }

                tree =
                [
                    new Column([
                        new If(_ => labObs.Count != 0, new CzLaboratoryObservation(labObs)),
                        ..otherObs.Select(x => new ChangeContext(x,
                            new ObservationCard(hideObservationType: sectionCode == LoincSectionCodes.VitalSigns)))
                    ], flexContainerClasses: "gap-0")
                ];
            }
                break;
            case "Organization":
                tree =
                [
                    // Title is rendered as collapserTitle in PersonOrOrganization
                    ..items.Select(item =>
                        new Container(
                        [
                            new PersonOrOrganization(item,
                                collapserTitle: new Optional("f:name/@value", new Text()),
                                collapserSubtitle: displayResourceType ? [new ConstantText("Organizace")] : null)
                        ], idSource: item)),
                ];
                break;
            case "List":
                tree =
                [
                    ..OneOrManyColumns(items, _ => new List())
                ];
                break;
            case "Location":
                tree =
                [
                    ..items.Select(item => new Container([new Location(item)], idSource: item)),
                ];
                break;
            case "Patient":
                tree =
                [
                    // Title is rendered as collapserTitle in PersonOrOrganization
                    ..items.Select(item =>
                        new Container(
                        [
                            new PersonOrOrganization(item,
                                collapserTitle: new Optional("f:name", new HumanNameCompact(".")),
                                collapserSubtitle: displayResourceType ? [new ConstantText("Pacient")] : null)
                        ], idSource: item)),
                ];
                break;
            case "Practitioner":
                tree =
                [
                    // Title is rendered as collapserTitle in PersonOrOrganization
                    ..items.Select(item =>
                        new Container(
                        [
                            new PersonOrOrganization(item,
                                collapserTitle: new Optional("f:name", new HumanNameCompact(".")),
                                collapserSubtitle: displayResourceType ? [new ConstantText("Lékař")] : null)
                        ], idSource: item)),
                ];
                break;
            case "PractitionerRole":
                tree =
                [
                    // Title is rendered as collapserTitle in PersonOrOrganization
                    ..items.Select(nav =>
                    {
                        var resourceSummary = ResourceSummaryProvider.GetSummary(nav);
                        return new Container(
                            [
                                new ChangeContext(nav,
                                    new PersonOrOrganization(nav, showCollapser: true,
                                        collapserTitle: resourceSummary?.Widget)),
                            ],
                            idSource: nav);
                    })
                ];
                break;
            case "Procedure":
                tree =
                [
                    new Procedures(items),
                ];
                break;
            case "Provenance":
                tree =
                [
                    new Provenances(items)
                ];
                break;
            case "QuestionnaireResponse":
                tree =
                [
                    new QuestionnaireResponses(items),
                ];
                break;
            case "RelatedPerson":
                tree =
                [
                    // Title is rendered as collapserTitle in PersonOrOrganization
                    ..items.Select(item =>
                        new Container(
                        [
                            new PersonOrOrganization(item,
                                collapserTitle: new Optional("f:name", new HumanNameCompact(".")),
                                collapserSubtitle: displayResourceType
                                    ? [new ConstantText("Související osoba")]
                                    : null),
                        ], idSource: item)),
                ];
                break;
            case "RequestGroup":
                tree =
                [
                    ..items.Select(x => new ChangeContext(x, new Container([new RequestGroup()], idSource: x))),
                ];
                break;
            case "RiskAssessment":
                tree =
                [
                    new RiskAssessments(items),
                ];
                break;
            case "ServiceRequest":
                tree =
                [
                    ..OneOrManyColumns(items, _ => new ServiceRequest())
                ];
                break;
            case "Substance":
                tree =
                [
                    ..items.Select(x => new Container([new Substance(x)], idSource: x)),
                ];
                break;
            case "Specimen":
                tree =
                [
                    ..OneOrManyColumns(items, x => new Specimen(x))
                ];
                break;
            case "Task":
                tree =
                [
                    ..OneOrManyColumns(items, _ => new TaskResource())
                ];
                break;
            case "VisionPrescription":
                tree =
                [
                    // A specific title is rendered inside the VisionPrescription widget
                    ..OneOrManyColumns(items, _ => new VisionPrescriptionCard())
                ];
                break;
            case "ClinicalImpression":
                tree =
                [
                    new ClinicalImpression(items),
                ];
                break;
            case "Group":
                tree =
                [
                    ..OneOrManyColumns(items, _ => new Group(), "patient-cards-layout")
                ];
                break;
            default:
                tree =
                [
                    ..items.Select(x =>
                        new Row(
                        [
                            new ChangeContext(x, new ConstantText($"{resourceType}/"), new Text("f:id/@value"),
                                new NarrativeModal()),
                        ], flexContainerClasses: "align-items-center justify-content-between", idSource: x)),
                ];
                break;
        }

        var widget = new If(_ => displayResourceType,
            new Section(
                ".",
                null,
                title: [new LocalNodeName(resourceType, items.Count > 1)],
                content: tree
            )
        ).Else(new Concat(tree));

        return widget.Render(navigator, renderer, context);
    }

    private static IEnumerable<Widget> OneOrManyColumns(
        List<XmlDocumentNavigator> inputItems,
        Func<XmlDocumentNavigator, Widget> builder,
        string? optionalClass = null
    )
    {
        // more than one → wrap in a Row
        if (inputItems.Count != 1)
        {
            return
            [
                new Column([..inputItems.Select(x => new ChangeContext(x, new Container([builder(x)], idSource: x)))],
                    childContainerClasses: optionalClass, flexContainerClasses: "gap-0")
            ];
        }

        var item = inputItems.FirstOrDefault();
        if (item == null)
        {
            return [new NullWidget()];
        }

        // just return the widget directly
        return
        [
            new ChangeContext(item,
                new Container([builder(item)], idSource: item, optionalClass: optionalClass))
        ];
    }
}