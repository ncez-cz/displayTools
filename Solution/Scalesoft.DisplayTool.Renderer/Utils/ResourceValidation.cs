using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Utils;

public static class ResourceValidation
{
    /// <summary>
    /// Evaluates if a node has expired or is marked as obsolete based on date and use type
    /// </summary>
    /// <param name="navigator">The XML document navigator</param>
    /// <param name="endPath">The path to the end date node</param>
    /// <param name="usePath">The path to the use type node</param>
    /// <param name="obsoleteUseValue">Value that indicates obsolete usage</param>
    /// <returns>True if the node is current/valid, false if expired/obsolete</returns>
    /// <exception cref="InvalidOperationException">Thrown when both endPath and usePath are null</exception>
    /// <exception cref="FormatException">Thrown when the end date value is provided and cannot be parsed as a valid date</exception>
    public static bool IsNodeCurrent(
        XmlDocumentNavigator navigator,
        string? endPath = "f:period/f:end/@value",
        string? usePath = "f:use/@value",
        string obsoleteUseValue = UseTypes.Old)
    {
        // Check if marked as obsolete via use type
        if (usePath != null)
        {
            var useNode = navigator.SelectSingleNode(usePath).Node;
            if (useNode != null && useNode.Value == obsoleteUseValue)
            {
                return false;
            }
        }

        // Check end date -> if both endPath and usePath is null, the operation is invalid.
        if (endPath == null)
        {
            throw new InvalidOperationException("At least one of endPath or usePath must be provided.");
        }
        
        var endDateNode = navigator.SelectSingleNode(endPath).Node;
        
        // No end date means not expired
        if (endDateNode == null)
        {
            return true;
        }
        
        // Parse and validate end date
        if (DateTime.TryParse(endDateNode.Value, out var endDate))
        {
            return endDate >= DateTime.Today;
        }
        
        // Invalid date format
        throw new FormatException($"Invalid end date: {endDateNode.Value}.");
    }
}