using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Alert.DetectedIssues;

public class DetectedIssuesProblemDetailCell(XmlDocumentNavigator item) : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var infrequentOptions = InfrequentProperties.Evaluate<InfrequentPropertiesPaths>([item]);

        var tree = new TableCell([
            infrequentOptions.Contains(InfrequentPropertiesPaths.Code)
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new ConstantText("Kategorie")]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular, [new Optional("f:code", new CodeableConcept())]),
                    new LineBreak()
                ])
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Detail)
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new ConstantText("Detail")]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular, [new Optional("f:detail", new Text("@value"))]),
                    new LineBreak()
                ])
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Implicated)
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold,
                    [
                        new ConstantText("Související zdroje")
                    ]), //Indicates the resource representing the current activity or proposed activity that is potentially problematic.
                    new ConstantText(": "),
                    new LineBreak(),
                    new TextContainer(TextStyle.Regular,
                    [
                        new ConcatBuilder("f:implicated", _ => [new ConstantText("- "), new AnyReferenceNamingWidget()],
                            new LineBreak())
                    ]),
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
        Code,
        Detail,
        Implicated
    }
}