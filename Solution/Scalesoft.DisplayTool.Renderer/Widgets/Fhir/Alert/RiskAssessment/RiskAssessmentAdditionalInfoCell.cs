using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Alert.RiskAssessment;

public class RiskAssessmentAdditionalInfoCell(XmlDocumentNavigator item) : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var infrequentOptions = InfrequentProperties.Evaluate<InfrequentPropertiesPaths>([item]);

        var actorsTableCell = new TableCell([
            new HideableDetails(
                infrequentOptions.Contains(InfrequentPropertiesPaths.Identifier)
                    ? new TextContainer(TextStyle.Regular, [
                        new TextContainer(TextStyle.Bold, [new ConstantText("Identifikátor podáni")]),
                        new ConstantText(": "),
                        new TextContainer(TextStyle.Regular,
                            [new CommaSeparatedBuilder("f:identifier", _ => [new ShowIdentifier()])]),
                        new LineBreak(),
                    ])
                    : infrequentOptions.Contains(InfrequentPropertiesPaths.Id)
                        ? new TextContainer(TextStyle.Regular, [
                            new TextContainer(TextStyle.Bold, [new ConstantText("Technický identifikátor podáni")]),
                            new ConstantText(": "),
                            new TextContainer(TextStyle.Regular, [new Optional("f:id", new Text("@value"))]),
                            new LineBreak(),
                        ])
                        : new ConstantText("Identifikátor podání není specifikován")
            ),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Occurrence)
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new ConstantText("Výskyt")]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular,
                        [new OpenTypeElement(null, "occurrence")]), // dateTime | Period
                    new LineBreak(),
                ])
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Basis)
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new ConstantText("Podklad")]),
                    new ConstantText(": "),
                    new LineBreak(),
                    new TextContainer(TextStyle.Regular,
                    [
                        new ItemListBuilder("f:basis", ItemListType.Unordered, _ => [new AnyReferenceNamingWidget()])
                    ]),
                    new LineBreak(),
                ])
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Mitigation)
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new ConstantText("Opatření")]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular, [new Optional("f:mitigation", new Text("@value"))]),
                    new LineBreak(),
                ])
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Note)
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new ConstantText("Komentář")]),
                    new ConstantText(": "),
                    new LineBreak(),
                    new TextContainer(TextStyle.Regular,
                        [new ItemListBuilder("f:note", ItemListType.Unordered, _ => [new ShowAnnotationCompact()])]),
                    new LineBreak(),
                ])
                : new NullWidget(),
        ]);

        if (infrequentOptions.Count == 0)
        {
            actorsTableCell = new TableCell([
                new TextContainer(TextStyle.Muted, [new ConstantText("Informace nejsou k dispozici")])
            ]);
        }

        return actorsTableCell.Render(item, renderer, context);
    }

    private enum InfrequentPropertiesPaths
    {
        Identifier,
        Id,
        [OpenType("occurrence")] Occurrence,
        Basis, //0..*	Reference(Any) - Information used in assessment
        Mitigation, //	0..1	string
        Note, //0..*	Annotation
    }
}