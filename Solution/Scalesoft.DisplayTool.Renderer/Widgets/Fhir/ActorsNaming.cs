using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class ActorsNaming : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        Task<RenderResult> result;

        switch (navigator.Node?.Name)
        {
            case "Device":
                result = DeviceParsingInfo.CompactRenderingWidgets.RenderConcatenatedResult(navigator, renderer, context);
                break;
            case "Practitioner":
            case "Patient":
            case "RelatedPerson":
                result = new HumanNameCompact("f:name").Render(navigator, renderer, context);
                break;
            case "Organization":
            case "HealthcareService":
            case "CareTeam":
            case "Location":
                result = new Text("f:name/@value").Render(navigator, renderer, context);
                break;
            case "PractitionerRole":
                result =
                    new Optional("f:practitioner",
                        ShowSingleReference.WithDefaultDisplayHandler(x => [new Container([new HumanNameCompact("f:name")], ContainerType.Span, idSource: x)], ".")
                    ).Render(navigator, renderer, context);
                break;
            default:
                return Task.FromResult<RenderResult>(new ParseError
                {
                    Kind = ErrorKind.InvalidValue, Message = "Unknown observation performer type",
                    Path = navigator.GetFullPath(),
                    Severity = ErrorSeverity.Warning
                });
        }

        return result;
    }
}