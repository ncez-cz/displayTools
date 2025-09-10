using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class ShowTiming(string path = ".") : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        if (IsDataAbsent(navigator, path))
        {
            return new AbsentData(path).Render(navigator, renderer, context);
        }

        if (context.RenderMode == RenderMode.Documentation)
        {
            return Task.FromResult<RenderResult>(navigator.SelectSingleNode(path).GetFullPath());
        }

        var tree = new ChangeContext(path,
            new Container([
                new Optional("f:event",
                    new TextContainer(TextStyle.Bold, [new ConstantText("Datum: ")])),
                new CommaSeparatedBuilder("f:event", _ => [new ShowDateTime()]),

                new Optional("f:event",
                    new OptionalLineBreak(
                        [
                            "../f:repeat/f:boundsDuration",
                            "../f:repeat/f:boundsRange",
                            "../f:repeat/f:boundsPeriod",
                            "../f:repeat/f:frequency",
                            "../f:repeat/f:period",
                            "../f:repeat/f:timeOfDay",
                            "../f:repeat/f:dayOfWeek",
                            "../f:repeat/f:when"
                        ]
                    )
                ),

                new Optional("f:repeat/f:boundsDuration",
                    new NameValuePair(new ConstantText("Doba trvání: "), new ShowDuration()),
                    new OptionalLineBreak(
                        [
                            "../f:boundsRange",
                            "../f:boundsPeriod",
                            "../f:frequency",
                            "../f:period",
                            "../f:timeOfDay",
                            "../f:dayOfWeek",
                            "../f:when"
                        ]
                    )
                ),

                new Optional("f:repeat/f:boundsRange",
                    new ShowRange(),
                    new OptionalLineBreak(
                        [
                            "../f:boundsPeriod",
                            "../f:frequency",
                            "../f:period",
                            "../f:timeOfDay",
                            "../f:dayOfWeek",
                            "../f:when"
                        ]
                    )
                ),

                new Optional("f:repeat/f:boundsPeriod",
                    new ShowPeriod(),
                    new OptionalLineBreak(
                        [
                            "../f:frequency",
                            "../f:period",
                            "../f:timeOfDay",
                            "../f:dayOfWeek",
                            "../f:when"
                        ]
                    )
                ),

                new Optional("f:repeat/f:frequency",
                    new Text("@value"),
                    new ConstantText(" "),
                    new DisplayLabel(LabelCodes.Times)
                ),

                new Optional("f:repeat/f:period",
                    new ConstantText(" "),
                    new DisplayLabel(LabelCodes.Every),
                    new ConstantText(" "),
                    new Text("@value"),
                    new ConstantText(" ")
                ),
                new Optional("f:repeat/f:periodUnit",
                    new EnumLabel("@value", "http://hl7.org/fhir/ValueSet/units-of-time"),
                    new OptionalLineBreak(
                        [
                            "../f:timeOfDay",
                            "../f:dayOfWeek",
                            "../f:when"
                        ]
                    )
                ),

                new Optional("f:repeat/f:timeOfDay",
                    new TextContainer(TextStyle.Bold, [new ConstantText("Časy: ")])
                ),
                new CommaSeparatedBuilder("f:repeat/f:timeOfDay", _ => [new Text("@value")]),
                new Optional("f:repeat/f:timeOfDay",
                    new OptionalLineBreak(
                        [
                            "../f:dayOfWeek",
                            "../f:when"
                        ]
                    )
                ),

                new Optional("f:repeat/f:dayOfWeek",
                    new TextContainer(TextStyle.Bold, [new ConstantText("Dny v týdnu: ")])
                ),

                new CommaSeparatedBuilder("f:repeat/f:dayOfWeek", _ => [new Text("@value")]),
                new Optional("f:repeat/f:dayOfWeek",
                    new OptionalLineBreak(
                        [
                            "../f:when"
                        ]
                    )
                ),

                new Optional("f:repeat/f:when",
                    new TextContainer(TextStyle.Bold, [new ConstantText("Denní doby: ")])
                ),
                new CommaSeparatedBuilder("f:repeat/f:when", _ => [new Text("@value")])
            ], ContainerType.Span)
        );

        return tree.Render(navigator, renderer, context);
    }
}