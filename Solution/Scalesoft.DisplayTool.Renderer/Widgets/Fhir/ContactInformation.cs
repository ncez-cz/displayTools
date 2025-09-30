using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class ContactInformation(string addressPath = "f:address", string telecomPath = "f:telecom") : Widget
{
    public override async Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        List<Widget> contact =
        [
            new If(_ => navigator.EvaluateCondition(addressPath) ||
                        navigator.EvaluateCondition(telecomPath),
                new PlainBadge(new DisplayLabel(LabelCodes.ContactInformation)), new Row(
                    [
                        new If(_ => navigator.EvaluateCondition(addressPath), new Address(addressPath)),
                        new If(_ => navigator.EvaluateCondition(telecomPath),
                            new NameValuePair([new DisplayLabel(LabelCodes.Telecom)],
                                [new ShowContactPoint(telecomPath)], direction: FlexDirection.Column)),
                    ],
                    flexContainerClasses: "column-gap-8")),
        ];

        return await contact.RenderConcatenatedResult(navigator, renderer, context);
    }
}