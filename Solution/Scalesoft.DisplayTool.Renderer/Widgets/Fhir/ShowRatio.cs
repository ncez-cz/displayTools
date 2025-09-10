using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class ShowRatio(string path = ".") : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var tree = new ChangeContext(path, new Optional("f:numerator", new ShowQuantity()),
            new Choose([new When("f:numerator and f:denominator", new ConstantText("/"))]),
            new Optional("f:denominator", new ConstantText(" "), new ShowQuantity()), new UncertaintyExtensions());

        return tree.Render(navigator, renderer, context);
    }
}