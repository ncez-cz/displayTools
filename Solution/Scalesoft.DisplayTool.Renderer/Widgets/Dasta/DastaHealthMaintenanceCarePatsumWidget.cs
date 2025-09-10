using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Dasta;

public class DastaHealthMaintenanceCarePatsumWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        List<Widget> widgetTree =
        [
            new Variable("carePlan",
                "/ds:dasta/ds:is/dsip:ip/dsip:ku/dsip:ku_z[@typku='PATSUM.DAT']/dsip:ku_z_patsumdat/dsip:ld[@typ='A']"),
            new Variable("noCarePlan",
                "/ds:dasta/ds:is/dsip:ip/dsip:ku/dsip:ku_z[@typku='PATSUM.DAT']/dsip:ku_z_patsumdat/dsip:ld[@typ='N']"),
            new Variable("unknownCarePlan",
                "/ds:dasta/ds:is/dsip:ip/dsip:ku/dsip:ku_z[@typku='PATSUM.DAT']/dsip:ku_z_patsumdat/dsip:ld[@typ='U']"),
            new Section("/ds:dasta/ds:is/dsip:ip/dsip:ku/dsip:ku_z[@typku='PATSUM.DAT']/dsip:ku_z_patsumdat/dsip:ld[1]", null,
                [new ConstantText("Léčebná doporučení")],
                [
                    new Choose(
                    [
                        new When("$noCarePlan", [new ConstantText("nebyla dosud žádné zaznamenatelné léčebná doporučení")]), // no suitable eHDSI labels found
                        new When("$unknownCarePlan", [new ConstantText("neznámá informace (informace nedostupná - tj. mohou i nemusí být patřičné údaje)")]), // no suitable eHDSI labels found
                    ], [
                        new DastaText("$carePlan/dsip:text"),
                        // ignore autor
                        // ignore @dat_ab
                        // ignore iid
                        // ignore @ind_oprav_sd
                    ]),
                ], titleAbbreviations: ("LD", "CP")),
        ];

        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
