using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public abstract class Reference : Widget
{
    protected readonly string ElementName;

    protected Reference(string elementName = "f:entry/f:reference")
    {
        ElementName = elementName;
    }

    protected ReferenceResult GetReferences(XmlDocumentNavigator navigator, string? referencePrefix = null)
    {
        var itemIds = navigator
            .SelectAllNodes($"{ElementName}/@value")
            .Select(x => x.Node?.Value)
            .Where(value => value != null && ((referencePrefix == null || value.Split("/").First() == referencePrefix) || value.StartsWith("urn:")))
            .OfType<string>()
            .ToList();

        var result = new ReferenceResult();

        foreach (var itemId in itemIds)
        {
            if (!ReferenceHandler.TryBuildReferenceXpath(itemId, out var xpath))
            {
                result.Errors.Add(new ParseError
                {
                    Kind = ErrorKind.InvalidValue,
                    Path = navigator.GetFullPath(),
                    Message = $"Invalid {ElementName} id: {itemId}",
                    Severity = ErrorSeverity.Warning,
                });

                return result;
            }
            
            var itemNavigator =
                navigator.SelectSingleNode(xpath);
            if (itemNavigator.Node != null)
            {
                result.Navigators.Add(itemNavigator);
            }
        }

        return result;
    }
}
