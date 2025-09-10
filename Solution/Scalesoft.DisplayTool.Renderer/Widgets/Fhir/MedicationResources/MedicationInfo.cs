using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.MedicationResources;

public class MedicationInfo(XmlDocumentNavigator item) : Widget
{
    
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        navigator = item;
        
        List<Widget> tree =
        [
            new Optional("f:code", new CodeableConcept()),
            new ConstantText(" - "),
            new Ingredient()
        ];
        
        return tree.RenderConcatenatedResult(navigator, renderer, context);  
    }
}