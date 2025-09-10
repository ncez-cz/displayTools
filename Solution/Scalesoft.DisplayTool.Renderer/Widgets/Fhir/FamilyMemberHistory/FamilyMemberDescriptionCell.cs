using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.FamilyMemberHistory;

public class FamilyMemberDescriptionCell(XmlDocumentNavigator item) : Widget
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
            infrequentOptions.Contains(InfrequentPropertiesPaths.Name)
                ? new Concat([
                    new Heading([
                        new Optional("f:name", new Text("@value")),
                        infrequentOptions.Contains(InfrequentPropertiesPaths.Relationship)
                            ? new Concat([
                                new ConstantText(" ("),
                                new Optional("f:relationship", new CodeableConcept()),
                                new ConstantText(")"),
                            ], string.Empty)
                            : new NullWidget(),
                    ], HeadingSize.H5),
                ])
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Relationship) &&
            !infrequentOptions.Contains(InfrequentPropertiesPaths.Name)
                ? new Concat([
                    new TextContainer(TextStyle.Bold, [new ConstantText("Vztah")]),
                    new ConstantText(": "),
                    new Optional("f:relationship", new CodeableConcept()),
                    new LineBreak()
                ], string.Empty)
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Born)
                ? new Concat([
                    new TextContainer(TextStyle.Bold, [new ConstantText("Datum narození")]),
                    new ConstantText(": "),
                    new Chronometry("born"),
                    new LineBreak()
                ])
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Age)
                ? new Concat([
                    new TextContainer(TextStyle.Bold, [new ConstantText("Věk")]),
                    new ConstantText(": "),
                    new OpenTypeElement(null, "age"), // Age | Range | string
                    new LineBreak()
                ], string.Empty)
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Deceased)
                ? new Concat([
                    new TextContainer(TextStyle.Bold, [new ConstantText("Zesnulý")]),
                    new ConstantText(": "),
                    new OpenTypeElement(null, "deceased"), // boolean | Age | Range | date | string
                    new LineBreak()
                ], string.Empty)
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Sex)
                ? new Concat([
                    new TextContainer(TextStyle.Bold, [new ConstantText("Pohlaví")]),
                    new ConstantText(": "),
                    new Optional("f:sex", new CodeableConcept()),
                    new LineBreak()
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
        Name, //0..1	string	The family member described
        Relationship, //1..1	CodeableConcept	Relationship to the subject.  http://terminology.hl7.org/CodeSystem/v3-RoleCode
        [OpenType("born")] Born, /*0..1		(approximate) date of birth - Chronometry*/
        [OpenType("age")] Age, /*0..1		(approximate) age - opentype*/
        [OpenType("deceased")] Deceased, /*0..1		Dead? How old/when? - opentype*/
        Sex, //0..1	CodeableConcept	male | female | other | unknown
    }
}