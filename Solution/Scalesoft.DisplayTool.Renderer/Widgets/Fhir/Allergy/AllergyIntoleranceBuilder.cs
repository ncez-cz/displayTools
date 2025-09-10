using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Allergy;

public class AllergyIntoleranceBuilder(
    XmlDocumentNavigator item,
    InfrequentPropertiesData<AllergiesAndIntolerancesInfrequentProperties> infrequentProperties,
    InfrequentPropertiesData<AllergiesAndIntolerancesReactionInfrequentProperties> infrequentReactionProperties
) : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var reactions = item.SelectAllNodes("f:reaction").ToList();

        // Show at least one row, even if no reactions exist.
        var reactionCount = Math.Max(reactions.Count, 1);

        var widgets = new List<Widget>();
        for (var i = 1; i <= reactionCount; i++)
        {
            widgets.Add(new AllergyIntoleranceRow(infrequentProperties, infrequentReactionProperties, i, reactionCount));
        }

        return new TableBody(widgets, idSource: item).Render(item, renderer, context);
    }
}