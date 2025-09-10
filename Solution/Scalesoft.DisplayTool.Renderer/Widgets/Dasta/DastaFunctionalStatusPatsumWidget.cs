using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Dasta;

public class DastaFunctionalStatusPatsumWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        List<Widget> widgetTree =
        [
            new Variable("functionalStatus",
                "/ds:dasta/ds:is/dsip:ip/dsip:ku/dsip:ku_z[@typku='PATSUM.DAT']/dsip:ku_z_patsumdat/dsip:fs[@typ='A']"),
            new Variable("noFunctionalStatus",
                "/ds:dasta/ds:is/dsip:ip/dsip:ku/dsip:ku_z[@typku='PATSUM.DAT']/dsip:ku_z_patsumdat/dsip:fs[@typ='N']"),
            new Variable("unknownFunctionalStatus",
                "/ds:dasta/ds:is/dsip:ip/dsip:ku/dsip:ku_z[@typku='PATSUM.DAT']/dsip:ku_z_patsumdat/dsip:fs[@typ='U']"),
            new Section("/ds:dasta/ds:is/dsip:ip/dsip:ku/dsip:ku_z[@typku='PATSUM.DAT']/dsip:ku_z_patsumdat/dsip:fs[1]", null,
                [new ConstantText("Funkční stav")],
                [
                    new Choose(
                    [
                        new When("$noFunctionalStatus", [new ConstantText("nebyla dosud žádná zaznamenaná informace o funkčním stavu pacienta")]), // no suitable eHDSI labels found
                        new When("$unknownFunctionalStatus", [new ConstantText("neznámá informace (informace nedostupná - tj. mohou i nemusí být patřičné údaje)")]), // no suitable eHDSI labels found
                    ], [
                        new DastaText("$functionalStatus/dsip:text"),
                        // ignore autor
                        // ignore @dat_ab
                        // ignore iid
                        // ignore @ind_oprav_sd
                    ]),
                ], titleAbbreviations: ("FS", "FS")),
        ];

        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
