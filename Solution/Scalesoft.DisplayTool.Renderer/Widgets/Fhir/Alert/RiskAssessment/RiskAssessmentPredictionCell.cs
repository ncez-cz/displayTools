using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Alert.RiskAssessment;

public class RiskAssessmentPredictionCell(XmlDocumentNavigator item) : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer, RenderContext context)
    {
        var predictionNav = item.SelectSingleNode("f:prediction");
        var infrequentOptions = InfrequentProperties.Evaluate<InfrequentPropertiesPaths>([predictionNav]);

        var actorsTableCell = new TableCell(
        [
            new ChangeContext(predictionNav,
                infrequentOptions.Contains(InfrequentPropertiesPaths.Outcome)
                    ? new TextContainer(TextStyle.Regular, [
                        new TextContainer(TextStyle.Bold, [new ConstantText("Důsledek")]),
                        new ConstantText(": "),
                        new TextContainer(TextStyle.Regular,
                            [new Optional("f:outcome", new CodeableConcept())]),
                        new LineBreak()
                    ])
                    : new NullWidget(),
                infrequentOptions.Contains(InfrequentPropertiesPaths.Probability)
                    ? new TextContainer(TextStyle.Regular, [
                        new TextContainer(TextStyle.Bold, [new ConstantText("Pravděpodobnost")]),
                        new ConstantText(": "),
                        new TextContainer(TextStyle.Regular, [new OpenTypeElement(null, "probability")]), // decimal | Range
                        new LineBreak()
                    ])
                    : new NullWidget(),
                infrequentOptions.Contains(InfrequentPropertiesPaths.QualitativeRisk)
                    ? new TextContainer(TextStyle.Regular, [
                        new TextContainer(TextStyle.Bold, [new ConstantText("Kvalitativní riziko")]),
                        new ConstantText(": "),
                        new TextContainer(TextStyle.Regular,
                            [new Optional("f:qualitativeRisk", new CodeableConcept())]),
                        new LineBreak()
                    ])
                    : new NullWidget(),
                infrequentOptions.Contains(InfrequentPropertiesPaths.RelativeRisk)
                    ? new TextContainer(TextStyle.Regular, [
                        new TextContainer(TextStyle.Bold, [new ConstantText("Relativní riziko")]),
                        new ConstantText(": "),
                        new TextContainer(TextStyle.Regular,
                            [new Optional("f:relativeRisk", new ShowDecimal())]),
                        new LineBreak()
                    ])
                    : new NullWidget(),
                infrequentOptions.Contains(InfrequentPropertiesPaths.When)
                    ? new TextContainer(TextStyle.Regular, [
                        new TextContainer(TextStyle.Bold, [new ConstantText("Období")]),
                        new ConstantText(": "),
                        new TextContainer(TextStyle.Regular,
                            [new OpenTypeElement(null, "when")]), // Period | Range
                        new LineBreak()
                    ])
                    : new NullWidget(),
                infrequentOptions.Contains(InfrequentPropertiesPaths.Rationale)
                    ? new TextContainer(TextStyle.Regular, [
                        new TextContainer(TextStyle.Bold, [new ConstantText("Vysvětlení")]),
                        new ConstantText(": "),
                        new TextContainer(TextStyle.Regular,
                            [new Optional("f:rationale", new Text("@value"))]),
                        new LineBreak()
                    ])
                    : new NullWidget()
            )
        ]);

        if (infrequentOptions.Count == 0)
        {
            actorsTableCell = new TableCell([new TextContainer(TextStyle.Muted, [new ConstantText("Informace nejsou k dispozici")])]);
        }

        return actorsTableCell.Render(item, renderer, context);
    }

    private enum InfrequentPropertiesPaths
    {
        /*Prediction 0..*	BackboneElement*/
        Outcome, //0..1	CodeableConcept	- Possible outcome for the subject
        [OpenType("probability")] Probability,
        QualitativeRisk, //	0..1	CodeableConcept	- Likelihood of specified outcome as a qualitative value
        RelativeRisk, //0..1	decimal	- Relative likelihood
        [OpenType("when")] When, //0..1 Timeframe or age range
        Rationale, //0..1	string	- Explanation of prediction
    }
}