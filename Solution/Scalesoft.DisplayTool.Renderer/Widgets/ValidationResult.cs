using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

public class ValidationResult(ValidationResultModel validationResult) : Widget
{
    public override async Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        var view = await renderer.RenderValidations(new ViewModel { ValidationResult = validationResult});
        return new RenderResult(view);
    }
    
    public class ViewModel    {
        public required ValidationResultModel ValidationResult { get; set; }
    }
}