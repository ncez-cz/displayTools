using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir.PatientSection;

public class PatientDetails : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var resourceConfiguration = new ResourceConfiguration();
        var configurations = resourceConfiguration.ProcessConfigurations(navigator).Results;
        var name = configurations.First(r => r.Name == ResourceNames.Name).FormattedPath;

        const string ridIdentifier = "https://ncez.mzcr.cz/fhir/sid/rid";
        const string insuranceCompanyCodeIdentifier = "https://ncez.mzcr.cz/fhir/sid/kp";
        const string clinicalGender = "http://hl7.org/fhir/StructureDefinition/patient-sexParameterForClinicalUse";
        const string birthPlace = "http://hl7.org/fhir/StructureDefinition/patient-birthPlace";
        const string nationality = "http://hl7.org/fhir/StructureDefinition/patient-nationality";
        const string registeringProvider = "https://hl7.cz/fhir/core/StructureDefinition/registering-provider-cz";
        const string patientAnimal = "http://hl7.org/fhir/StructureDefinition/patient-animal";
        const string recordedSexOrGender = "http://hl7.org/fhir/StructureDefinition/individual-recordedSexOrGender";

        var isAnimal = navigator.EvaluateCondition($"f:extension[@url='{patientAnimal}']");

        var tree = new List<Widget>
        {
            new Container([
                new Container([
                    new Row([
                        //name
                        new ChangeContext(name,
                            new HumanName(".", true, nameWrapper: x => new Heading([x], HeadingSize.H3),
                                hideNominalLetters: isAnimal)
                        ),
                        new Optional($"f:extension[@url='{patientAnimal}']",
                            new AnimalDetails()
                        ),
                        new Optional("f:birthDate",
                            new Container([
                                new PlainBadge(new DisplayLabel(LabelCodes.DateOfBirth)),
                                new Heading([new ShowDateTime()], HeadingSize.H3),
                            ])
                        ),
                        //Gender
                        new Container([
                            new If(_ => navigator.EvaluateCondition("f:gender"),
                                    new PlainBadge(new DisplayLabel(LabelCodes.AdministrativeGender)), new LineBreak(),
                                    new Heading(
                                        [
                                            new EnumLabel("f:gender",
                                                "https://hl7.cz/fhir/ValueSet/administrative-gender-cz")
                                        ],
                                        HeadingSize.H3))
                                .Else(
                                    new Optional($"f:extension[@url='{recordedSexOrGender}']",
                                        new PlainBadge(new DisplayLabel(LabelCodes.AdministrativeGender)),
                                        new LineBreak(),
                                        new Optional("f:extension[@url='value']/f:valueCodeableConcept",
                                            new Heading([new CodeableConcept()], HeadingSize.H3)
                                        ),
                                        new Optional("f:extension[@url='type']/f:valueCodeableConcept",
                                            new NameValuePair(
                                                new ConstantText("Typ"),
                                                new CodeableConcept()
                                            )
                                        )
                                    )
                                ),
                        ]),
                    ], flexContainerClasses: "column-gap-4"),
                    new Row([
                        //Clinical Gender
                        new If(_ => navigator.EvaluateCondition($"f:extension[@url='{clinicalGender}']"),
                            new Container([
                                new PlainBadge(new ConstantText("Pohlaví pro klinické použití")),
                                new LineBreak(),
                                new ConcatBuilder($"f:extension[@url='{clinicalGender}']", _ =>
                                [
                                    new Concat([
                                        new TextContainer(TextStyle.Bold,
                                        [
                                            new Optional("f:extension[@url='value']", new OpenTypeElement(null))
                                        ]), // CodeableConcept
                                        new LineBreak(),
                                        new NameValuePair(
                                            [new ConstantText("Období")],
                                            [
                                                new Optional("f:extension[@url='period']", new OpenTypeElement(null))
                                            ] // Period
                                        ),
                                        new Container([
                                            new TextContainer(TextStyle.Bold, [
                                                new ConstantText("Komentář: ")
                                            ]),
                                            new Optional("f:extension[@url='comment']",
                                                new OpenTypeElement(null)), // string
                                        ]),
                                    ], string.Empty),
                                ], new LineBreak()),
                            ])),
                        new Container([
                            //Contacts
                            new ContactInformation(),
                        ]),
                        new Optional(
                            $"f:extension[@url='{patientAnimal}']/f:extension[@url='genderStatus']/f:valueCodeableConcept",
                            new Container([
                                new Concat([
                                    new PlainBadge(new ConstantText("Stav pohlaví")),
                                    new TextContainer(TextStyle.Bold, new CodeableConcept())
                                ], new LineBreak())
                            ])
                        ),
                    ], flexContainerClasses: "column-gap-8"),
                    new Row([
                        //Birth Place
                        new If(_ => navigator.EvaluateCondition($"f:extension[@url='{birthPlace}']"), new Container([
                            new PlainBadge(new ConstantText("Místo narození")), new LineBreak(), new ChangeContext(
                                $"f:extension[@url='{birthPlace}']",
                                new OpenTypeElement(null,
                                    hints: OpenTypeElementRenderingHints.HideAddressLabel)) // CZ_Address
                        ])),
                        // Nationality
                        new If(_ => navigator.EvaluateCondition($"f:extension[@url='{nationality}']"), new Container([
                                new PlainBadge(new ConstantText("Národnost")), new LineBreak(), new ConcatBuilder(
                                    $"f:extension[@url='{nationality}']", _ =>
                                    [
                                        new Concat([
                                            new Optional("f:extension[@url='code']",
                                                new OpenTypeElement(null)), // CodeableConcept
                                            new If(_ => navigator.EvaluateCondition("f:extension[@url='period']"),
                                                new ConstantText("-"),
                                                new Optional("f:extension[@url='period']",
                                                    new OpenTypeElement(null))), // Period
                                        ]),
                                    ])
                            ]
                        )),
                    ], flexContainerClasses: "column-gap-8"),
                    new Row([
                        new If(_ => navigator.EvaluateCondition($"f:extension[@url='{registeringProvider}']"),
                            new Container([
                                new PlainBadge(new ConstantText("Registrující poskytovatel")),
                                new LineBreak(),
                                new ConcatBuilder($"f:extension[@url='{registeringProvider}']", _ =>
                                    [
                                        new Concat([
                                            new Optional("f:extension[@url='category']",
                                                new OpenTypeElement(null)), // CodeableConcept
                                            new ConstantText("-"),
                                            new Optional("f:extension[@url='value']",
                                                new OpenTypeElement(
                                                    null)), // Reference(Organization | Practitioner Role)
                                        ]),
                                    ],
                                    new LineBreak()
                                )
                            ])
                        ),
                    ]),
                ], optionalClass: ""),
                new Container([
                    new If(_ => navigator.EvaluateCondition($"f:identifier[f:system/@value='{ridIdentifier}']"),
                        new ChangeContext($"f:identifier[f:system/@value='{ridIdentifier}']",
                            new PlainBadge(new ConstantText("Resortní Identifikátor")),
                            new LineBreak(),
                            new Heading([
                                new ShowIdentifier(showSystem: false)
                            ], HeadingSize.H3)
                        )
                    ).Else(
                        new ChangeContext("f:identifier",
                            new PlainBadge(new ConstantText("Identifikátor pacienta")),
                            new LineBreak(),
                            new Heading([new ShowIdentifier(showSystem: false)], HeadingSize.H3)
                        )
                    ),
                    //Identifiers
                    new If(_ => navigator.EvaluateCondition("f:identifier[f:use/@value='official']"),
                        new PlainBadge(new DisplayLabel(LabelCodes.PrimaryPatientIdentifier))),
                    new ConcatBuilder(
                        $"f:identifier[f:use/@value='official' and not(f:system/@value='{ridIdentifier}')]",
                        (_, _, nav) =>
                        {
                            var showSystem = nav.EvaluateCondition(
                                "f:type/f:coding[f:system/@value='http://terminology.hl7.org/CodeSystem/v2-0203' and f:code/@value='PPN']");

                            return HandleIdentifierDisplay(nav, showSystem, insuranceCompanyCodeIdentifier);
                        }),
                    new If(
                        _ => navigator.EvaluateCondition(
                            $"f:identifier[not(f:use/@value='official') and not(f:system/@value='{ridIdentifier}')]"),
                        new PlainBadge(new DisplayLabel(LabelCodes.SecondaryPatientIdentifier))),
                    new ConcatBuilder(
                        $"f:identifier[not(f:use/@value='official') and not(f:system/@value='{ridIdentifier}')]",
                        (_, _, nav) => HandleIdentifierDisplay(nav, true, insuranceCompanyCodeIdentifier)),
                ], optionalClass: "ms-auto"),
            ], optionalClass: "d-flex"),
        };

        return tree.RenderConcatenatedResult(navigator, renderer, context);
    }

    private Widget[] HandleIdentifierDisplay(
        XmlDocumentNavigator nav,
        bool showSystem,
        string insuranceCompanyCodeIdentifier,
        bool? isInsuranceCoIdOfficial = null
    )
    {
        if (nav.EvaluateCondition("f:system[@value='https://ncez.mzcr.cz/fhir/sid/cpoj']"))
            return
            [
                new Container([
                    new NameValuePair([new IdentifierSystemLabel(),], [new ShowIdentifier(showSystem: false),]),
                    new ShowSingleReference(issuerNav =>
                    {
                        if (!issuerNav.ResourceReferencePresent)
                        {
                            return [];
                        }

                        var indentifierUseCondition = string.Empty;
                        if (isInsuranceCoIdOfficial == true)
                        {
                            indentifierUseCondition = "and f:use/@value='official'";
                        }

                        if (isInsuranceCoIdOfficial == false)
                        {
                            indentifierUseCondition = "and not(f:use/@value='official')";
                        }

                        return
                        [
                            new ChangeContext(issuerNav.Navigator,
                                new Optional(
                                    $"f:identifier[f:system/@value='{insuranceCompanyCodeIdentifier}' {indentifierUseCondition}]",
                                    new NameValuePair([new IdentifierSystemLabel(),],
                                        [new ShowIdentifier(showSystem: false),])
                                )
                            )
                        ];
                    }, "f:assigner"),
                ]),
            ];

        return
        [
            new NameValuePair([
                    new IdentifierSystemLabel()
                ],
                [
                    new ShowIdentifier(showSystem: showSystem),
                ]
            ),
        ];
    }
}