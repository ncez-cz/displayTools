using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.PlanOfCare;

public class FhirCarePlan(List<XmlDocumentNavigator> items) : Widget
{
    public override async Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        var processingErrors = new List<ParseError>();

        // Process each top-level care plan item provided in the constructor
        var widgetsToRender = items.Select(carePlanNavigator =>
            new Container(
            [
                new CarePlanDetails(carePlanNavigator),
                new GoalsCard(carePlanNavigator),
                new AddressesCard(carePlanNavigator),
                new Activities(carePlanNavigator)
            ], idSource: carePlanNavigator)).Cast<Widget>().ToList();

        // Handle accumulated fatal errors before rendering
        if (processingErrors.Count != 0 && processingErrors.MaxSeverity() >= ErrorSeverity.Fatal)
        {
            return processingErrors;
        }

        // Render all collected widgets sequentially
        var finalRenderResult = await widgetsToRender.RenderConcatenatedResult(navigator, renderer, context);

        // Add non-fatal errors to the final rendered result
        finalRenderResult.Errors.AddRange(processingErrors);
        return finalRenderResult;
    }

    // Keep these utility methods as they're used by other widgets in the namespace
    public static Choose NarrativeAndOrChildren(IList<Widget> widgets)
    {
        return
            new Choose(
                [new When("f:text", new Tooltip(widgets, [new Narrative("f:text")]))],
                widgets.ToArray());
    }
}