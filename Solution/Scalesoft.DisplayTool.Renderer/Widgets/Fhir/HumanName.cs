using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class HumanName(
    string namePath,
    bool throwErrorWhenInvalid = false,
    Func<Widget, Widget>? nameWrapper = null,
    bool unformattedName = false,
    bool hideNominalLetters = false
) : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        if (throwErrorWhenInvalid && (navigator.SelectSingleNode($"{namePath}").Node == null ||
                                      (navigator.SelectSingleNode($"{namePath}/f:family").Node == null &&
                                       navigator.SelectSingleNode($"{namePath}/f:given").Node == null &&
                                       navigator.SelectSingleNode($"{namePath}/f:text").Node == null)))
        {
            return Task.FromResult(new RenderResult("Chybí jméno pacienta.",
            [
                new ParseError
                {
                    Kind = ErrorKind.MissingValue,
                    Message =
                        "Patient must have their given name, family name or the text representation of their name defined.",
                    Path = navigator.GetFullPath(),
                    Severity = ErrorSeverity.Warning,
                },
            ]));
        }

        var itemNavigator = navigator.SelectSingleNode(namePath);

        var tree = new List<Widget>
        {
            new Choose([
                new When(
                    "(f:given and f:family) or (not(f:text) and (f:family or f:given))",
                    new If(_ => !unformattedName, new Row([
                        new If(_ => !hideNominalLetters && navigator.EvaluateCondition("f:prefix"),
                            new Container([
                                new PlainBadge(new ConstantText("Titul")),
                                CreateNamePartWidgetWithWrapper("f:prefix/@value", true)
                            ])
                        ),
                        new Container([
                            new PlainBadge(new DisplayLabel(LabelCodes.FamilyName)),
                            CreateNamePartWidgetWithWrapper("f:family/@value", false)
                        ]),
                        new Container([
                            new PlainBadge(new DisplayLabel(LabelCodes.GivenName)),
                            CreateNamePartWidgetWithWrapper("f:given/@value", true)
                        ]),
                        new If(_ => !hideNominalLetters && navigator.EvaluateCondition("f:suffix"),
                            new Container([
                                new PlainBadge(new ConstantText("Titul")),
                                CreateNamePartWidgetWithWrapper("f:suffix/@value", true)
                            ])
                        ),
                    ])).Else(
                        new TextContainer(TextStyle.Bold, [
                            new ConcatBuilder("f:prefix/@value", _ =>
                            [
                                new Text()
                            ], " "),
                            new ConstantText(" "),
                            new ConcatBuilder("f:given/@value", _ =>
                            [
                                new Text()
                            ], " "),
                            new ConstantText(" "),
                            new ConcatBuilder("f:family/@value", _ =>
                            [
                                new Text()
                            ], " "),
                            new ConstantText(" "),
                            new ConcatBuilder("f:suffix/@value", _ =>
                            [
                                new Text()
                            ], " "),
                        ])
                    )),
            ], new If(_ => unformattedName,
                    new Text("f:text/@value"))
                .Else(
                    new Container([
                        new PlainBadge(new ConstantText("Celé jméno")),
                        CreateNamePartWidgetWithWrapper("f:text/@value", false)
                    ])
                ))
        };

        return tree.RenderConcatenatedResult(itemNavigator, renderer, context);
    }

    private Widget CreateNamePartWidgetWithWrapper(string path, bool allowsMultiple)
    {
        var name = new If(_ => allowsMultiple,
            new ConcatBuilder(path, _ =>
            [
                new Text()
            ], " ")
        ).Else(new Text(path));

        return
            nameWrapper != null
                ? nameWrapper(
                    name
                )
                : new Container(name, ContainerType.Paragraph);
    }
}