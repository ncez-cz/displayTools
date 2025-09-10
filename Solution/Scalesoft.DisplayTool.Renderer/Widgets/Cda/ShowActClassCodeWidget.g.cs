using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class ShowActClassCodeWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Choose([
new When(" $clsCode = 'ACT' ", [
new ConstantText(@"healthcare service"), 
]),
new When(" $clsCode = 'ACCM' ", [
new ConstantText(@"accommodation"), 
]),
new When(" $clsCode = 'ACCT' ", [
new ConstantText(@"account"), 
]),
new When(" $clsCode = 'ACSN' ", [
new ConstantText(@"accession"), 
]),
new When(" $clsCode = 'ADJUD' ", [
new ConstantText(@"financial adjudication"), 
]),
new When(" $clsCode = 'CONS' ", [
new ConstantText(@"consent"), 
]),
new When(" $clsCode = 'CONTREG' ", [
new ConstantText(@"container registration"), 
]),
new When(" $clsCode = 'CTTEVENT' ", [
new ConstantText(@"clinical trial timepoint event"), 
]),
new When(" $clsCode = 'DISPACT' ", [
new ConstantText(@"disciplinary action"), 
]),
new When(" $clsCode = 'ENC' ", [
new ConstantText(@"encounter"), 
]),
new When(" $clsCode = 'INC' ", [
new ConstantText(@"incident"), 
]),
new When(" $clsCode = 'INFRM' ", [
new ConstantText(@"inform"), 
]),
new When(" $clsCode = 'INVE' ", [
new ConstantText(@"invoice element"), 
]),
new When(" $clsCode = 'LIST' ", [
new ConstantText(@"working list"), 
]),
new When(" $clsCode = 'MPROT' ", [
new ConstantText(@"monitoring program"), 
]),
new When(" $clsCode = 'PCPR' ", [
new ConstantText(@"care provision"), 
]),
new When(" $clsCode = 'PROC' ", [
new ConstantText(@"procedure"), 
]),
new When(" $clsCode = 'REG' ", [
new ConstantText(@"registration"), 
]),
new When(" $clsCode = 'REV' ", [
new ConstantText(@"review"), 
]),
new When(" $clsCode = 'SBADM' ", [
new ConstantText(@"substance administration"), 
]),
new When(" $clsCode = 'SPCTRT' ", [
new ConstantText(@"speciment treatment"), 
]),
new When(" $clsCode = 'SUBST' ", [
new ConstantText(@"substitution"), 
]),
new When(" $clsCode = 'TRNS' ", [
new ConstantText(@"transportation"), 
]),
new When(" $clsCode = 'VERIF' ", [
new ConstantText(@"verification"), 
]),
new When(" $clsCode = 'XACT' ", [
new ConstantText(@"financial transaction"), 
]),
], [
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
