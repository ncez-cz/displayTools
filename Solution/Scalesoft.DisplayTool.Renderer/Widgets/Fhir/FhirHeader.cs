using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class FhirHeader : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        Widget[] widget =
        [
            new Row([
                new FlexList([
                    new Optional("f:identifier",
                        new NameValuePair(new ConstantText("Identifikátor dokumentu"), new ShowIdentifier())),
                    new NameValuePair(new DisplayLabel(LabelCodes.LastUpdate), new ShowDateTime("f:date")),
                    new Optional(
                        "f:event[f:code/f:coding/f:system[@value='http://terminology.hl7.org/CodeSystem/v3-ActClass'] and f:code/f:coding/f:code[@value='PCPR'] and f:period]",
                        new NameValuePair(new ConstantText("Období zahrnuté v dokumentu"), new ShowPeriod("f:period"),
                            new IdentifierSource())),
                    new NameValuePair(new ConstantText("Stav dokumentu"),
                        new EnumLabel("f:status", "http://hl7.org/fhir/ValueSet/composition-status")),
                    new NameValuePair(
                        new ConstantText("Kategorie"),
                        new ChangeContext("f:type", new CodeableConcept())
                    ),
                    new Heading([new Text("f:title/@value")], customClass: "mt-3 mb-auto uppercase"),
                ], FlexDirection.Column, flexContainerClasses: "justify-content-start"),
                new FlexList([
                    new Optional("f:identifier/f:value/@value",
                        new Container([
                            new Barcode(new Text(), margin: 0, optionalInnerClass: "header-code",
                                optionalOuterClass: "d-flex align-content-center justify-content-center"),
                            new TextContainer(TextStyle.Bold, [new ConstantText("QR identifikátor dokumentu")],
                                optionalClass: "header-code-label"),
                        ], optionalClass: "header-code-container")
                    ),
                    ShowSingleReference.WithDefaultDisplayHandler(_ => [new HospitalLogo()], "f:custodian")
                ], FlexDirection.Row, flexContainerClasses: "justify-content-end"),
            ], flexContainerClasses: "justify-content-between document-header"),
            new ThematicBreak()
        ];

        return widget.RenderConcatenatedResult(navigator, renderer, context);
    }
}