using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Alert.RiskAssessment;

public class RiskAssessmentBasicInfoCell(XmlDocumentNavigator item) : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var infrequentOptions = InfrequentProperties.Evaluate<InfrequentPropertiesPaths>([item]);

        var actorsTableCell = new TableCell([
            infrequentOptions.Contains(InfrequentPropertiesPaths.Condition)
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new ConstantText("Posuzovaný zdravotní stav")]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular,
                        [new Optional("f:condition", new AnyReferenceNamingWidget())]),
                    new LineBreak()
                ])
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Code)
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new ConstantText("Popis")]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular,
                        [new Optional("f:code", new CodeableConcept())]),
                    new LineBreak()
                ])
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Method)
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new ConstantText("Metoda posouzení")]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular,
                        [new Optional("f:method", new CodeableConcept())]),
                    new LineBreak()
                ])
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.ReasonCode)
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new ConstantText("Důvody")]),
                    new ConstantText(": "),
                    new LineBreak(),
                    new TextContainer(TextStyle.Regular,
                        [new ItemListBuilder("f:reasonCode", ItemListType.Unordered, _ => [new CodeableConcept()])]),
                    new LineBreak()
                ])
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.ReasonReference)
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new ConstantText("Odkazy na důvod")]),
                    new ConstantText(": "),
                    new LineBreak(),
                    new TextContainer(TextStyle.Regular,
                    [
                        new ItemListBuilder("f:reasonReference", ItemListType.Unordered,
                            _ => [new AnyReferenceNamingWidget()])
                    ]),
                    new LineBreak()
                ])
                : new NullWidget(),
        ]);

        if (infrequentOptions.Count == 0)
        {
            actorsTableCell = new TableCell([
                new TextContainer(TextStyle.Muted, [new ConstantText("Informace nejsou k dispozici")])
            ]);
        }

        return actorsTableCell.Render(item, renderer, context);
    }

    private enum InfrequentPropertiesPaths
    {
        Condition, //0..1	Reference(Condition) - Condition assessed
        Code, //0..1	CodeableConcept - description
        ReasonCode, //0..*	CodeableConcept
        ReasonReference, //Reference(Condition | Observation | DiagnosticReport | DocumentReference)
        Method, //0..1	CodeableConcept - evaluation mechanism
    }
}