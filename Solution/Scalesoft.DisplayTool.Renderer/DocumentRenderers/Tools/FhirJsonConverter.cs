using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml.Linq;

namespace Scalesoft.DisplayTool.Renderer.DocumentRenderers.Tools;

public static class FhirJsonConverter
{
    private static readonly XNamespace m_fhirNs = "http://hl7.org/fhir";

    public static string ToFhirXml(string jsonString)
    {
        using var doc = JsonDocument.Parse(jsonString);
        var root = doc.RootElement;
        var xmlDocument = new XDocument(new XDeclaration("1.0", "UTF-8", "no"));
        xmlDocument.Add(ConvertObjectToXml(root));

        return xmlDocument.ToString();
    }

    private static IList<XElement> ConvertObjectToXml(JsonElement jsonElement, XElement? parentXmlElement = null)
    {
        var result = new List<XElement>();

        if (TryGetResourceXmlElement(jsonElement, out var resourceXmlElement))
        {
            var updatedJsonElement = RemoveResourcePropertyFromJsonElement(jsonElement);

            resourceXmlElement!.Add(ConvertObjectToXml(updatedJsonElement, resourceXmlElement));
            result.Add(resourceXmlElement);

            return result;
        }

        foreach (var property in jsonElement.EnumerateObject())
        {
            if (property.NameEquals("div"))
            {
                var divElement = ConvertXhtmlDivToXml(property);
                result.Add(divElement);
                continue;
            }

            if (property.NameEquals("url") && parentXmlElement != null)
            {
                parentXmlElement.SetAttributeValue("url", property.Value.ToString());
                continue;
            }

            if (property.NameEquals("id") && parentXmlElement != null)
            {
                parentXmlElement.SetAttributeValue("id", property.Value.ToString());
            }

            // JSON properties with underscore prefix pairs with the property of the same name without prefix
            // its properties are translated as attributes in XML to the XElement of the matching property.
            // if no matching element without prefix exists, render element without value.
            if (property.Name.Contains('_'))
            {
                var propertyNameWithoutPrefix = property.Name.Remove(0, 1);
                var matchingPropertyExists = jsonElement.TryGetProperty(propertyNameWithoutPrefix, out _);

                if (matchingPropertyExists)
                {
                    continue;
                }
            }

            switch (property.Value.ValueKind)
            {
                case JsonValueKind.Object:
                    var objectXmlElement = new XElement(m_fhirNs + GetValidElementName(property.Name));
                    objectXmlElement.Add(ConvertObjectToXml(property.Value, objectXmlElement));
                    result.Add(objectXmlElement);
                    break;
                case JsonValueKind.Array:
                    result.AddRange(ConvertJsonArrayToXml(property, jsonElement));
                    break;
                case JsonValueKind.String:
                case JsonValueKind.Number:
                case JsonValueKind.True:
                case JsonValueKind.False:
                    result.Add(ConvertJsonPrimitiveToXml(property, jsonElement));
                    break;
                default:
                    continue;
            }
        }

        return result;
    }

    private static List<XElement> ConvertJsonArrayToXml(JsonProperty property, JsonElement parent)
    {
        var result = new List<XElement>();
        var index = 0;

        foreach (var item in property.Value.EnumerateArray())
        {
            var sectionXmlElement = new XElement(m_fhirNs + GetValidElementName(property.Name));

            switch (item.ValueKind)
            {
                case JsonValueKind.Object:
                    sectionXmlElement.Add(ConvertObjectToXml(item, sectionXmlElement));
                    break;
                case JsonValueKind.Array:
                    break;
                case JsonValueKind.String:
                case JsonValueKind.Number:
                case JsonValueKind.True:
                case JsonValueKind.False:
                    sectionXmlElement.SetAttributeValue("value", item.ToString());
                    HandlePrimitivePropertyAdditionalInfo(property, index, parent, sectionXmlElement);
                    break;
            }

            result.Add(sectionXmlElement);
            index++;
        }

        return result;
    }

    private static XElement ConvertJsonPrimitiveToXml(JsonProperty property, JsonElement parent)
    {
        var value = property.Value.ToString();
        if (property.Value.ValueKind is JsonValueKind.True or JsonValueKind.False)
        {
            value = value.ToLower();
        }

        var element = new XElement(m_fhirNs + GetValidElementName(property.Name), new XAttribute("value", value));

        HandlePrimitivePropertyAdditionalInfo(property, 0, parent, element);

        return element;
    }

    private static XElement ConvertXhtmlDivToXml(JsonProperty property)
    {
        XNamespace xhtmlNs = "http://www.w3.org/1999/xhtml";
        var xhtmlElement = XElement.Parse(property.Value.GetString() ?? string.Empty);

        var divElement = new XElement(xhtmlNs + "div",
            xhtmlElement.Nodes().Select(node =>
                node is XElement el ? new XElement(xhtmlNs + el.Name.LocalName, el.Attributes(), el.Nodes()) : node));

        return divElement;
    }

    private static bool TryGetResourceXmlElement(JsonElement element, out XElement? resourceElement)
    {
        if (element.EnumerateObject().Any(x => x.NameEquals("resourceType")))
        {
            var elementName = element.EnumerateObject().First(x => x.NameEquals("resourceType")).Value.GetString();
            resourceElement = new XElement(m_fhirNs + GetValidElementName(elementName));
            return true;
        }

        resourceElement = default;
        return false;
    }

    private static JsonElement RemoveResourcePropertyFromJsonElement(JsonElement json)
    {
        var jsonNode = JsonNode.Parse(json.GetRawText());
        if (jsonNode == null)
        {
            throw new InvalidOperationException();
        }

        jsonNode.AsObject().Remove("resourceType");

        return jsonNode.Deserialize<JsonElement>();
    }

    private static void HandlePrimitivePropertyAdditionalInfo(
        JsonProperty property,
        int propertyIndex,
        JsonElement parent,
        XElement parentXmlElement
    )
    {
        if (parent.ValueKind == JsonValueKind.Object)
        {
            var propertyHasAdditionalInfo = parent.TryGetProperty($"_{property.Name}", out var additionalInfoElement);
            if (propertyHasAdditionalInfo)
            {
                if (additionalInfoElement.ValueKind == JsonValueKind.Object)
                {
                    parentXmlElement.Add(ConvertObjectToXml(additionalInfoElement, parentXmlElement));
                }
                else if (additionalInfoElement.ValueKind == JsonValueKind.Array)
                {
                    var currentExtension = additionalInfoElement[propertyIndex];
                    if (currentExtension.ValueKind != JsonValueKind.Null)
                    {
                        parentXmlElement.Add(ConvertObjectToXml(currentExtension, parentXmlElement));
                    }
                }
            }
        }
    }

    private static string GetValidElementName(string? propertyName)
    {
        return propertyName?.Replace("_", string.Empty) ?? string.Empty;
    }
}