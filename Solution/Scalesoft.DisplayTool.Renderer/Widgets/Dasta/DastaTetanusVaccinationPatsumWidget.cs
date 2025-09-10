using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Dasta;

public class DastaTetanusVaccinationPatsumWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        List<Widget> widgetTree =
        [
            new Variable("tetanusVaccinationPlaintext",
                "/ds:dasta/ds:is/dsip:ip/dsip:ku/dsip:ku_z[@typku='PATSUM.DAT']/dsip:ku_z_patsumdat/dsip:u/dsip:uot"), // coded vaccination are in oc/ocz blocks
            new Section("/ds:dasta/ds:is/dsip:ip/dsip:ku/dsip:ku_z[@typku='PATSUM.DAT']/dsip:ku_z_patsumdat/dsip:u/dsip:uot", null,
                [new ConstantText("Urgentní informace - očkování proti tetanu")],
                [
                    new Table([
                        new TableCell([new ConstantText("Datum očkování proti tetanu")], TableCellType.Header),
                        new ConcatBuilder("$medicationPlaintext", i =>
                        [
                            new TableRow([
                                new TableCell([new DastaDate("dsip:dat_du")]),
                                // ignore autor
                                // ignore dat_ab
                                // ignore iid
                                // ignore @ind_oprav_sd, according to Hynek Kružík
                            ])
                        ]),
                    ]),
                ], titleAbbreviations: ("OT", "TV")),
        ];

        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
