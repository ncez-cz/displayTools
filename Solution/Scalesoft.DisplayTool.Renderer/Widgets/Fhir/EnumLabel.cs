using System.Xml.XPath;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class EnumLabel(string nodePath, string enumDefinitionUri) : Widget
{
    public override Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        if (IsDataAbsent(navigator, nodePath))
        {
            return new AbsentData(nodePath).Render(navigator, renderer, context);
        }

        if (context.RenderMode == RenderMode.Documentation)
        {
            return Task.FromResult<RenderResult>(navigator.SelectSingleNode(nodePath).GetFullPath());
        }

        var node = navigator.SelectSingleNode(nodePath);
        if (node.Node == null)
        {
            return Task.FromResult<RenderResult>(new ParseError
            {
                Kind = ErrorKind.MissingValue,
                Message = $"Could not find value for enum {enumDefinitionUri}",
                Path = node.GetFullPath(),
                Severity = ErrorSeverity.Warning,
            });
        }

        var enumValue = node.Node.NodeType == XPathNodeType.Attribute
            ? node.Node.Value
            : node.SelectSingleNode("@value").Node?.Value;

        if (enumValue == null)
        {
            return Task.FromResult<RenderResult>(new ParseError
            {
                Kind = ErrorKind.MissingValue,
                Message = $"Could not find value for enum {enumDefinitionUri}",
                Path = node.GetFullPath(),
                Severity = ErrorSeverity.Warning,
            });
        }

        var widget = new CodedValue(enumValue, enumDefinitionUri, fallbackValue: enumValue, isValueSet: true);
        return widget.Render(navigator, renderer, context);
    }
}