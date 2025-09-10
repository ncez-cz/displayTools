using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;
public class SingleReference(
    Func<XmlDocumentNavigator, IList<Widget>> contentBuilder,
    string elementName = "f:entry/f:reference",
    string? referencePrefix = null)
    : Reference(elementName)
{
    public override async Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        var referenceResult = GetReferences(navigator, referencePrefix);
        var errors = referenceResult.Errors;
        var itemNavigators = referenceResult.Navigators;
        
        if (itemNavigators.Count > 1)
        {
            errors.Add(new ParseError
            {
                Kind = ErrorKind.TooManyValues,
                Path = navigator.GetFullPath(),
                Message = $"Multiple values found for {ElementName} when only one is allowed",
                Severity = ErrorSeverity.Warning,
            });
        }
        var itemNavigator = itemNavigators.FirstOrDefault();
        if (itemNavigator?.Node == null)
        {
            errors.Add(new ParseError
            {
                Kind = ErrorKind.InvalidValue,
                Path = navigator.GetFullPath(),
                Message = $"No valid reference found for {ElementName}",
                Severity = ErrorSeverity.Warning,
            });
            return errors;
        }

        if (errors.MaxSeverity() >= ErrorSeverity.Fatal)
        {
            return errors;
        }

        var contentWidget = contentBuilder(itemNavigator);
        return await contentWidget.RenderConcatenatedResult(itemNavigator, renderer, context);
    }
}
