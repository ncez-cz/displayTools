using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Utils;

/// <summary>
/// Defines a rule for selecting and prioritizing FHIR resources within XML documents.
/// 
/// Each rule specifies an XPath to locate resources, priority-ordered type values for selection,
/// and tracking for the selected resource index. Rules can be standalone or nested within
/// other rules to support hierarchical resource selection.
/// 
/// The class works with the ResourceSelector to implement the actual selection algorithm
/// that chooses the most appropriate resource based on the specified types and validity criteria.
/// </summary>
public class ResourceSelectionRule
{
    /// <summary>
    /// Gets the unique identifier for this rule, used for dependency references.
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Gets the XPath expression used to select nodes from the XML document.
    /// </summary>
    public string Path { get; }
    
    /// <summary>
    /// Gets the array of prioritized resource types. Resources will be matched against these 
    /// types in sequence, with earlier matches taking precedence.
    /// </summary>
    public string?[] Types { get; }
    
    /// <summary>
    /// Gets or sets the index (1-based) of the selected resource after processing.
    /// </summary>
    public int SelectedIndex { get; set; }
    
    /// <summary>
    /// Gets or sets the list of XML navigators containing the matched resources.
    /// </summary>
    public List<XmlDocumentNavigator> Navigators { get; set; } = [];
    
    /// <summary>
    /// Gets the name of the parent rule that this rule depends on, if any.
    /// Null indicates this is a standalone (non-nested) rule.
    /// </summary>
    public string? DependsOn { get; }
    
    /// <summary>
    /// Gets whether this rule is nested within another rule.
    /// </summary>
    public bool IsNested => DependsOn != null;
    
    private string? m_parentFormattedPath;
    
    /// <summary>
    /// Gets the formatted XPath with index selection for the selected resource.
    /// For nested rules, includes the parent rule's formatted path.
    /// </summary>
    public string FormattedPath
    {
        get
        {
            if (IsNested && m_parentFormattedPath != null)
            {
                return $"{m_parentFormattedPath}/{Path}[{SelectedIndex}]";
            }
            return $"{Path}[{SelectedIndex}]";
        }
    }

    /// <summary>
    /// Initializes a new instance of the ResourceSelectionRule class.
    /// </summary>
    /// <param name="name">The unique identifier for this rule.</param>
    /// <param name="path">The XPath expression to select nodes from the XML document.</param>
    /// <param name="types">An array of prioritized resource types for selection.</param>
    /// <param name="dependsOn">Optional name of the parent rule that this rule depends on.</param>
    public ResourceSelectionRule(string name, string path, string?[] types, string? dependsOn = null)
    {
        Name = name;
        Path = path;
        Types = types;
        DependsOn = dependsOn;
    }

    /// <summary>
    /// Sets the parent rule's formatted path for this rule.
    /// This is used to build the complete XPath for nested rules.
    /// </summary>
    /// <param name="parentPath">The formatted path of the parent rule.</param>
    public void SetParentPath(string parentPath)
    {
        m_parentFormattedPath = parentPath;
    }
}