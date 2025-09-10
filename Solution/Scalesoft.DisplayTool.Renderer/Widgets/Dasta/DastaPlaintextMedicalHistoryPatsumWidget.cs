using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Dasta;

public class DastaPlaintextMedicalHistoryPatsumWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        List<Widget> widgetTree =
        [
            new Variable("medicationHistoryPlaintext",
                "/ds:dasta/ds:is/dsip:ip/dsip:ku/dsip:ku_z[@typku='PATSUM.DAT']/dsip:ku_z_patsumdat/dsip:an"), // coded medications are in anf blocks
            new Section("/ds:dasta/ds:is/dsip:ip/dsip:ku/dsip:ku_z[@typku='PATSUM.DAT']/dsip:ku_z_patsumdat/dsip:an[1]", null,
                [new ConstantText("Souhrnná anamnéza společná pro všechny obory medicíny (volným textem)")],
                [
                    new Table([
                        new TableCell([new ConstantText("Obsah anamnézy")], TableCellType.Header),
                        new ChangeContext("$medicationHistoryPlaintext", 
                        [
                            new TableRow([
                                // ignore garant_dat
                                new TableCell([new DastaText("dsip:text")]),
                                // ignore autor
                                // ignore dat_ak, unused in DASTA
                                // ignore dat_ab
                                // ignore iid
                                // ignore @ind_oprav_sd, according to Hynek Kružík
                            ])
                        ]),
                    ]),
                ], titleAbbreviations: ("SA", "HS")),
        ];

        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
