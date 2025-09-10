using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.MedicationResources.MedicationAdministrationSection;

public class MedicationAdministrationDosageCell(XmlDocumentNavigator item): Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer, RenderContext context)
    {
        var dosageNav = item.SelectSingleNode("f:dosage");
        var infrequentOptions = InfrequentProperties.Evaluate<InfrequentPropertiesPaths>([dosageNav]);
            
        var dosageInstructionsTableCell = new TableCell(
        [
            infrequentOptions.Contains(InfrequentPropertiesPaths.Route) 
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new DisplayLabel(LabelCodes.AdministrationRoute)]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular,[new Optional("f:dosage/f:route", new CodeableConcept())]),
                    new LineBreak(),
                ]) 
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Site) 
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new DisplayLabel(LabelCodes.BodySite)]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular,[new Optional("f:dosage/f:site",  new CodeableConcept())]),
                    new LineBreak(),
                ]) 
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Method) 
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new ConstantText("Způsob podání")]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular,[new Optional("f:dosage/f:method",  new CodeableConcept())]),
                    new LineBreak(),
                ]) 
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Dose) 
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new ConstantText("Dávka léku")]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular,[new ConstantText(" "), new ShowQuantity("f:dosage/f:dose")]),
                    new LineBreak(),
                ]) 
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.RateRatio)
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new ConstantText("Dávkování")]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular,
                        [new ConstantText(" "), new ShowRatio("f:dosage/f:rateRatio")]),
                    new LineBreak()
                ])
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.RateQuantity)
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new ConstantText("Dávkování")]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular,
                        [new ConstantText(" "), new ShowQuantity("f:dosage/f:rateQuantity")]),
                    new LineBreak()
                ])
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Text) 
                ? new TextContainer(TextStyle.Regular, [
                    infrequentOptions.Count > 1 ? new LineBreak() : new NullWidget(),
                    new TextContainer(TextStyle.Bold, [new ConstantText("Pozn.")]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular,[new Optional("f:dosage/f:text", new Text("@value"))]),
                    new LineBreak(),
                ]) 
                : new NullWidget(),
        ], idSource: dosageNav);
        
        if (infrequentOptions.Count == 0)
        {
            dosageInstructionsTableCell = new TableCell([ new TextContainer(TextStyle.Muted, [new ConstantText("Informace nejsou k dispozici")])]);
        }
        
        return dosageInstructionsTableCell.Render(item, renderer, context);
    }
    
    private enum InfrequentPropertiesPaths
    {
        Route,
        Text,
        Site,
        Method,
        Dose,
        RateRatio,
        RateQuantity
    }
}
