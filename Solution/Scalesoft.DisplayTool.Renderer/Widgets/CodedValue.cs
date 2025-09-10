using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

public class CodedValue(
    string? code,
    string? codeSystem,
    string? fallbackValue = null,
    bool displayCodeSystem = false,
    bool displayCodeSystemOnFallbackOnly = false,
    bool isValueSet = false
) : Widget
{
    public override async Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context)
    {
        var language = context.Language.Primary.Code;
        var fallbackLanguage = context.Language.Fallback.Code;

        if (code == null || codeSystem == null)
        {
            return fallbackValue ?? code ?? string.Empty;
        }

        var translated = await context.Translator.GetCodedValue(
            code,
            codeSystem,
            language,
            fallbackLanguage,
            isValueSet);
        var value = translated ?? fallbackValue ?? string.Empty;

        if (context.RenderMode == RenderMode.Documentation)
        {
            value = navigator.GetFullPath();
        }

        List<Widget> widgets =
        [
            new ConstantText(value),
        ];

        if (!displayCodeSystem)
        {
            return await widgets.RenderConcatenatedResult(navigator, renderer, context);
        }

        if (!displayCodeSystemOnFallbackOnly || (string.IsNullOrEmpty(translated) && !string.IsNullOrEmpty(fallbackValue)))
        {
            if (context.RenderMode != RenderMode.Documentation)
            {
                widgets.Add(new HideableDetails(ContainerType.Span, new ConstantText($" ({codeSystem})")));
            }
        }

        return await widgets.RenderConcatenatedResult(navigator, renderer, context);
    }
}