using System.Diagnostics.CodeAnalysis;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Models;

public class ResourceIdentifier : IEquatable<ResourceIdentifier>
{
    private (string logicalId, string resourceType) Id { get; }

    public ResourceIdentifier(string logicalId, string resourceType)
    {
        Id = (logicalId, resourceType);
    }
    
    public static bool TryFromNavigator(XmlDocumentNavigator nav, [NotNullWhen(true)] out ResourceIdentifier? identifier, string idXpath = "f:id/@value")
    {
        if (nav.Node == null)
        {
            identifier = null;
            return false;
        }
        
        var idValue = nav.EvaluateString(idXpath);
        if (string.IsNullOrEmpty(idValue))
        {
            identifier = null;
            return false;
        }

        identifier = new ResourceIdentifier(idValue, nav.Node.Name);
        return true;
    }

    public static bool TryFromReference(string referenceValue, [NotNullWhen(true)] out ResourceIdentifier? identifier)
    {
        if (referenceValue.Split('/') is not [var resourceType, var logicalId])
        {
            identifier = null;
            return false;
        }
        
        identifier = new ResourceIdentifier(logicalId, resourceType);
        return true;
    }
    
    public string LogicalId => Id.logicalId;
    
    public string ResourceType => Id.resourceType;

    public string BuildId()
    {
        return Id.resourceType + "/" + Id.logicalId;
    }

    public bool Equals(ResourceIdentifier? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((ResourceIdentifier)obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
