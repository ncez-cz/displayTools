using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils;
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
        var encounterNavigator = ReferenceHandler.GetSingleNodeNavigatorFromReference(navigator, "f:encounter",
            ".");
        XmlDocumentNavigator? logoNavigator = null;
        if (encounterNavigator?.Node != null)
        {
            logoNavigator = ReferenceHandler.GetSingleNodeNavigatorFromReference(encounterNavigator,
                "f:serviceProvider",
                "f:extension[@url='https://hl7.cz/fhir/core/StructureDefinition/cz-organization-logo']");
        }

        var containsLogo = logoNavigator?.Node != null;

        var qrCode =
            new Optional("f:identifier/f:value/@value",
                new Container([
                    new Barcode(new Text(), margin: 0, optionalInnerClass: "header-code",
                        optionalOuterClass: "d-flex align-content-center justify-content-center")
                ], optionalClass: "header-code-container align-content-center")
            );

        Widget[] widget =
        [
            new If(_ => containsLogo,
                new Row([
                    new ChangeContext(logoNavigator!, new HospitalLogo()),
                    qrCode,
                ], flexContainerClasses: "justify-content-between")
            ),
            new Row([
                new FlexList([
                        new Container([
                            new Optional("f:identifier",
                                new NameValuePair(new ConstantText("Identifikátor dokumentu"), new ShowIdentifier())),
                            new NameValuePair(new DisplayLabel(LabelCodes.LastUpdate), new ShowDateTime("f:date")),
                            new Optional(
                                "f:event[f:code/f:coding/f:system[@value='http://terminology.hl7.org/CodeSystem/v3-ActClass'] and f:code/f:coding/f:code[@value='PCPR'] and f:period]",
                                new NameValuePair(new ConstantText("Období zahrnuté v dokumentu"),
                                    new ShowPeriod("f:period"),
                                    new IdentifierSource())),
                            new NameValuePair(new ConstantText("Stav dokumentu"),
                                new EnumLabel("f:status", "http://hl7.org/fhir/ValueSet/composition-status")),
                            new NameValuePair(
                                new ConstantText("Kategorie"),
                                new ChangeContext("f:type", new CodeableConcept())
                            ),
                        ], ContainerType.Div, "two-col-grid " + (!containsLogo ? "pe-2" : string.Empty)),
                    ], FlexDirection.Column,
                    flexContainerClasses: "justify-content-start " + (containsLogo ? "w-100" : "flex-grow-1")),
                new If(_ => !containsLogo,
                    qrCode
                ),
            ], flexContainerClasses: "justify-content-between document-header"),
            new Heading([new Text("f:title/@value")], customClass: "mt-3 mb-auto uppercase"),
            new ThematicBreak(),
        ];

        return widget.RenderConcatenatedResult(navigator, renderer, context);
    }
}