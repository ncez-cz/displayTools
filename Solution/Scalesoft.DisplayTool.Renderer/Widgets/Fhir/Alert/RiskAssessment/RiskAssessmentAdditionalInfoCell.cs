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
                    ? new NameValuePair([new ConstantText("Identifikátor podáni")],
                    [
                        new CommaSeparatedBuilder("f:identifier", _ => [new ShowIdentifier()])
                    ])
                    : infrequentOptions.Contains(InfrequentPropertiesPaths.Id)
                        ? new NameValuePair([new ConstantText("Technický identifikátor podáni")],
                        [
                            new Optional("f:id", new Text("@value"))
                        ])
                        : new ConstantText("Identifikátor podání není specifikován")
            ),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Occurrence)
                ? new NameValuePair([new ConstantText("Výskyt")],
                [
                    new OpenTypeElement(null, "occurrence") // dateTime | Period
                ])
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Basis)
                ? new NameValuePair([new ConstantText("Podklad")],
                [
                    new ItemListBuilder("f:basis", ItemListType.Unordered, _ => [new AnyReferenceNamingWidget()])
                ])
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Mitigation)
                ? new NameValuePair([new ConstantText("Opatření")],
                [
                    new Optional("f:mitigation", new Text("@value"))
                ])
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Note)
                ? new NameValuePair([new ConstantText("Komentář")],
                [
                    new ItemListBuilder("f:note", ItemListType.Unordered, _ => [new ShowAnnotationCompact()])
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