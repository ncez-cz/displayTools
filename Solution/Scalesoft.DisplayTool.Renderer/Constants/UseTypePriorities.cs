namespace Scalesoft.DisplayTool.Renderer.Constants;

/// <summary>
/// Defines prioritized lists of FHIR 'use' type values for different resource elements.
/// Each array defines the preferred types in priority order, with null representing an unspecified type.
/// </summary>
public static class UseTypePriorities
{
    public static readonly string?[] IdentifierTypes = [UseTypes.Official, null];
    public static readonly string?[] NameTypes = [UseTypes.Official, null];
    public static readonly string?[] AddressTypes = [UseTypes.Home, null];
    public static readonly string?[] OrgAddressTypes = [UseTypes.Work, null];
}
