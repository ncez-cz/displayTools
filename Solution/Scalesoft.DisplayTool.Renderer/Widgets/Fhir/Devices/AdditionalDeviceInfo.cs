using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Devices;

public class AdditionalDeviceInfo(
    XmlDocumentNavigator item,
    bool addIdentifier = false,
    bool addVisualIdentifiers = true
) : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        navigator = item;

        List<Widget> tree =
        [
            new TableCell([
                    new Optional("f:note", new TextContainer(TextStyle.Bold, [new ConstantText("Poznámky: ")])),
                    new CommaSeparatedBuilder("f:note",
                        _ => [new ShowAnnotationCompact()]),
                    new Optional("f:note",
                        new OptionalLineBreak(
                            [
                                "../f:version"
                            ]
                        )
                    ),
                    new CommaSeparatedBuilder("f:version", _ =>
                    [
                        new Optional("f:type", new CodeableConcept(), new ConstantText(" - ")),
                        new Optional("f:component", new ShowIdentifier(), new ConstantText(", ")),
                        new Optional("f:value", new Text("@value"))
                    ]),
                ], idSource: addIdentifier ? navigator : (IdentifierSource?)null,
                visualIdSource: addVisualIdentifiers ? navigator : (IdentifierSource?)null),
        ];

        return tree.RenderConcatenatedResult(navigator, renderer, context);
    }
}