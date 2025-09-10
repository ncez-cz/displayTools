using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class LocationCompact : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        var widget =
            new Choose(
                [new When("f:text", new Narrative("f:text"))],
                new Condition("f:name",
                    new Text("f:name/@value"),
                    new Condition("f:address",
                        new ConstantText(", "),
                        new LineBreak()
                    )
                ),
                new Optional("f:address", new Address()));

        return widget.Render(navigator, renderer, context);
    }
}