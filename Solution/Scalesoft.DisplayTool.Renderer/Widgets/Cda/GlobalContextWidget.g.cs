using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class GlobalContextWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new ValueVariable("epsosLangDir", "'../EpsosRepository'"),
new ValueVariable("actionpath", "''"),
new ValueVariable("allowDispense", "'false'"),
new ValueVariable("shownarrative", "''"),
new Variable("documentCode", "/n1:ClinicalDocument/n1:code/@code"),
new Variable("title", [ 
new Choose([
new When("/n1:ClinicalDocument/n1:code/@displayName", [
new Text("/n1:ClinicalDocument/n1:code/@displayName")
, 
]),
new When("string-length(/n1:ClinicalDocument/n1:title)  >= 1", [
new Text("/n1:ClinicalDocument/n1:title")
, 
]),
], [
new ConstantText(@"Clinical Document"), 
]), 
]),
new Variable("originalNarrativeTableTitle", [ 
new WidgetWithVariables(new ShowEHdsiDisplayLabelWidget(), [
new Variable("code", "'108'"),
]), 
]),
new Variable("translatedCodedTableTitle", [ 
new WidgetWithVariables(new ShowEHdsiDisplayLabelWidget(), [
new Variable("code", "'109'"),
]), 
]),
new Variable("otherExist", "/n1:ClinicalDocument/n1:component/n1:structuredBody/n1:component/n1:section/n1:code[@code!='48765-2'][@code!='47420-5'][@code!='11450-4'][@code!='30954-2'][@code!='11348-0'][@code!='46264-8'][@code!='10160-0'][@code!='8716-3'][@code!='10162-6'][@code!='29762-2'][@code!='47519-4'][@code!='18776-5'][@code!='11369-6'][@code!='42348-3']"),
new Variable("advanceDirectivesSectionCode", "'42348-3'"),
new Variable("vitalSignsSectionCode", "'8716-3'"),
new Variable("pregnancyHistorySectionCode", "'10162-6'"),
new Variable("socialHistorySectionCode", "'29762-2'"),
new Variable("functionalStatusSectionCode", "'47420-5'"),
new Variable("healthMaintenanceCarePlanSectionCode", "'18776-5'"),
new Variable("immunizationsSectionCode", "'11369-6'"),
new Variable("historyOfPastIllnessesSectionCode", "'11348-0'"),
new Variable("surgicalProceduresSectionCode", "'47519-4'"),
new Variable("medicalDevicesSectionCode", "'46264-8'"),
new Variable("medicationSummarySectionCode", "'10160-0'"),
new Variable("currentProblemsSectionCode", "'11450-4'"),
new Variable("codedResultsSectionCode", "'30954-2'"),
new Variable("allergiesAndIntolerancesSectionCode", "'48765-2'"),
new Variable("patientRole", "/n1:ClinicalDocument/n1:recordTarget/n1:patientRole"),
new Variable("patientPreferLang", "$patientRole/n1:patient/n1:languageCommunication/n1:languageCode"),
new Variable("patientGuardian", "$patientRole/n1:patient/n1:guardian"),
new Variable("participantPRS", "/n1:ClinicalDocument/n1:participant/n1:functionCode[@code='PCP'][@codeSystem='2.16.840.1.113883.5.88']/../n1:associatedEntity[@classCode='PRS']"),
new Variable("AuthoringDeviceName", "/n1:ClinicalDocument/n1:author/n1:assignedAuthor/n1:assignedAuthoringDevice"),
new Variable("legalAuthenticatorAssignedPerson", "/n1:ClinicalDocument/n1:legalAuthenticator/n1:assignedEntity/n1:assignedPerson"),
new Variable("legalAuthenticatorRepresentedOrganization", "/n1:ClinicalDocument/n1:legalAuthenticator/n1:assignedEntity/n1:representedOrganization"),
new Variable("patientCustodian", "/n1:ClinicalDocument/n1:custodian/n1:assignedCustodian/n1:representedCustodianOrganization"),
new Variable("denominatorUnit", [ 
]),
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
