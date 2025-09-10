using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Consent;

public class PolicyDetailsTable : Widget
{
    public override async Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var policies = navigator.SelectAllNodes("f:policy").ToList();

        var infrequentProperties =
            InfrequentProperties.Evaluate<PolicyInfrequentProperties>(policies);

        var policyTable = new Table([
            new TableHead([
                new TableRow([
                    new TableCell([new ConstantText("#")], TableCellType.Header),
                    new If(_ => infrequentProperties.Contains(PolicyInfrequentProperties.Authority),
                        new TableCell([new ConstantText("Authority")], TableCellType.Header)
                    ),
                    new If(_ => infrequentProperties.Contains(PolicyInfrequentProperties.Uri),
                        new TableCell([new ConstantText("URI")], TableCellType.Header)
                    )
                ])
            ]),
            new TableBody([
                new ConcatBuilder("f:policy", i =>
                [
                    new TableRow([
                        new TableCell([new ConstantText($"{i + 1}")]),
                        new If(_ => infrequentProperties.Contains(PolicyInfrequentProperties.Authority),
                            new TableCell([
                                new Optional("f:authority",
                                    new Text("@value")
                                ),
                            ])
                        ),
                        new If(_ => infrequentProperties.Contains(PolicyInfrequentProperties.Uri),
                            new TableCell([
                                new Optional("f:uri",
                                    new Link(new Text("@value"), new Text("@value"))
                                )
                            ])
                        )
                    ])
                ])
            ])
        ]);

        return await policyTable.Render(navigator, renderer, context);
    }

    private enum PolicyInfrequentProperties
    {
        Authority,
        Uri
    }
}