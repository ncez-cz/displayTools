using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Alert.DetectedIssues;

public class DetectedIssuesActorsCell(XmlDocumentNavigator item) : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var infrequentOptions = InfrequentProperties.Evaluate<InfrequentPropertiesPaths>([item]);

        var tree = new TableCell([
            infrequentOptions.Contains(InfrequentPropertiesPaths.Author)
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new ConstantText("Autor")]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular, [new AnyReferenceNamingWidget("f:author")]),
                    new LineBreak()
                ])
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Patient)
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new ConstantText("Pacient")]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular, [new AnyReferenceNamingWidget("f:patient")]),
                    new LineBreak()
                ])
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Reference)
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new ConstantText("Odborný zdroj")]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular, [
                        new Link(
                            new ConstantText("odkaz"),
                            new Text("f:reference/@value")
                        )
                    ]),
                    new LineBreak()
                ])
                : new NullWidget()
        ]);
        
        
        if (infrequentOptions.Count == 0)
        {
            tree = new TableCell([ new TextContainer(TextStyle.Muted, [new ConstantText("Informace nejsou k dispozici")])]);
        }

        return tree.Render(item, renderer, context);
    }

    private enum InfrequentPropertiesPaths
    {
        Patient,
        Author,
        Reference,
    }
}