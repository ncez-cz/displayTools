using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.MedicationResources;

public class MedicationStatementNameValuePairs : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var list =
            new Concat([
                new Optional("f:manufacturer",
                    new NameValuePair(
                        new ConstantText("Výrobce"),
                        new AnyReferenceNamingWidget(),
                        direction: FlexDirection.Column,
                        style: NameValuePair.NameValuePairStyle.Primary
                    )
                ),
                new Optional("f:form",
                    new NameValuePair(
                        new ConstantText("Forma"),
                        new CodeableConcept(),
                        direction: FlexDirection.Column,
                        style: NameValuePair.NameValuePairStyle.Primary
                    )
                ),
                new Condition("f:ingredient",
                    new NameValuePair(
                        new ConstantText("Látky"),
                        new CommaSeparatedBuilder("f:ingredient", _ =>
                        [
                            new OpenTypeElement(null, "item")
                        ]),
                        direction: FlexDirection.Column,
                        style: NameValuePair.NameValuePairStyle.Primary
                    )
                ),
                new Optional("f:amount",
                    new NameValuePair(
                        new ConstantText("Množství"),
                        new ShowRatio(),
                        direction: FlexDirection.Column,
                        style: NameValuePair.NameValuePairStyle.Primary
                    )
                ),
                new Optional("f:batch/f:lotNumber",
                    new NameValuePair(
                        new ConstantText("Číslo šarže"),
                        new Text("@value"),
                        direction: FlexDirection.Column,
                        style: NameValuePair.NameValuePairStyle.Primary
                    )
                ),
                new Optional("f:batch/f:expirationDate",
                    new NameValuePair(
                        new ConstantText("Datum expirace"),
                        new ShowDateTime(),
                        direction: FlexDirection.Column,
                        style: NameValuePair.NameValuePairStyle.Primary
                    )
                ),
            ]);

        return list.Render(navigator, renderer, context);
    }
}