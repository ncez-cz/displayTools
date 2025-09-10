using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.FunctionalStatus.ClinicallImpression;

public class ClinicalImpressionBuilder(
    XmlDocumentNavigator item,
    InfrequentPropertiesData<ClinicalImpressionInfrequentProperties> infrequentProperties
) : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var existingProblems = ReferenceHandler.GetContentFromReferences(item, "f:problem");

        Widget widget = new ClinicalImpressionRow(item, infrequentProperties, existingProblems);

        return widget.Render(item, renderer, context);
    }
}