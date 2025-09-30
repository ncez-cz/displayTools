using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Alert.DetectedIssues;

public class DetectedIssuesAdditionalInfoCell(XmlDocumentNavigator item) : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var infrequentOptions = InfrequentProperties.Evaluate<InfrequentPropertiesPaths>([item]);

        var tree = new TableCell([
            new HideableDetails(
                infrequentOptions.Contains(InfrequentPropertiesPaths.Identifier)
                    ? new NameValuePair([new ConstantText("Identifikátor podání")],
                        [new CommaSeparatedBuilder("f:identifier", _ => [new ShowIdentifier()])])
                    : infrequentOptions.Contains(InfrequentPropertiesPaths.Id)
                        ? new NameValuePair([new ConstantText("Technický identifikátor podání")],
                            [new Optional("f:id", new Text("@value"))])
                        : new ConstantText("Identifikátor podání není specifikován")
            ),
            infrequentOptions.Contains(InfrequentPropertiesPaths.IdentifiedDateTime)
                ? new NameValuePair([new DisplayLabel(LabelCodes.Date)],
                [
                    new CommaSeparatedBuilder("f:identifier",
                        _ => [new Optional("f:identifiedDateTime", new ShowDateTime())])
                ])
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.IdentifiedPeriod)
                ? new NameValuePair([new DisplayLabel(LabelCodes.Date)],
                [
                    new Optional("f:identifiedPeriod", new ShowPeriod())
                ])
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Evidence)
                ? new NameValuePair([new ConstantText("Podpůrný důkaz")],
                [
                    new CommaSeparatedBuilder("f:evidence", _ => [new CodeableConcept()])
                ])
                : new NullWidget()
        ]);

        if (infrequentOptions.Count == 0)
        {
            tree = new TableCell([
                new TextContainer(TextStyle.Muted, [new ConstantText("Informace nejsou k dispozici")])
            ]);
        }

        return tree.Render(item, renderer, context);
    }

    private enum InfrequentPropertiesPaths
    {
        Identifier,
        Id,
        IdentifiedDateTime,
        IdentifiedPeriod,
        Evidence
    }
}