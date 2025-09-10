using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.FamilyMemberHistory;

public class FamilyMemberAdditionalInfoCell(XmlDocumentNavigator item) : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var infrequentOptions =
            InfrequentProperties.Evaluate<InfrequentPropertiesPaths>([item]);

        var familyMemberTableCell = new TableCell(
        [
            new HideableDetails(
                infrequentOptions.Contains(InfrequentPropertiesPaths.Identifier)
                    ? new Concat([
                        new TextContainer(TextStyle.Bold, [new ConstantText("Identifikátor týmu")]),
                        new ConstantText(": "),
                        new CommaSeparatedBuilder("f:identifier", _ => [new ShowIdentifier()]),
                        new LineBreak(),
                    ], string.Empty)
                    : infrequentOptions.Contains(InfrequentPropertiesPaths.Id)
                        ? new Concat([
                            new TextContainer(TextStyle.Bold, [new ConstantText("Technický identifikátor týmu")]),
                            new ConstantText(": "),
                            new Optional("f:id", new Text("@value")),
                            new LineBreak()
                        ], string.Empty)
                        : new ConstantText("Identifikátor podání není specifikován")
            ),
            infrequentOptions.Contains(InfrequentPropertiesPaths.ReasonCode)
                ? new Concat([
                    new TextContainer(TextStyle.Bold, [new ConstantText("Důvod provedení")]),
                    new ConstantText(": "),
                    new ItemListBuilder("f:reasonCode", ItemListType.Unordered, _ => [new CodeableConcept()]),
                ], string.Empty)
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.ReasonReference)
                ? new Concat([
                    new TextContainer(TextStyle.Bold, [new ConstantText("Související záznam")]),
                    new ConstantText(": "),
                    new ItemListBuilder("f:reasonReference", ItemListType.Unordered, _ =>
                    [
                        new Optional(".", new AnyReferenceNamingWidget()),
                    ])
                ], string.Empty)
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Date)
                ? new Concat([
                    new TextContainer(TextStyle.Bold, [new ConstantText("Datum změny")]),
                    new ConstantText(": "),
                    new ShowDateTime("f:date"),
                    new LineBreak(),
                ], string.Empty)
                : new NullWidget(),
        ]);
        if (infrequentOptions.Count == 0)
        {
            familyMemberTableCell = new TableCell([
                new TextContainer(TextStyle.Muted, [new ConstantText("Informace nejsou k dispozici")])
            ]);
        }

        return familyMemberTableCell.Render(item, renderer, context);
    }

    private enum InfrequentPropertiesPaths
    {
        /*Participant, //	0..*	BackboneElement	Members of the team*/
        Id,
        Identifier, //	0..*	Identifier	External Id(s) for this record
        Date, //0..1	dateTime	When history was recorded or last updated
        ReasonCode, //	0..*	CodeableConcept	Why was family member history performed?
        ReasonReference, //0..*	Reference(Condition | Observation | AllergyIntolerance | QuestionnaireResponse | DiagnosticReport | DocumentReference) Why was family member history performed?
    }
}