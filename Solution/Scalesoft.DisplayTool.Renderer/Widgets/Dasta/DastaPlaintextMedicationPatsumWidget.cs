using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Dasta;

public class DastaPlaintextMedicationPatsumWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        List<Widget> widgetTree =
        [
            new Variable("medicationPlaintext",
                "/ds:dasta/ds:is/dsip:ip/dsip:ku/dsip:ku_z[@typku='PATSUM.DAT']/dsip:ku_z_patsumdat/dsip:u/dsip:utm"), // coded medications are in le blocks
            new Section("/ds:dasta/ds:is/dsip:ip/dsip:ku/dsip:ku_z[@typku='PATSUM.DAT']/dsip:ku_z_patsumdat/dsip:u/dsip:utm[1]", null,
                [new ConstantText("Trvalá medikace (volným textem)")],
                [
                    new Table([
                        new TableCell([new ConstantText("Trvalá medikace")], TableCellType.Header),
                        new TableCell([new ConstantText("Datum zavedení medikace")], TableCellType.Header),
                        new ConcatBuilder("$medicationPlaintext", i =>
                        [
                            new TableRow([
                                new TableCell([new Text("dsip:u_tm")]),
                                // ignore autor
                                new TableCell([
                                    new DastaDate("dsip:dat_du"),
                                ]),
                                // ignore dat_ab
                                // ignore iid
                                // ignore @ind_oprav_sd, according to Hynek Kružík
                            ])
                        ]),
                    ]),
                ], titleAbbreviations: ("TM", "PM")),
        ];

        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
