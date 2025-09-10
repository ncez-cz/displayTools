using Scalesoft.DisplayTool.Renderer.Constants;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Utils;

/// <summary>
///     Manages configuration rules for FHIR resource selection and processing.
/// </summary>
public class ResourceConfiguration
{
    /// <summary>
    ///     Gets the list of selection rules for different FHIR resource types.
    ///     Each rule contains configuration for a specific resource type.
    /// </summary>
    private List<ResourceSelectionRule> SelectionRules { get; }

    /// <summary>
    ///     Creates a new ResourceConfiguration instance with either custom or default rules.
    /// </summary>
    /// <param name="selectionRules">Optional custom selection rules. If null, default rules will be used.</param>
    public ResourceConfiguration(List<ResourceSelectionRule>? selectionRules = null)
    {
        SelectionRules = selectionRules ?? GetDefaultRules();
    }

    /// <summary>
    ///     Creates the default set of resource selection rules.
    /// </summary>
    /// <returns>A list of default resource selection rules.</returns>
    private static List<ResourceSelectionRule> GetDefaultRules()
    {
        return
        [
            new ResourceSelectionRule(ResourceNames.Name, "f:name", UseTypePriorities.NameTypes),
            new ResourceSelectionRule(ResourceNames.Address, "f:address", UseTypePriorities.AddressTypes),
            new ResourceSelectionRule(ResourceNames.Identifier, "f:identifier", UseTypePriorities.IdentifierTypes)
        ];
    }

    /// <summary>
    ///     Processes the configuration rules using the provided XML navigator.
    ///     For each selection rule, retrieves matching nodes from the XML document
    ///     and selects the preferred resource based on configured type priorities.
    /// </summary>
    /// <param name="navigator">The XML document navigator used to select and process nodes.</param>
    /// <returns>A ParseResult containing the processed resource selection rules and any errors encountered.</returns>
    public ParseResult<ResourceSelectionRule> ProcessConfigurations(XmlDocumentNavigator navigator)
    {
        List<ParseError> errors = [];
        List<ResourceSelectionRule> processedRules = [];

        // Process non-nested configurations first
        foreach (var config in SelectionRules.Where(rule => !rule.IsNested))
        {
            config.Navigators = navigator.SelectAllNodes(config.Path).ToList();
            try
            {
                config.SelectedIndex = ResourceSelector.SelectPreferredResourceIndex(config.Navigators, config.Types);
            }
            catch (FormatException e)
            {
                config.SelectedIndex = 1;
                errors.Add(new ParseError
                {
                    Kind = ErrorKind.InvalidValue,
                    Message = e.Message,
                    Path = navigator.GetFullPath(),
                    Severity = ErrorSeverity.Warning,
                });
            }

            processedRules.Add(config);
        }

        // Process nested configurations after their dependencies
        foreach (var config in SelectionRules.Where(rule => rule.IsNested))
        {
            var parentRule = SelectionRules.First(r => r.Name == config.DependsOn);
            config.SetParentPath(parentRule.FormattedPath);
            config.Navigators = navigator.SelectAllNodes(config.FormattedPath).ToList();

            try
            {
                config.SelectedIndex = ResourceSelector.SelectPreferredResourceIndex(config.Navigators, config.Types);
            }
            catch (FormatException e)
            {
                config.SelectedIndex = 1;
                errors.Add(new ParseError
                {
                    Kind = ErrorKind.InvalidValue,
                    Message = e.Message,
                    Path = navigator.GetFullPath(),
                    Severity = ErrorSeverity.Warning,
                });
            }

            processedRules.Add(config);
        }

        if (errors.MaxSeverity() >= ErrorSeverity.Fatal)
        {
            return errors;
        }

        return new ParseResult<ResourceSelectionRule>
        {
            Results = processedRules,
            Errors = errors
        };
    }
}