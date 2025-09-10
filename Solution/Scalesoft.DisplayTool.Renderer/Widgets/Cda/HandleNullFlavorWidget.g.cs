using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class HandleNullFlavorWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Choose([
new When("$node/@nullFlavor='OTH'", [
new Tooltip([], [
new Icon(SupportedIcons.CircleExclamation), 
new Container([
new WidgetWithVariables(new ShowEHdsiDisplayLabelWidget(), [
new Variable("code", "'153'"),
]), 
], ContainerType.Span), 
]), 
new ConstantText(@" "), 
new TextContainer(TextStyle.Italic, [
new Text("$node/n1:translation/@displayName")
, 
]), 
]),
], [
new WidgetWithVariables(new ShowEHdsiNullFlavorWidget(), [
new Variable("code", "$node/@nullFlavor"),
]), 
]), 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
