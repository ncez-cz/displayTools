using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class AlternatingBackgroundColumn(List<Widget> widgets) : Widget
{
    public override async Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        if (widgets.Count == 0)
        {
            return RenderResult.NullResult;
        }

        List<Widget> finalWidgets = [];


        for (var i = 0; i < widgets.Count; i++)
        {
            var widget = widgets[i];

            string? classToAdd = null;

            if (i % 2 == 0)
            {
                classToAdd += "striped-row-bg ";

                if (widget == widgets.Last())
                {
                    classToAdd += "pb-4";
                }
            }

            Widget widgetToAdd = new Container(widget, optionalClass: classToAdd);

            if (widget != widgets.Last())
            {
                widgetToAdd = new Concat([widgetToAdd, new ThematicBreak("mx-negative-2")]);
            }

            finalWidgets.Add(widgetToAdd);
        }

        return await finalWidgets.RenderConcatenatedResult(navigator, renderer, context);
    }
}