using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class HumanNameCompact(string namePath) : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var widget = new Choose([
                new When($"{namePath}/f:text", new Text($"{namePath}/f:text/@value")),
            ], new ConcatBuilder($"{namePath}/f:prefix/@value", _ => [new Text()], " "),
            new Condition($"{namePath}/f:prefix", new ConstantText(" ")),
            new ConcatBuilder($"{namePath}/f:given/@value", _ => [new Text()], " "), new ConstantText(" "),
            new ConcatBuilder($"{namePath}/f:family/@value", _ => [new Text()], " "),
            new Condition($"{namePath}/f:suffix", new ConstantText(" ")),
            new ConcatBuilder($"{namePath}/f:suffix/@value", _ => [new Text()], " ")
        );

        return widget.Render(navigator, renderer, context);
    }
}