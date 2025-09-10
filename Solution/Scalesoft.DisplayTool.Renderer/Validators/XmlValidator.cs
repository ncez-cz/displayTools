using System.Xml;

namespace Scalesoft.DisplayTool.Renderer.Validators;

public class XmlValidator
{
    public static bool IsValidXml(string xml)
    {
        try
        {
            using var stringReader = new StringReader(xml);
            using var reader = XmlReader.Create(stringReader);
            while (reader.Read()) { }

            return true;
        }
        catch (XmlException)
        {
            return false;
        }
    }
}