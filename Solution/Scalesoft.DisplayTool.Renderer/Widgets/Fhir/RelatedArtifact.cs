using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

/// <summary>
/// This is an element, not a resource.
/// </summary>
public class RelatedArtifact : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var card = new Card(null,
            new Concat([
                new NameValuePair(
                    new ConstantText("Typ"),
                    new EnumLabel("f:type", "http://hl7.org/fhir/ValueSet/related-artifact-type")
                ),
                new Optional("f:label", new NameValuePair(
                    new ConstantText("Název"),
                    new Text("@value")
                )),
                new Optional("f:display", new NameValuePair(
                    new ConstantText("Zobrazovaný text"),
                    new Text("@value")
                )),
                new Optional("f:citation", new NameValuePair(
                    new ConstantText("Citace"),
                    new Markdown("@value")
                )),
                new Optional("f:url", new NameValuePair(
                    new ConstantText("URL"),
                    new Link(new Text("@value"), new Text("@value"))
                )),
                new Optional("f:document", new NameValuePair(
                    new ConstantText("Dokument"),
                    new Attachment()
                )),
                new Optional("f:resource", new NameValuePair(
                    new ConstantText("Zdroj"),
                    new AnyReferenceNamingWidget()
                ))
            ])
        );

        return card.Render(navigator, renderer, context);
    }
}