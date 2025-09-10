using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.PatientSection;

public class CoreActors : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var widget = new Container(
            content:
            [
                #region General practitioner

                new If(_ => navigator.EvaluateCondition("f:generalPractitioner/f:reference"),
                    new ConcatBuilder("f:generalPractitioner", _ =>
                    [
                        ShowSingleReference.WithDefaultDisplayHandler(nav =>
                        [
                            new Container([
                                new PersonOrOrganization(
                                    nav,
                                    skipWhenInactive: true,
                                    collapserTitle: new DisplayLabel(LabelCodes.PreferredProvider)
                                ),
                            ], idSource: nav),
                        ]),
                    ])
                ),

                #endregion

                #region Contacts

                new If(_ => navigator.EvaluateCondition("f:contact"),
                    new ConcatBuilder("f:contact", (_, _, nav) =>
                        [
                            new PersonOrOrganization(
                                nav,
                                skipWhenInactive: true,
                                showNarrative: true,
                                collapserTitle: new HumanNameCompact("f:name"),
                                collapserSubtitle:
                                [
                                    new Condition("f:relationship",
                                        new TextContainer(TextStyle.Bold, new ConstantText("Vztah: ")),
                                        new CommaSeparatedBuilder("f:relationship", _ => [new CodeableConcept()])),
                                ]
                            ),
                        ]
                    )
                ),

                #endregion
            ],
            optionalClass: "patient-cards-layout"
        );

        return widget.Render(navigator, renderer, context);
    }
}