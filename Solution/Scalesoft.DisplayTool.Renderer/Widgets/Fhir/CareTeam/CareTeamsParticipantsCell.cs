using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.CareTeam;

public class CareTeamsParticipantsCell(XmlDocumentNavigator item) : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        var participantTableCell = new TableCell(
        [
            new ItemListBuilder("f:participant", ItemListType.Unordered, (_, x) =>
            {
                var infrequentOptions = 
                    InfrequentProperties.Evaluate<InfrequentPropertiesPaths>([x]);
                
                return
                [
                    infrequentOptions.Contains(InfrequentPropertiesPaths.Member)
                        ? new TextContainer(TextStyle.Bold | TextStyle.Underlined,
                        [new Optional("f:member", new AnyReferenceNamingWidget(".")), new LineBreak()])
                        : new NullWidget(),
                    infrequentOptions.Contains(InfrequentPropertiesPaths.Role)
                        ? new Concat([
                            new TextContainer(TextStyle.Bold, [new ConstantText("Role")]),
                            new ConstantText(": "),
                            new Optional("f:role", new CodeableConcept()),
                            new LineBreak()
                        ], string.Empty): new NullWidget(),
                    infrequentOptions.Contains(InfrequentPropertiesPaths.Period)
                        ? new Concat([
                            new TextContainer(TextStyle.Bold, [new DisplayLabel(LabelCodes.Duration)]),
                            new ConstantText(": "),
                            new ShowPeriod("f:period"), new LineBreak()
                        ], string.Empty): new NullWidget(),
                    infrequentOptions.Contains(InfrequentPropertiesPaths.OnBehalfOf)
                        ? new Concat([
                            new TextContainer(TextStyle.Bold, [new ConstantText("Organizace")]),
                            new ConstantText(": "),
                            new Optional("f:onBehalfOf", new AnyReferenceNamingWidget("."))
                        ],string.Empty): new NullWidget()
                ];
            }),
        ]);

        if (!item.EvaluateCondition("f:participant"))
        {
            participantTableCell = new TableCell([
                new TextContainer(TextStyle.Muted, [new ConstantText("Informace nejsou k dispozici")])
            ]);
        }

        return participantTableCell.Render(item, renderer, context);
    }

    private enum InfrequentPropertiesPaths
    {
        /*Participant, //	0..*	BackboneElement	Members of the team*/
        Role, //0..*	CodeableConcept	Type of involvement
        Member, //0..1	Reference(Practitioner | PractitionerRole | RelatedPerson | Patient | Organization | CareTeam)	Who is involved
        OnBehalfOf, //	0..1	Reference(Organization)	Organization of the practitioner
        Period //	0..1	Period	Time period of participant
    }
}