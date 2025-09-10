using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.MedicationResources.MedicationAdministrationSection;

public class MedicationAdministrationMedicationCell(XmlDocumentNavigator item) : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        var infrequentOptions = InfrequentProperties.Evaluate<InfrequentPropertiesPaths>([item]);

        var medicationTableCell = new TableCell(
        [
            infrequentOptions.Contains(InfrequentPropertiesPaths.MedicationReference)
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new DisplayLabel(LabelCodes.Name)]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular,[new CommaSeparatedBuilder("f:medicationReference", _ => [new AnyReferenceNamingWidget(".")])]),
                    new LineBreak(),
                ]) : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.MedicationCodeableConcept)
                    ? new TextContainer(TextStyle.Regular, [
                        new TextContainer(TextStyle.Bold, [new DisplayLabel(LabelCodes.Name)]),
                        new ConstantText(": "),
                        new TextContainer(TextStyle.Regular,[new CommaSeparatedBuilder("f:medicationCodeableConcept", _ => [new CodeableConcept()])]),
                        new LineBreak(),
                    ]) 
                    : new NullWidget(),
            new TextContainer(TextStyle.Regular, [
                new TextContainer(TextStyle.Bold, [new ConstantText("Datum podání")]),
                new ConstantText(": "),
                new TextContainer(TextStyle.Regular, [ new Chronometry("effective")]),
                new LineBreak(),
            ]),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Request)
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new ConstantText("Na základě žádosti")]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular, ReferenceHandler.BuildAnyReferencesNaming(item,"f:request", context, renderer)),
                    new LineBreak(),
                ])
                : new NullWidget(),
        ]);

        return medicationTableCell.Render(item, renderer, context);
    }

    private enum InfrequentPropertiesPaths
    {
        MedicationCodeableConcept,
        MedicationReference,
        Request,
    }
}