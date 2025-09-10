using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class HospitalLogo : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var brandLogo = new Optional(
            "f:extension[@url='https://hl7.cz/fhir/core/StructureDefinition/cz-organization-logo']",
            ShowSingleReference.WithDefaultDisplayHandler(
                nav =>
                [
                    new Container(
                        new ChangeContext("f:content[1]/f:attachment",
                            new Binary()), // why does logo DocumentReference contain multiple content?
                        optionalClass: "header-image"),
                ],
                "f:valueReference")
        );

        return brandLogo.Render(navigator, renderer, context);
    }
}