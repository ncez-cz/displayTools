using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class ShowParticipationFunctionWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Choose([
new When(" $pFunction = 'ADMPHYS' ", [
new ConstantText(@"admitting physician"), 
]),
new When(" $pFunction = 'ANEST' ", [
new ConstantText(@"anesthesist"), 
]),
new When(" $pFunction = 'ANRS' ", [
new ConstantText(@"anesthesia nurse"), 
]),
new When(" $pFunction = 'ATTPHYS' ", [
new ConstantText(@"attending physician"), 
]),
new When(" $pFunction = 'DISPHYS' ", [
new ConstantText(@"discharging physician"), 
]),
new When(" $pFunction = 'FASST' ", [
new ConstantText(@"first assistant surgeon"), 
]),
new When(" $pFunction = 'MDWF' ", [
new ConstantText(@"midwife"), 
]),
new When(" $pFunction = 'NASST' ", [
new ConstantText(@"nurse assistant"), 
]),
new When(" $pFunction = 'PCP' ", [
new ConstantText(@"primary care physician"), 
]),
new When(" $pFunction = 'PRISURG' ", [
new ConstantText(@"primary surgeon"), 
]),
new When(" $pFunction = 'RNDPHYS' ", [
new ConstantText(@"rounding physician"), 
]),
new When(" $pFunction = 'SASST' ", [
new ConstantText(@"second assistant surgeon"), 
]),
new When(" $pFunction = 'SNRS' ", [
new ConstantText(@"scrub nurse"), 
]),
new When(" $pFunction = 'TASST' ", [
new ConstantText(@"third assistant"), 
]),
], [
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
