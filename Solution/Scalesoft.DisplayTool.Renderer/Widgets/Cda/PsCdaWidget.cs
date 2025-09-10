using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class PsCdaWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        List<Widget> widgetTree =
        [
            new Container([
                new PatientSectionWidget(),
            ], ContainerType.Div),
            new Container([
                new WidgetWithVariables(new AllergiesAndIntolerancesWidget(), [
                ]),
                new WidgetWithVariables(new CodedResultsWidget(), [
                ]),
                new WidgetWithVariables(new ActiveProblemsWidget(), [
                ]),
                new WidgetWithVariables(new MedicationSummaryWidget(), [
                ]),
                new WidgetWithVariables(new MedicalDevicesWidget(), [
                ]),
                new WidgetWithVariables(new SurgicalProceduresWidget(), [
                ]),
                new WidgetWithVariables(new HistoryOfPastIllnessesWidget(), [
                ]),
                new WidgetWithVariables(new ImmunizationsWidget(), [
                ]),
                new WidgetWithVariables(new HealthMaintenanceCarePlanWidget(), [
                ]),
                new WidgetWithVariables(new FunctionalStatusWidget(), [
                ]),
                new WidgetWithVariables(new SocialHistoryWidget(), [
                ]),
                new WidgetWithVariables(new PregnancyHistoryWidget(), [
                ]),
                new WidgetWithVariables(new VitalSignsWidget(), [
                ]),
                new WidgetWithVariables(new AdvanceDirectivesWidget(), [
                ]),
                new WidgetWithVariables(new OtherSectionsWidget(), [
                ]),
            ], ContainerType.Div),
        ];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}