using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

public class Timeline(IEnumerable<Widget> cards, string? cssClass = null, bool vertical = true)
    : Widget
{
    public override async Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        var internalRenderResult = await RenderInternal(navigator, renderer, cards, context);

        var cardsRendered = cards
            .Select(card => internalRenderResult.GetContent(card))
            .OfType<string>()
            .ToList();

        var viewModel = new ViewModel
        {
            Items = cardsRendered,
            CssClass = cssClass,
            Vertical = vertical
        };
        
        var renderedTimeline = await renderer.RenderTimeline(viewModel);

        return new RenderResult(renderedTimeline, internalRenderResult.Errors);
    }

    public class ViewModel
    {
        public required List<string> Items { get; set; }
        public string? CssClass { get; set; }
        public bool Vertical { get; set; } = true;
    }
}
