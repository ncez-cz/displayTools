using System.Web;
using System.Xml;
using System.Xml.XPath;

namespace Scalesoft.DisplayTool.TermxTranslator;

public class TermxApiClient
{
    private readonly HttpClient m_client;

    public TermxApiClient(HttpClient client)
    {
        m_client = client;
    }

    public async Task<XPathDocument> LookupCodeSystemValue(
        string code,
        string codeSystem
    )
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query.Add("system", codeSystem);
        query.Add("code", code);

        var response = await m_client.GetAsync($"CodeSystem/$lookup?{query}");
        response.EnsureSuccessStatusCode();

        var reader = XmlReader.Create(await response.Content.ReadAsStreamAsync());
        var document = new XPathDocument(reader, XmlSpace.Preserve);

        return document;
    }

    public async Task<XPathDocument> ExpandValueSet(
        string valueSet,
        string filter
    )
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query.Add("url", valueSet);
        query.Add("filter", filter);

        var response = await m_client.GetAsync($"ValueSet/$expand?{query}");
        response.EnsureSuccessStatusCode();

        var reader = XmlReader.Create(await response.Content.ReadAsStreamAsync());
        var document = new XPathDocument(reader, XmlSpace.Preserve);

        return document;
    }

    public async Task<XPathDocument> LookupCodeSystem(string identifier)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query.Add("identifier", identifier);


        var response = await m_client.PostAsync($"CodeSystem/_search?{query}", null);
        response.EnsureSuccessStatusCode();

        var reader = XmlReader.Create(await response.Content.ReadAsStreamAsync());
        var document = new XPathDocument(reader, XmlSpace.Preserve);

        return document;
    }
}