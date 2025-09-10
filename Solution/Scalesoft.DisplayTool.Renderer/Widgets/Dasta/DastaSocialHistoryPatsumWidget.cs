using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Dasta;

public class DastaSocialHistoryPatsumWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        List<Widget> widgetTree =
        [
            new Variable("socialHistory",
                "/ds:dasta/ds:is/dsip:ip/dsip:ku/dsip:ku_z[@typku='PATSUM.DAT']/dsip:ku_z_patsumdat/dsip:fzs[@typ='A']"),
            new Variable("noSocialHistory",
                "/ds:dasta/ds:is/dsip:ip/dsip:ku/dsip:ku_z[@typku='PATSUM.DAT']/dsip:ku_z_patsumdat/dsip:fzs[@typ='N']"),
            new Variable("unknownSocialHistory",
                "/ds:dasta/ds:is/dsip:ip/dsip:ku/dsip:ku_z[@typku='PATSUM.DAT']/dsip:ku_z_patsumdat/dsip:fzs[@typ='U']"),
            new Section("/ds:dasta/ds:is/dsip:ip/dsip:ku/dsip:ku_z[@typku='PATSUM.DAT']/dsip:ku_z_patsumdat/dsip:fzs[1]", null,
                [new ConstantText("Faktory životního stylu")],
                [
                    new Choose(
                    [
                        new When("$noSocialHistory", [new ConstantText("nebyly dosud žádné zaznamenatelné faktory životního stylu")]), // no suitable eHDSI labels found
                        new When("$unknownSocialHistory", [new ConstantText("neznámá informace (není známo, informace nedostupná - tj. mohou i nemusí být patřičné údaje)")]), // no suitable eHDSI labels found
                    ], [
                        // fzs is not implemented in dasta
                    ]),
                ], titleAbbreviations: ("ŽS", "SH")),
        ];

        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
