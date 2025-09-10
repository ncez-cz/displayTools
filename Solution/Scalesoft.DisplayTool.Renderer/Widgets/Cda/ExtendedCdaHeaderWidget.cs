using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class ExtendedCdaHeaderWidget : Widget
{
    public override async Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context1)
    {
        List<Widget>  flexListContent = [];
        if (navigator.EvaluateCondition("$participantPRS"))
        {
            flexListContent.Add(new Container([
                new WidgetWithVariables(new DisplayPreferredHcpAndLegalOrganizationWidget(), [
                ]),
            ], ContainerType.Div));
        }
        
        if (navigator.EvaluateCondition("/n1:ClinicalDocument/n1:author"))
        {
            flexListContent.Add(  new Container([
                new WidgetWithVariables(new DisplayAuthorsWidget(), [
                ]),
            ], ContainerType.Div));
        }

        var legalAuthenticatorContainer = new WidgetWithVariables(new DisplayLegalAuthenticatorWidget(), []);
        var renderResult = await legalAuthenticatorContainer.Render(navigator, renderer, context1);
        if (!string.IsNullOrEmpty(renderResult.Content))
        {
            flexListContent.Add(  new Container([
                new WidgetWithVariables(new DisplayLegalAuthenticatorWidget(), [
                ]),
            ], ContainerType.Div));
        }

        var context = navigator.SelectAllNodes(
            "/n1:ClinicalDocument/n1:participant/n1:templateId[@root='1.3.6.1.4.1.19376.1.5.3.1.2.4']/../n1:associatedEntity");
        var conditionResult = context.Any(x => x.EvaluateCondition("not(../n1:functionCode) or not(../n1:functionCode/@code='PCP')") &&
                                               x.EvaluateCondition("n1:associatedPerson/n1:name/* or n1:scopingOrganization"));
        if (conditionResult)
        {
            flexListContent.Add(  new Container([
                new WidgetWithVariables(new DisplayOtherContactsWidget(), [
                ]),
            ], ContainerType.Div));
        }
        if (navigator.EvaluateCondition("$patientGuardian"))
        {
            flexListContent.Add(     new Container([
                new WidgetWithVariables(new DisplayGuardianWidget(), [
                ]),
            ], ContainerType.Div));
        }
        if (navigator.EvaluateCondition("$patientCustodian"))
        {
            flexListContent.Add(     new Container([
                new WidgetWithVariables(new DisplayCustodianWidget(), [
                ]),
            ], ContainerType.Div));
        }
        List<Widget> widgetTree =
        [
            new FlexList([
                ..flexListContent
            ], FlexDirection.Row, null, "patient-cards-layout"),
        ];
        return await widgetTree.RenderConcatenatedResult(navigator, renderer, context1);
    }
}