using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

public class ThematicBreak(string? optionalClass = null) : Widget
{
    public override async Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        ViewModel viewModel = new()
        {
            CustomClass = optionalClass,
        };

        return await renderer.RenderThematicBreak(viewModel);
    }

    public class ViewModel : ViewModelBase;
}