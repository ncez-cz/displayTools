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
                ? new NameValuePair([new ConstantText("Autor")], [new AnyReferenceNamingWidget("f:author")])
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Patient)
                ? new NameValuePair([new ConstantText("Pacient")], [new AnyReferenceNamingWidget("f:patient")])
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Reference)
                ? new NameValuePair([new ConstantText("Odborný zdroj")], [
                    new Link(
                        new ConstantText("odkaz"),
                        new Text("f:reference/@value")
                    )
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
        Patient,
        Author,
        Reference,
    }
}