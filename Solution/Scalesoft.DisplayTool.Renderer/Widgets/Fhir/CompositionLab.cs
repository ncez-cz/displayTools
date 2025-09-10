using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Encounter;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir.PatientSection;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class CompositionLab : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        List<Widget> widgets =
        [
            new FhirHeader(),
            new ModifierExtensionCheck(),
            new Row([
                new Button(onClick: "expandOrCollapseAllSections();", variant: ButtonVariant.CollapseSection,
                    style: ButtonStyle.Outline),
                new Button(variant: ButtonVariant.ToggleDetails, style: ButtonStyle.Outline),
                new NarrativeModal(),
            ], flexContainerClasses: ""),
            new Optional("f:encounter",
                // multireference widget is used only for customising broken references builder, semantically the reference is x..1 
                new ShowMultiReference(".",
                    (items, _) => items.Select(Widget (x) => new EncounterCard(x)).ToList(),
                    x =>
                    [
                        new Collapser([new ConstantText(Labels.Encounter)], [], x.ToList(),
                            isCollapsed: true)
                    ]
                )
            ),
            new NarrativeCollapser(),

            ShowSingleReference.WithDefaultDisplayHandler(x => [new Patient(x)], "f:subject"),

            new ConcatBuilder("f:section", _ => [new FhirSection()]),

            new FhirFooter(),
        ];
        return widgets.RenderConcatenatedResult(navigator, renderer, context);
    }
}