using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public static class DeviceParsingInfo
{
    public static Widget[] CompactRenderingWidgets = [
        new Optional("f:manufacturer", [
            new TextContainer(TextStyle.Bold, [new ConstantText("Výrobce: ")]),
            new Text("@value"),
            new LineBreak(),
        ]),
        new Condition("f:deviceName", [
            new Container([
                new TextContainer(TextStyle.Bold, [new ConstantText("Název: ")]),
                new CommaSeparatedBuilder("f:deviceName", _ => [new Optional("f:name/@value", new Text())]),
                new LineBreak(),
            ], idSource: new IdentifierSource(true)),
        ]),
        new Optional("f:modelNumber", [
            new TextContainer(TextStyle.Bold, [new ConstantText("Číslo modelu: ")]),
            new Text("@value"),
            new LineBreak(),
        ]),
        new Optional("f:serialNumber", [
            new TextContainer(TextStyle.Bold, [new ConstantText("Sériové číslo: ")]),
            new Text("@value"),
            new LineBreak(),
        ]),
        new Condition("f:specialization", [
            new Container([
                new TextContainer(TextStyle.Bold, [new ConstantText("Specializace: ")]),
                new CommaSeparatedBuilder("f:specialization", _ => [new Optional("f:systemType", new CodeableConcept())]),
                new LineBreak(),
            ], idSource: new IdentifierSource(true)),
        ]),
        new Optional("f:expirationDate", [
            new TextContainer(TextStyle.Bold, [new ConstantText("Datum expirace: ")]),
            new ShowDateTime(),
            new LineBreak(),
        ]),
    ];
}
