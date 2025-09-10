using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Immunization;

public class ImmunizationsBuilder(
    XmlDocumentNavigator item,
    InfrequentPropertiesData<Immunizations.InfrequentPropertiesPaths> infrequentOptions,
    bool skipIdPopulation,
    InfrequentPropertiesData<Immunizations.InfrequentProtocolPropertiesPaths> infrequentProtocolOptions
) : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var protocolsApplied = item.SelectAllNodes("f:protocolApplied").ToList();

        // Show at least one row, even if no protocols exist.
        var protocolsCount = Math.Max(protocolsApplied.Count, 1);

        var widgets = new List<Widget>();
        for (var i = 1; i <= protocolsCount; i++)
        {
            widgets.Add(new ImmunizationRow(infrequentOptions, infrequentProtocolOptions, i, protocolsCount));
        }

        return new TableBody(widgets, idSource: skipIdPopulation ? null : new IdentifierSource(item)).Render(item, renderer, context);
    }
}