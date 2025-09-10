using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class NarrativeImage(List<KeyValuePair<string, string>> attributes) : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var src = attributes.FirstOrDefault(x => x.Key.ToLower() == "src").Value;
        if (src == null)
        {
            return Task.FromResult<RenderResult>(new ParseError
            {
                Kind = ErrorKind.InvalidValue,
                Message = "Image must have a src attribute",
                Path = navigator.GetFullPath(),
                Severity = ErrorSeverity.Warning,
            });
        }

        if (!src.StartsWith('#'))
        {
            return new Image(attributes).Render(navigator, renderer, context);
        }

        // Reference to an embedded resource
        var resource = navigator.SelectSingleNode($"ancestor::*/f:contained/*[f:id/@value = '{src[1..]}']");
        var height = attributes
            .FirstOrDefault(x => x.Key.Equals("height", StringComparison.CurrentCultureIgnoreCase)).Value;
        var width = attributes.FirstOrDefault(x => x.Key.Equals("width", StringComparison.CurrentCultureIgnoreCase))
            .Value;
        var alt = attributes.FirstOrDefault(x => x.Key.Equals("alt", StringComparison.CurrentCultureIgnoreCase))
            .Value;

        // Url or data src
        return resource.Node?.Name switch
        {
            "Binary" => new Container([new Binary(width, height, alt, onlyContentOrUrl: true)], idSource: resource)
                .Render(resource, renderer, context),
            "Media" => new Container([new NarrativeMedia(width, height, alt)], idSource: resource).Render(resource,
                renderer, context),
            _ => Task.FromResult<RenderResult>(new ParseError
            {
                Kind = ErrorKind.InvalidValue,
                Message = "Unsupported resource type",
                Path = resource.GetFullPath(),
                Severity = ErrorSeverity.Warning,
            })
        };
    }
}