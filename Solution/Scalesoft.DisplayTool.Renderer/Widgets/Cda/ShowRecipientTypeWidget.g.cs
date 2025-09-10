using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class ShowRecipientTypeWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Choose([
new When("$typeCode='PRCP'", [
new ConstantText(@"
                Primary Recipient:
            "), 
]),
new When("$typeCode='TRC'", [
new ConstantText(@"
                Secondary Recipient:
            "), 
]),
], [
new ConstantText(@"
                Recipient:
            "), 
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
