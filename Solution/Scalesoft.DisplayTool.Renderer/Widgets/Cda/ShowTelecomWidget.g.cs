using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class ShowTelecomWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Choose([
new When("$telecom", [
new Condition("$telecom/@nullFlavor", [
new WidgetWithVariables(new ShowEHdsiNullFlavorWidget(), [
new Variable("code", "$telecom/@nullFlavor"),
]), 
])
, 
new Variable("type", "substring-before($telecom/@value, ':')"), 
new Variable("value", "substring-after($telecom/@value, ':')"), 
new Condition("$type", [
new WidgetWithVariables(new TranslateTelecomCodeWidget(), [
new Variable("code", "$type"),
]), 
])
, 
new Condition("@use", [
new ConstantText(@" ("), 
new WidgetWithVariables(new ShowEHdsiTelecomAddressWidget(), [
new Variable("code", "@use"),
]), 
new ConstantText(@")"), 
new ConstantText(@": "), 
new ConstantText(@" "), 
])
, 
new Text("$value")
, 
]),
], [
]), 
new LineBreak(), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
