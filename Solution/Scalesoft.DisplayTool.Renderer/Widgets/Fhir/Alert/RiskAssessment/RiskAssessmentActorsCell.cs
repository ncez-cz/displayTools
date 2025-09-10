using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.Alert.RiskAssessment;

public class RiskAssessmentActorsCell(XmlDocumentNavigator item): Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer, RenderContext context)
    {
        var infrequentOptions = InfrequentProperties.Evaluate<InfrequentPropertiesPaths>([item]);
        
        var actorsTableCell = new TableCell([
            infrequentOptions.Contains(InfrequentPropertiesPaths.Performer)
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new ConstantText("Provedl(a)")]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular,
                        [new Optional("f:performer", new AnyReferenceNamingWidget("."))]), 
                    new LineBreak()
                ])
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Parent)
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new ConstantText("Nadřazený záznam")]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular,
                        [new Optional("f:parent", new AnyReferenceNamingWidget("."))]), 
                    new LineBreak()
                ])
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Subject)
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new ConstantText("Subjekt")]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular,
                        [new Optional("f:subject", new AnyReferenceNamingWidget("."))]), 
                    new LineBreak()
                ])
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.BasedOn)
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new ConstantText("Na základě")]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular,
                        [new Optional("f:basedOn", new AnyReferenceNamingWidget("."))]), 
                    new LineBreak()
                ])
                : new NullWidget(),
        ]);
        
        if (infrequentOptions.Count == 0)
        {
            actorsTableCell = new TableCell([ new TextContainer(TextStyle.Muted, [new ConstantText("Informace nejsou k dispozici")])]);
        }
        
        return actorsTableCell.Render(item, renderer, context);
    }
    
    private enum InfrequentPropertiesPaths
    {
        BasedOn, //	0..1	Reference(Any)
        Parent, //	0..1	Reference(Any)
        Subject, //1..1	Reference(Patient | Group)
        Encounter, //0..1	Reference(Encounter)
        Performer, //0..1	Reference(Practitioner | PractitionerRole | Device)
    }
    
}