using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class Address(string addressPath = ".", bool showLabel = true) : Widget
{
    public const string AddressPointExtensionUrl =
        "https://hl7.cz/fhir/core/StructureDefinition/address-point-cz";

    public const string StreetNameExtensionUrl =
        "http://hl7.org/fhir/StructureDefinition/iso21090-ADXP-streetName";

    public const string HouseNumberExtensionUrl =
        "http://hl7.org/fhir/StructureDefinition/iso21090-ADXP-houseNumber";

    public const string PostBoxExtensionUrl =
        "http://hl7.org/fhir/StructureDefinition/iso21090-ADXP-postBox";

    private const string CountryCodeExtensionUrl =
        "http://hl7.org/fhir/StructureDefinition/iso21090-SC-coding";

    private const string PermanentResidenceExtensionUrl =
        "https://hl7.cz/fhir/core/StructureDefinition/permanent-residencer-address-cz";

    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var addresses = new ConcatBuilder(addressPath, (_, _, nav) =>
        {
            var tree =
                new Concat([
                    new If(_ => showLabel, new Concat([
                        new Choose([
                            new When($"f:extension[@url='{PermanentResidenceExtensionUrl}']",
                                new TextContainer(TextStyle.Bold, [
                                    new ConstantText("Trvalé bydliště"),
                                    new Optional(
                                        $"f:extension[@url='{PermanentResidenceExtensionUrl}']/f:valueCodeableConcept",
                                        new ConstantText(" ("),
                                        new CodeableConcept(),
                                        new ConstantText(")")
                                    ),
                                    new ConstantText(":"),
                                ]),
                                new LineBreak()
                            )
                        ], new TextContainer(TextStyle.Bold, new ConstantText("Adresa: ")), new LineBreak())
                    ])),
                    new Optional($"f:extension[@url='{AddressPointExtensionUrl}']",
                        new TextContainer(TextStyle.Small | TextStyle.Bold, new ConstantText("RÚIAN: ")),
                        new TextContainer(TextStyle.Small, new ShowIdentifier("f:valueIdentifier")),
                        new LineBreak()
                    ),
                    new Choose([
                            new When("f:text", new Text("f:text/@value")),
                        ],
                        new If(_ => !nav.EvaluateCondition("f:line/@value"),
                            new ConcatBuilder("f:line", _ =>
                            [
                                new Concat([
                                    new ConcatBuilder($"f:extension[@url='{StreetNameExtensionUrl}']", _ =>
                                            [new Text("f:valueString/@value")], " "
                                    ),
                                    new ConcatBuilder($"f:extension[@url='{HouseNumberExtensionUrl}']", _ =>
                                            [new Text("f:valueString/@value")], " "
                                    ),
                                    new ConcatBuilder($"f:extension[@url='{PostBoxExtensionUrl}']", _ =>
                                            [new Text("f:valueString/@value")], " "
                                    )
                                ])
                            ], new LineBreak()),
                            new Condition("f:line",
                                new LineBreak()
                            )
                        ).Else(
                            new ConcatBuilder("f:line/@value", _ =>
                            [
                                new Text()
                            ], new LineBreak()),
                            new LineBreak()
                        ),

                        // Only add space before postalCode if city exists and postalCode exists
                        new Condition("f:city and f:postalCode",
                            new ConstantText(" ")
                        ),
                        new Optional("f:city", new Text("@value")),
                        new Condition("f:city and f:postalCode",
                            new ConstantText(" ")
                        ),
                        new Optional("f:postalCode", new Text("@value")),

                        // Only add comma and space before country if (city OR postalCode) AND country exist
                        new Condition(
                            "(f:city or f:postalCode) and f:country",
                            new ConstantText(", ")
                        ),
                        new Optional("f:country",
                            new Choose([
                                new When($"f:extension[@url='{CountryCodeExtensionUrl}']",
                                    new ChangeContext($"f:extension[@url='{CountryCodeExtensionUrl}']/f:valueCoding",
                                        new Coding())
                                ),
                            ], new Text("@value"))
                        )
                    ),
                ]);

            return [tree];
        }, new Concat([new LineBreak(), new LineBreak()]));

        return addresses.Render(navigator, renderer, context);
    }
}