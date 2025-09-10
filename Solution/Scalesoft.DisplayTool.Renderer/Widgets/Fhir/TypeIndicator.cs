using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class TypeIndicator(string xpath, Dictionary<string, SupportedIcons> iconMapping, string enumUri)
    : Widget
{
    public override async Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        if (context.RenderMode == RenderMode.Documentation)
        {
            return navigator.SelectSingleNode(xpath).GetFullPath();
        }

        var whens = new List<When>();

        foreach (var (value, icon) in iconMapping)
        {
            whens.Add(new When($"{xpath}/@value='{value}'",
                new Container([
                    new TextContainer(TextStyle.Bold, [
                        new Icon(icon),
                        new ConstantText(" "),
                        new EnumLabel($"{xpath}/@value", enumUri)
                    ])
                ], ContainerType.Span, "text-nowrap")
            ));
        }

        var typeIndicator = new Choose(
            whens,
            new EnumLabel($"{xpath}/@value", enumUri)
        );

        return await typeIndicator.Render(navigator, renderer, context);
    }
}