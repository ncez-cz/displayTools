using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Dasta;

public class DastaRootWidget : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        List<Widget> widgetTree =
        [
            new ChangeContext("/ds:dasta", new DastaHeader()),
            new Button(onClick: "expandOrCollapseAllSections();", variant: ButtonVariant.CollapseSection,
                style: ButtonStyle.Outline),
            new DastaPatientInfoWidget(),
            new DastaAllergyPatsumWidget(),
            new DastaDiagnosesPatsumWidget(),
            new DastaPhysicalFindingsPatsumWidget(),

            new DastaCodedMedicationPatsumWidget(),
            new DastaPlaintextMedicationPatsumWidget(),
            new DastaPlaintextMedicalHistoryPatsumWidget(),

            new DastaMedicalDevicesPatsumWidget(),
            new DastaVaccinationPatsumWidget(),
            new DastaSurgeriesPatsumWidget(),

            new DastaFunctionalStatusPatsumWidget(),

            new DastaWeightAndHeightPatsumWidget(),
            new DastaBloodGroupPatsumWidget(),

            new DastaPregnancyPatsumWidget(),
            new DastaRiskFactorsPatsumWidget(),

            new DastaHealthMaintenanceCarePatsumWidget(),
            //new DastaTetanusVaccinationPatsumWidget(), display only data from 'oc' blocks, according to Hynek Kružík
            //new DastaCodedMedicalHistoryPatsumWidget(),
            // new DastaSocialHistoryPatsumWidget(), not implemented in dasta
        ];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}