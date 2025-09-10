using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class NarrativeMedia(string? width = null, string? height = null, string? altText = null) : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        return new ChangeContext("f:content",
            new Container([new Attachment(width, height, altText, onlyContentOrUrl: true)],
                idSource: new IdentifierSource())).Render(navigator, renderer, context);
    }
}