using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Encounter;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Allergy;

public class AllergyIntoleranceRow(
    InfrequentPropertiesData<AllergiesAndIntolerancesInfrequentProperties> infrequentProperties,
    InfrequentPropertiesData<AllergiesAndIntolerancesReactionInfrequentProperties> infrequentReactionProperties,
    int reactionIndex = 1,
    int totalCount = 1
) : Widget
{
    public override async Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        Widget tree;

        navigator.SetVariable("reaction", $"f:reaction[{reactionIndex}]");


        var collapsibleRow = new StructuredDetails();

        if (navigator.EvaluateCondition("f:encounter"))
        {
            var encounterNarrative = ReferenceHandler.GetSingleNodeNavigatorFromReference(navigator,
                "f:encounter", "f:text");

            collapsibleRow.AddCollapser(new ConstantText(Labels.Encounter),
                ShowSingleReference.WithDefaultDisplayHandler(nav => [new EncounterCard(nav, false, false)],
                    "f:encounter"),
                encounterNarrative != null
                    ?
                    [
                        new NarrativeCollapser(encounterNarrative.GetFullPath())
                    ]
                    : null,
                encounterNarrative != null
                    ? new NarrativeModal(encounterNarrative.GetFullPath())
                    : null
            );
        }

        if (navigator.EvaluateCondition("f:text"))
        {
            collapsibleRow.AddCollapser(new DisplayLabel(LabelCodes.OriginalNarrative), new Narrative("f:text"));
        }

        if (reactionIndex == 1)
        {
            tree = new TableRow([
                new If(_ => infrequentProperties.Contains(AllergiesAndIntolerancesInfrequentProperties.Type),
                    new TableCell([
                        new Optional("f:type",
                            new EnumLabel("@value", "http://hl7.org/fhir/ValueSet/allergy-intolerance-type")),
                    ])
                ),
                new If(
                    _ => infrequentReactionProperties.Contains(AllergiesAndIntolerancesReactionInfrequentProperties
                        .Manifestation),
                    new TableCell([
                        new CommaSeparatedBuilder("$reaction/f:manifestation", _ => [new CodeableConcept()]),
                    ])
                ),
                new If(_ => infrequentProperties.Contains(AllergiesAndIntolerancesInfrequentProperties.Code),
                    new TableCell([new Optional("f:code", new CodeableConcept())])
                ),
                new If(_ => infrequentProperties.Contains(AllergiesAndIntolerancesInfrequentProperties.Onset),
                    new TableCell([new Chronometry("onset", true)])
                ),
                new If(
                    _ => infrequentProperties.ContainsAnyOf(AllergiesAndIntolerancesInfrequentProperties.Abatement,
                        AllergiesAndIntolerancesInfrequentProperties.AbatementDateTime),
                    new TableCell([
                        new Optional(
                            "f:extension[@url='http://hl7.org/fhir/StructureDefinition/allergyintolerance-abatement' or @url='http://hl7.org/fhir/uv/ips/StructureDefinition/abatement-dateTime-uv-ips']",
                            new Chronometry("value")),
                    ])
                ),
                new If(
                    _ => infrequentReactionProperties.ContainsAnyOf(AllergiesAndIntolerancesReactionInfrequentProperties
                        .Severity) || infrequentProperties.ContainsAnyOf(
                        AllergiesAndIntolerancesInfrequentProperties.Criticality,
                        AllergiesAndIntolerancesInfrequentProperties.ClinicalStatus,
                        AllergiesAndIntolerancesInfrequentProperties.VerificationStatus),
                    new TableCell(
                    [
                        new EnumIconTooltip("$reaction/f:severity", "http://hl7.org/fhir/reaction-event-severity",
                            new DisplayLabel(LabelCodes.Severity)),
                        new Optional("f:criticality",
                            new EnumIconTooltip("@value", "http://hl7.org/fhir/allergy-intolerance-criticality",
                                new ConstantText("Riziko"))),
                        new Optional("f:clinicalStatus",
                            new CodeableConceptIconTooltip(new DisplayLabel(LabelCodes.ClinicalStatus))),
                        new Optional("f:verificationStatus",
                            new CodeableConceptIconTooltip(new ConstantText("Stav ověření")))
                    ])
                ),
                new If(
                    _ => infrequentProperties.Contains(AllergiesAndIntolerancesInfrequentProperties.Text),
                    new NarrativeCell()
                ),
            ], reactionIndex == totalCount ? collapsibleRow : null);
        }
        else
        {
            tree = new TableRow([
                new If(_ => infrequentProperties.Contains(AllergiesAndIntolerancesInfrequentProperties.Type),
                    new TableCell([]) // type
                ),
                new If(
                    _ => infrequentReactionProperties.Contains(AllergiesAndIntolerancesReactionInfrequentProperties
                        .Manifestation),
                    new TableCell([
                        new CommaSeparatedBuilder("$reaction/f:manifestation", _ => [new CodeableConcept()]),
                    ])
                ),
                new If(_ => infrequentProperties.Contains(AllergiesAndIntolerancesInfrequentProperties.Code),
                    new TableCell([]) // code
                ),
                new If(_ => infrequentProperties.Contains(AllergiesAndIntolerancesInfrequentProperties.Onset),
                    new TableCell([]) // onset
                ),
                new If(
                    _ => infrequentProperties.ContainsAnyOf(AllergiesAndIntolerancesInfrequentProperties.Abatement,
                        AllergiesAndIntolerancesInfrequentProperties.AbatementDateTime),
                    new TableCell([]) // abatement
                ),
                new If(
                    _ => infrequentReactionProperties.Contains(AllergiesAndIntolerancesReactionInfrequentProperties
                        .Severity),
                    new TableCell([
                        new Optional("$reaction/f:severity",
                            new EnumIconTooltip(".", "http://hl7.org/fhir/reaction-event-severity",
                                new DisplayLabel(LabelCodes.Severity))
                        )
                    ])
                ),
                new If(
                    _ => infrequentProperties.Contains(AllergiesAndIntolerancesInfrequentProperties.Text),
                    new NarrativeCell(false)
                ),
            ], reactionIndex == totalCount ? collapsibleRow : null);
        }

        var result = await tree.Render(navigator, renderer, context);
        return result;
    }
}