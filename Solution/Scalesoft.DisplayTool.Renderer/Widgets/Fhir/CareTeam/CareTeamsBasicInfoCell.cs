using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.CareTeam;

public class CareTeamsBasicInfoCell(XmlDocumentNavigator item) : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var infrequentOptions =
            InfrequentProperties.Evaluate<InfrequentPropertiesPaths>([item]);

        var participantTableCell = new TableCell(
        [
            infrequentOptions.Contains(InfrequentPropertiesPaths.Category)
                ? new Concat([
                    new TextContainer(TextStyle.Bold, [new ConstantText("Kategorie")]),
                    new ConstantText(": "),
                    new Optional("f:category", new CommaSeparatedBuilder(".", _ => new CodeableConcept())),
                    new LineBreak()
                ], string.Empty)
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Name)
                ? new Concat([
                    new TextContainer(TextStyle.Bold, [new DisplayLabel(LabelCodes.Name)]),
                    new ConstantText(": "),
                    new Optional("f:name", new Text("@value")),
                    new LineBreak()
                ], string.Empty)
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Period)
                ? new Concat([
                    new TextContainer(TextStyle.Bold, [new DisplayLabel(LabelCodes.Duration)]),
                    new ConstantText(": "),
                    new ShowPeriod("f:period"),
                    new LineBreak()
                ], string.Empty)
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Telecom)
                ? new Concat([
                    new TextContainer(TextStyle.Bold, [new ConstantText("Kontakt")]),
                    new ConstantText(": "),
                    new ItemListBuilder(".", ItemListType.Unordered, _ => [new ShowContactPoint()]),
                    new LineBreak(),
                ], string.Empty)
                : new NullWidget()
        ]);

        if (infrequentOptions.Count == 0)
        {
            participantTableCell = new TableCell([
                new TextContainer(TextStyle.Muted, [new ConstantText("Informace nejsou k dispozici")])
            ]);
        }

        return participantTableCell.Render(item, renderer, context);
    }

    private enum InfrequentPropertiesPaths
    {
        Status, //0..1	code	proposed | active | suspended | inactive | entered-in-error
        Category, //0..*	CodeableConcept	Type of team
        Name, //	0..1	string	Name of the team, such as crisis assessment team
        Period, //	0..1	Period	Time period team covers
        Telecom, //	0..*	ContactPoint	A contact detail for the care team (that applies to all members)
    }
}