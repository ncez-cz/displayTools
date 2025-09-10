using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Dasta;

public class DastaWeightAndHeightPatsumWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        List<Widget> widgetTree =
        [
            new Variable("weightAndHeight",
                "/ds:dasta/ds:is/dsip:ip/dsip:ku/dsip:ku_z[@typku='PATSUM.DAT']/dsip:ku_z_patsumdat/dsip:h"),
            new Section("/ds:dasta/ds:is/dsip:ip/dsip:ku/dsip:ku_z[@typku='PATSUM.DAT']/dsip:ku_z_patsumdat/dsip:h[1]", null,
                [new ConstantText("Běžná standardní výška a hmotnost pacienta")],
                [
                    new Table([
                        new TableCell([new ConstantText("Výška pacienta [cm]")], TableCellType.Header),
                        new TableCell([new ConstantText("Hmotnost pacienta [kg]")], TableCellType.Header),
                        new TableCell([new ConstantText("BMI index")], TableCellType.Header),
                        new TableCell([new ConstantText("Obvod hlavy [cm]")], TableCellType.Header),
                        new TableCell([new ConstantText("Obvod hrudníku [cm]")], TableCellType.Header),
                        new TableCell([new ConstantText("Obvod pasu [cm]")], TableCellType.Header),
                        new TableCell([new ConstantText("Poznámka k údajům")], TableCellType.Header),
                        new TableCell([new ConstantText("Datum (a čas) změření")], TableCellType.Header),
                        new ConcatBuilder("$weightAndHeight", i =>
                        [
                            new TableRow([
                                new TableCell([
                                    new Text("@vyska"),
                                ]),
                                new TableCell([
                                    new Text("@hmotnost"),
                                ]),
                                new TableCell([
                                    new Text("@bmi"),
                                ]),
                                new TableCell([
                                    new Text("@o_hlava"),
                                ]),
                                new TableCell([
                                    new Text("@o_hrudnik"),
                                ]),
                                new TableCell([
                                    new Text("@o_pas"),
                                ]),
                                new TableCell([
                                    new Text("dsip:pozn"),
                                ]),
                                // ignore autor
                                new TableCell([
                                    new DastaDate("dsip:dat_vys"),
                                ]),
                                // ignore dat_ab
                                // ignore iid
                            ])
                        ]),
                    ]),
                ], titleAbbreviations: ("VH", "HW")),
        ];

        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
