using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class ShowParticipationTypeWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Choose([
new When(" $ptype='PPRF' ", [
new ConstantText(@"primary performer"), 
]),
new When(" $ptype='PRF' ", [
new ConstantText(@"performer"), 
]),
new When(" $ptype='VRF' ", [
new ConstantText(@"verifier"), 
]),
new When(" $ptype='SPRF' ", [
new ConstantText(@"secondary performer"), 
]),
], [
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
