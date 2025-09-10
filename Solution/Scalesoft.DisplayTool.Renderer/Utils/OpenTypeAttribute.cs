using Scalesoft.DisplayTool.Renderer.Widgets;
using Scalesoft.DisplayTool.Renderer.Widgets.Fhir;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Utils;

/// <summary>
///     Used for "open-type" elements (value[x], effective[x], etc.)
///     <br /><br />
///     Signifies that
///     <see cref="InfrequentProperties.Evaluate{T}" />
///     should return this enum member
///     iff any element that starts with the given prefix is found.
/// </summary>
/// <remarks>
///     <see cref="PropertyPathAttribute" /> takes precedence over this attribute (this attribute is ignored if
///     PropertyPathAttribute is present)
/// </remarks>
[AttributeUsage(AttributeTargets.Field)]
public class OpenTypeAttribute : InfrequentPropertyAttribute
{
    public string Prefix { get; }

    public OpenTypeAttribute(string prefix)
    {
        Prefix = prefix;
    }

    public override string? Evaluate(List<XmlDocumentNavigator> items)
    {
        if (Prefix == "value")
        {
            if (items.Any(x => x.EvaluateCondition(OpenTypeElement.ValueR5ExtensionPath)))
            {
                return OpenTypeElement.ValueR5ExtensionPath;
            }
        }

        var propertyPath = $"f:*[starts-with(name(), '{Prefix}')]";
        return items.Any(x => x.EvaluateCondition(propertyPath)) ? propertyPath : null;
    }
}