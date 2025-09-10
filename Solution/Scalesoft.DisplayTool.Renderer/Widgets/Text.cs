using System.Web;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

public class Text(
    string path = ".",
    Func<XmlDocumentNavigator, RenderResult>? valueParser = null,
    string? optionalClass = null
)
    : ParsingWidget(path)
{
    private readonly Func<XmlDocumentNavigator, RenderResult> m_parseValue = valueParser ?? (x =>
    {
        var value = x.Node?.Value;
        return value != null
            ? value
            : new ParseError
            {
                Kind = ErrorKind.MissingValue,
                Severity = ErrorSeverity.Warning,
                Path = x.GetFullPath(),
            };
    });

    public override async Task<RenderResult> Render(
        XmlDocumentNavigator data,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        try
        {
            var element = data.SelectSingleNode(Path);
            if (context.RenderMode == RenderMode.Documentation)
            {
                return element.GetFullPath();
            }

            var parseResult = m_parseValue(element);
            if (!parseResult.HasValue)
            {
                return parseResult.Errors;
            }

            var viewModel = new ConstantText.ViewModel
            {
                Text = parseResult.Content,
                CustomClass = optionalClass,
            };

            var view = await renderer.RenderConstantText(viewModel);

            return new RenderResult(view, []);
        }
        catch
        {
            return HttpUtility.HtmlEncode(data.Evaluate(Path));
        }
    }
}