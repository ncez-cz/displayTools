using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class TranslateRoleAssoCodeWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Choose([
new When("$code='AFFL'", [
new ConstantText(@"affiliate"), 
]),
new When("$code='AGNT'", [
new ConstantText(@"agent"), 
]),
new When("$code='ASSIGNED'", [
new ConstantText(@"assigned entity"), 
]),
new When("$code='COMPAR'", [
new ConstantText(@"commissioning party"), 
]),
new When("$code='CON'", [
new ConstantText(@"contact"), 
]),
new When("$code='ECON'", [
new ConstantText(@"emergency contact"), 
]),
new When("$code='NOK'", [
new ConstantText(@"next of kin"), 
]),
new When("$code='SGNOFF'", [
new ConstantText(@"signing authority"), 
]),
new When("$code='GUARD'", [
new ConstantText(@"guardian"), 
]),
new When("$code='GUAR'", [
new ConstantText(@"guardian"), 
]),
new When("$code='CIT'", [
new ConstantText(@"citizen"), 
]),
new When("$code='COVPTY'", [
new ConstantText(@"covered party"), 
]),
], [
new ConstantText(@"{$code='"), 
new Text("$code")
, 
new ConstantText(@"'?}"), 
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
