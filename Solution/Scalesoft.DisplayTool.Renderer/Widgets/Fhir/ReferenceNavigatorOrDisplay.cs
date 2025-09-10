using System.Diagnostics.CodeAnalysis;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class ReferenceNavigatorOrDisplay
{
    public string? ReferenceDisplay { get; }
    
    public XmlDocumentNavigator? Navigator { get; }
    
    [MemberNotNullWhen(true, nameof(Navigator))]
    [MemberNotNullWhen(false, nameof(ReferenceDisplay))]
    public bool ResourceReferencePresent { get; }

    public ReferenceNavigatorOrDisplay(XmlDocumentNavigator navigator)
    {
        Navigator = navigator;
        ResourceReferencePresent = true;
    }
    
    public ReferenceNavigatorOrDisplay(string? referenceDisplay)
    {
        ReferenceDisplay = referenceDisplay;
        ResourceReferencePresent = false;
    }
    
    public static implicit operator ReferenceNavigatorOrDisplay(XmlDocumentNavigator navigator)
    {
        return new ReferenceNavigatorOrDisplay(navigator);
    }
    
    public static implicit operator ReferenceNavigatorOrDisplay(string? referenceDisplay)
    {
        return new ReferenceNavigatorOrDisplay(referenceDisplay);
    }
}
