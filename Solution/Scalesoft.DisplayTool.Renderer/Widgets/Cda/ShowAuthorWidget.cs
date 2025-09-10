using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class ShowAuthorWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        List<Widget> widgetTree =
        [
            new Choose([
                new When("$node/n1:assignedAuthor/n1:assignedPerson", [
                    new WidgetWithVariables(new ShowNameWidget(true), [
                        new Variable("name", "$node/n1:assignedAuthor/n1:assignedPerson/n1:name"),
                    ]),
                ]),
                new When("$node/n1:assignedAuthor/n1:assignedAuthoringDevice", [
                    new Text("$node/n1:assignedAuthor/n1:assignedAuthoringDevice/n1:manufacturerModelName"),
                    new ConstantText(@", "),
                    new Text("$node/n1:assignedAuthor/n1:assignedAuthoringDevice/n1:softwareName"),
                ]),
            ], [
            ]),
            new Condition("$node/n1:assignedAuthor/n1:representedOrganization", [
                new Text("$node/n1:assignedAuthor/n1:representedOrganization/n1:name")
            ])
        ];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}