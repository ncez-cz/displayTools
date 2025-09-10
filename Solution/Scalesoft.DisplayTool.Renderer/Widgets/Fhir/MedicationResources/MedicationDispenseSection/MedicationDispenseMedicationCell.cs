using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.MedicationResources.MedicationDispenseSection;

public class MedicationDispenseMedicationCell(XmlDocumentNavigator item) : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        var infrequentOptions = InfrequentProperties.Evaluate<InfrequentPropertiesPaths>([item]);

        var quantityValue = new Optional("f:quantity/f:value", new Text("@value"));
        var quantityUnit = new Optional("f:quantity/f:code", new Text("@value"));
        var daysSupplyValue = new Optional("f:daysSupply/f:value", new Text("@value"));
        var daysSupplyUnit = new Optional("f:daysSupply/f:unit", new Text("@value"));

        var medicationTableCell = new TableCell(
        [
            new TextContainer(TextStyle.Regular, [
                infrequentOptions.Contains(InfrequentPropertiesPaths.MedicationReference)
                    ? new TextContainer(TextStyle.Regular, [
                        new TextContainer(TextStyle.Bold, [new DisplayLabel(LabelCodes.Name)]),
                        new ConstantText(": "),
                        new TextContainer(TextStyle.Regular,[new CommaSeparatedBuilder("f:medicationReference", _ => [new AnyReferenceNamingWidget(".")])]),
                        new LineBreak(),
                    ]) 
                    : new NullWidget(),
                infrequentOptions.Contains(InfrequentPropertiesPaths.MedicationCodeableConcept)
                    ? new TextContainer(TextStyle.Regular, [
                        new TextContainer(TextStyle.Bold, [new DisplayLabel(LabelCodes.Name)]),
                        new ConstantText(": "),
                        new TextContainer(TextStyle.Regular,
                        [
                            new CommaSeparatedBuilder("f:medicationCodeableConcept", _ => [new CodeableConcept()])
                        ]),
                        new LineBreak(),
                    ])
                    : new NullWidget(),
                infrequentOptions.Contains(InfrequentPropertiesPaths.Quantity)
                    ? new TextContainer(TextStyle.Regular, [
                        new TextContainer(TextStyle.Bold, [new ConstantText("Množství")]),
                        new ConstantText(": "),
                        new TextContainer(TextStyle.Regular, [quantityValue, new ConstantText(" "), quantityUnit], idSource: item.SelectSingleNode("f:quantity")),
                        new LineBreak(),
                    ])
                    : new NullWidget(),
                infrequentOptions.Contains(InfrequentPropertiesPaths.DaysSupply)
                    ? new TextContainer(TextStyle.Regular, [
                        new TextContainer(TextStyle.Bold, [new ConstantText("Zásoba na")]),
                        new ConstantText(": "),
                        new TextContainer(TextStyle.Regular,
                            [daysSupplyValue, new ConstantText(" ("), daysSupplyUnit, new ConstantText(")")], idSource: item.SelectSingleNode("f:daysSupply")),
                        new LineBreak(),
                    ])
                    : new NullWidget(),
                infrequentOptions.Contains(InfrequentPropertiesPaths.WhenPrepared)
                    ? new TextContainer(TextStyle.Regular, [
                        new TextContainer(TextStyle.Bold, [new ConstantText("Datum přípravy")]),
                        new ConstantText(": "),
                        new TextContainer(TextStyle.Regular, [new Optional("f:whenPrepared", new ShowDateTime())]),
                        new LineBreak(),
                    ])
                    : new NullWidget(),
                infrequentOptions.Contains(InfrequentPropertiesPaths.WhenHandedOver)
                    ? new TextContainer(TextStyle.Regular, [
                        new TextContainer(TextStyle.Bold, [new ConstantText("Datum předáni")]),
                        new ConstantText(": "),
                        new TextContainer(TextStyle.Regular, [new Optional("f:whenHandedOver", new ShowDateTime())]),
                        new LineBreak(),
                    ])
                    : new NullWidget(),
            ])
        ]);

        return medicationTableCell.Render(item, renderer, context);
    }

    private enum InfrequentPropertiesPaths
    {
        Quantity,
        DaysSupply,
        WhenPrepared,
        WhenHandedOver,
        MedicationCodeableConcept,
        MedicationReference
    }
}