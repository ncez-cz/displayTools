using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class ShowAddressWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Choose([
new When("$address", [
new Condition("$address/@nullFlavor", [
new WidgetWithVariables(new ShowEHdsiNullFlavorWidget(), [
new Variable("code", "$address/@nullFlavor"),
]), 
])
, 
new Condition("$address/@use", [
new ConstantText(@" "), 
new WidgetWithVariables(new ShowEHdsiTelecomAddressWidget(), [
new Variable("code", "$address/@use"),
]), 
new ConstantText(@":"), 
new LineBreak(), 
])
, 
new ConcatBuilder("$address/n1:streetAddressLine", (i) => [
new Text(".")
, 
new LineBreak(), 
])
, 
new Condition("$address/n1:streetName", [
new Text("$address/n1:streetName")
, 
new ConstantText(@" "), 
new Text("$address/n1:houseNumber")
, 
new LineBreak(), 
])
, 
new Condition("string-length($address/n1:city)>0", [
new Text("$address/n1:city")
, 
])
, 
new Condition("string-length($address/n1:state)>0", [
new ConstantText(@", "), 
new Text("$address/n1:state")
, 
])
, 
new Condition("string-length($address/n1:postalCode)>0", [
new ConstantText(@" "), 
new Text("$address/n1:postalCode")
, 
])
, 
new Condition("string-length($address/n1:country)>0", [
new ConstantText(@", "), 
new Text("$address/n1:country")
, 
])
, 
]),
], [
]), 
new LineBreak(), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
