using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Dasta;

public class DastaPhysicalFindingsPatsumWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        List<Widget> widgetTree =
        [
            new Variable("physicalFindings",
                "/ds:dasta/ds:is/dsip:ip/dsip:ku/dsip:ku_z[@typku='PATSUM.DAT']/dsip:ku_z_patsumdat/dsip:fyznal"),
            new Section("/ds:dasta/ds:is/dsip:ip/dsip:ku/dsip:ku_z[@typku='PATSUM.DAT']/dsip:ku_z_patsumdat/dsip:fyznal[1]", null,
                [new ConstantText("Fyzikální vyšetření")],
                [
                    new Table([
                        new TableCell([new ConstantText("Tlak systolický / tlak diastolický [mmHg]")], TableCellType.Header),
                        new TableCell([new ConstantText("Srdeční frekvence [1/min]")], TableCellType.Header),
                        new TableCell([new ConstantText("Saturace Hb kyslíkem [1]")], TableCellType.Header),
                        new TableCell([new ConstantText("Dechová frekvence [1/min]")], TableCellType.Header),
                        new TableCell([new ConstantText("Tělesná teplota aktuální [°C]")], TableCellType.Header),
                        new TableCell([new ConstantText("Poznámka k údajům")], TableCellType.Header),
                        new TableCell([new ConstantText("Datum (a čas) vyšetření (změření)")], TableCellType.Header),
                        new ConcatBuilder("$physicalFindings", i =>
                        [
                            new TableRow([
                                new TableCell([
                                    new Text("dsip:tk_syst"),
                                    new ConstantText(" / "),
                                    new Text("dsip:tk_diast"),
                                ]),
                                new TableCell([
                                    new Text("dsip:pulz"),
                                ]),
                                new TableCell([
                                    new Text("dsip:o2sat"),
                                ]),
                                new TableCell([
                                    new Text("dsip:dech")
                                ]),
                                new TableCell([
                                    new Text("dsip:teplota")
                                ]),
                                new TableCell([
                                    new Text("dsip:pozn")
                                ]),
                                // ignore autor
                                new TableCell([
                                    new DastaDate("dsip:dat_vys")
                                ]),
                                // ignore dat_ab
                                // ignore iid
                                // ignore @ind_oprav_sd
                            ])
                        ]),
                    ]),
                ], titleAbbreviations: ("FN", "PF")),
        ];

        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
