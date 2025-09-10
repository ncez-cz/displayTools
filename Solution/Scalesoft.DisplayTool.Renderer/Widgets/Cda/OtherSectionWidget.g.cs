using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class OtherSectionWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Variable("otherSectionTitleCode", "n1:code/@code"), 
new Variable("otherSectionTitle", "n1:code[@code!='48765-2'][@code!='47420-5'][@code!='11450-4'][@code!='30954-2'][@code!='11348-0'][@code!='46264-8'][@code!='10160-0'][@code!='8716-3'][@code!='10162-6'][@code!='29762-2'][@code!='47519-4'][@code!='18776-5'][@code!='11369-6']/@displayName"), 
new Variable("otherSectionText", "n1:code[@code!='48765-2'][@code!='47420-5'][@code!='11450-4'][@code!='30954-2'][@code!='11348-0'][@code!='46264-8'][@code!='10160-0'][@code!='8716-3'][@code!='10162-6'][@code!='29762-2'][@code!='47519-4'][@code!='18776-5'][@code!='11369-6']/../n1:text"), 
new Choose([
new When("($otherSectionTitleCode!='48765-2' and $otherSectionTitleCode!='47420-5' and $otherSectionTitleCode!='11450-4' and $otherSectionTitleCode!='30954-2' and $otherSectionTitleCode!='11348-0' and $otherSectionTitleCode!='46264-8' and $otherSectionTitleCode!='10160-0' and $otherSectionTitleCode!='8716-3' and $otherSectionTitleCode!='10162-6' and $otherSectionTitleCode!='29762-2' and $otherSectionTitleCode!='47519-4' and $otherSectionTitleCode!='18776-5' and $otherSectionTitleCode!='11369-6')", [
new Container([
new Text("$otherSectionTitle")
, 
], ContainerType.Span), 
new LineBreak(), 
new WidgetWithVariables(new ShowNarrativeWidget(), [
new Variable("node", "/n1:ClinicalDocument/n1:component/n1:structuredBody/n1:component/n1:section/n1:code[@code!='48765-2'][@code!='47420-5'][@code!='11450-4'][@code!='30954-2'][@code!='11348-0'][@code!='46264-8'][@code!='10160-0'][@code!='8716-3'][@code!='10162-6'][@code!='29762-2'][@code!='47519-4'][@code!='18776-5'][@code!='11369-6']/../n1:text"),
]), 
]),
], [
]), 
new ConcatBuilder("n1:component/n1:section", (i) => [
new WidgetWithVariables(new OtherSectionWidget(), [
]), 
])
, 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
