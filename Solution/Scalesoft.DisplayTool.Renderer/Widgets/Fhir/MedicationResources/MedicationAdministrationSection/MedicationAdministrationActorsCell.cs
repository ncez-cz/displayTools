using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.MedicationResources.MedicationAdministrationSection;

public class MedicationAdministrationActorsCell(XmlDocumentNavigator item): Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer, RenderContext context)
    {
        var infrequentOptions = InfrequentProperties.Evaluate<InfrequentPropertiesPaths>([item]);
        
        var actorsTableCell = new TableCell(
        [
            infrequentOptions.Contains(InfrequentPropertiesPaths.Performer) 
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new ConstantText("Žádatel")]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular, [new CommaSeparatedBuilder("f:performer", (_, _, nav) => [new Container([new AnyReferenceNamingWidget("f:actor")], ContainerType.Span, idSource: nav)])]),
                    new LineBreak(),
                ]) 
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Subject) 
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new ConstantText("Příjemce")]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular, ReferenceHandler.BuildAnyReferencesNaming(item, "f:subject", context, renderer)),
                    new LineBreak(),
                ]) 
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.PartOf) 
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new ConstantText("Související úkony")]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular,[new CommaSeparatedBuilder("f:partOf", _ => [new AnyReferenceNamingWidget(".")])]),
                    new LineBreak(),
                ]) 
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Device)
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new ConstantText("Zařízení")]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular,[new CommaSeparatedBuilder("f:device", _ => [new AnyReferenceNamingWidget(".")])]),
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
        Subject,
        Performer,
        PartOf,
        Device,
    }
}