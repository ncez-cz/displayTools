using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.PlanOfCare;

public class GoalsCard(XmlDocumentNavigator item) : Widget
{
    public override async Task<RenderResult> Render(
        XmlDocumentNavigator _,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var card = new Condition("f:goal",
            new Card(new ConstantText("Cíle"), new ShowMultiReference("f:goal", displayResourceType: false)));

        return await card.Render(item, renderer, context);
    }
}