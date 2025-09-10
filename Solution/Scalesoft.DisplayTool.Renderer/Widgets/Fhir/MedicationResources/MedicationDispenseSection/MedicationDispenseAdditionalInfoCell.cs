using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.MedicationResources.MedicationDispenseSection;

public class MedicationDispenseAdditionalInfoCell(XmlDocumentNavigator item) : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var infrequentOptions = InfrequentProperties.Evaluate<InfrequentPropertiesPaths>([item]);

        var supportingInfoTableCell = new TableCell(
        [
            new HideableDetails(
                infrequentOptions.Contains(InfrequentPropertiesPaths.Identifier)
                    ? new TextContainer(TextStyle.Regular, [
                        new TextContainer(TextStyle.Bold, [new ConstantText("Identifikátor podáni")]),
                        new ConstantText(": "),
                        new TextContainer(TextStyle.Regular,
                            [new CommaSeparatedBuilder("f:identifier", _ => [new ShowIdentifier()])]),
                        new LineBreak(),
                    ])
                    : infrequentOptions.Contains(InfrequentPropertiesPaths.Id)
                        ? new TextContainer(TextStyle.Regular, [
                            new TextContainer(TextStyle.Bold, [new ConstantText("Technický identifikátor podani")]),
                            new ConstantText(": "),
                            new TextContainer(TextStyle.Regular, [new Optional("f:id", new Text("@value"))]),
                            new LineBreak(),
                        ])
                        : new ConstantText("Identifikátor podání není specifikován")
            ),
            infrequentOptions.Contains(InfrequentPropertiesPaths.StatusReasonCodeableConcept)
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new ConstantText("Důvod stavu")]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular,
                        [new CommaSeparatedBuilder("f:statusReasonCodeableConcept", _ => [new CodeableConcept()])]),
                    new LineBreak(),
                ])
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.StatusReasonReference)
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new ConstantText("Důvod stavu dle reference")]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular,
                        [new CommaSeparatedBuilder("f:statusReasonReference", _ => [new AnyReferenceNamingWidget()])]),
                    new LineBreak(),
                ])
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Category)
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new ConstantText("Kategorie")]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular, [new Optional("f:category", new CodeableConcept())]),
                    new LineBreak(),
                ])
                : new ConstantText(""),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Type)
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new ConstantText("Typ")]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular, [new Optional("f:type", new CodeableConcept())]),
                    new LineBreak(),
                ])
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.PartOf)
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new ConstantText("Související úkony")]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular,
                        [new CommaSeparatedBuilder("f:partOf", _ => [new AnyReferenceNamingWidget()])]),
                    new LineBreak(),
                ])
                : new NullWidget(),
            infrequentOptions.Contains(InfrequentPropertiesPaths.Note)
                ? new TextContainer(TextStyle.Regular, [
                    new TextContainer(TextStyle.Bold, [new ConstantText("Poznámka")]),
                    new ConstantText(": "),
                    new TextContainer(TextStyle.Regular,
                        [new CommaSeparatedBuilder("f:note", _ => [new Optional("f:text", new Text("@value"))])]),
                    new LineBreak(),
                ])
                : new NullWidget()
        ]);

        if (infrequentOptions.Count == 0)
        {
            supportingInfoTableCell = new TableCell([
                new TextContainer(TextStyle.Muted, [new ConstantText("Informace nejsou k dispozici")]),
            ]);
        }

        return supportingInfoTableCell.Render(item, renderer, context);
    }

    private enum InfrequentPropertiesPaths
    {
        Id,
        Identifier,
        Category,
        Type,
        PartOf,
        Note,
        StatusReasonCodeableConcept,
        StatusReasonReference
    }
}