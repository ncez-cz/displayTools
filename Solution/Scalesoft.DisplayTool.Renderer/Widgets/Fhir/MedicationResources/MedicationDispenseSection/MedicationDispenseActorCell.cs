using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.MedicationResources.MedicationDispenseSection;

public class MedicationDispenseActorCell(XmlDocumentNavigator item): Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer, RenderContext context)
    {
        var infrequentOptions = InfrequentProperties.Evaluate<InfrequentPropertiesPaths>([item]);
        
        var actorsTableCell = new TableCell(
        [
            infrequentOptions.Contains(InfrequentPropertiesPaths.Performer)
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new ConstantText("Žadatel")]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular,  [new CommaSeparatedBuilder("f:performer", (_, _, nav) => [new Container([new AnyReferenceNamingWidget("f:actor")], ContainerType.Span, idSource: nav)])]),
                    new LineBreak(),
                ]) 
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.AuthorizingPrescription) 
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new ConstantText("Identifikátor žádosti")]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular,[new CommaSeparatedBuilder("f:authorizingPrescription", _ => [new AnyReferenceNamingWidget(".")])]),
                    new LineBreak(),
                ]) 
                : new NullWidget()
        ]);
        
        if (infrequentOptions.Count == 0)
        {
            actorsTableCell = new TableCell([ new TextContainer(TextStyle.Muted, [new ConstantText("Informace nejsou k dispozici")])]);
        }
        
        return actorsTableCell.Render(item, renderer, context);
    }
    
    private enum InfrequentPropertiesPaths
    {
        Performer,
        AuthorizingPrescription
    }
    
}