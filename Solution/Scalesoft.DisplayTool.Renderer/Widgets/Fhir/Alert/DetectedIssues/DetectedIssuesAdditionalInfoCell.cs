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
                    ? new TextContainer(TextStyle.Regular, [
                        new TextContainer(TextStyle.Bold, [new ConstantText("Identifikátor podáni")]),
                        new ConstantText(": "),
                        new TextContainer(TextStyle.Regular,
                            [new CommaSeparatedBuilder("f:identifier", _ => [new ShowIdentifier()])]),
                        new LineBreak(),
                    ])
                    : infrequentOptions.Contains(InfrequentPropertiesPaths.Id)
                        ? new TextContainer(TextStyle.Regular, [
                            new TextContainer(TextStyle.Bold, [new ConstantText("Technický identifikátor podani")]),
                            new ConstantText(": "),
                            new TextContainer(TextStyle.Regular, [new Optional("f:id", new Text("@value"))]),
                            new LineBreak(),
                        ])
                        : new ConstantText("Identifikátor podání není specifikován")
            ),
            infrequentOptions.Contains(InfrequentPropertiesPaths.IdentifiedDateTime)
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new DisplayLabel(LabelCodes.Date)]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular, [new Optional("f:identifiedDateTime", new ShowDateTime())]),
                    new LineBreak()
                ])
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.IdentifiedPeriod)
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new DisplayLabel(LabelCodes.Date)]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular, [new Optional("f:identifiedPeriod", new ShowPeriod())]),
                    new LineBreak()
                ])
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Evidence)
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new ConstantText("Podpůrný důkaz")]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular,
                        [new CommaSeparatedBuilder("f:evidence", _ => [new CodeableConcept()])]),
                    new LineBreak()
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