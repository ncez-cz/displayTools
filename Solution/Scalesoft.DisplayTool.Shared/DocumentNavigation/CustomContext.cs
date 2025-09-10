using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Scalesoft.DisplayTool.Shared.DocumentNavigation;

// From https://learn.microsoft.com/en-us/dotnet/standard/data/xml/user-defined-functions-and-variables, simplified
public class CustomContext : XsltContext
{
    public CustomContext(Dictionary<string, object> args)
        : base(new NameTable())
    {
        Variables = args;
    }

    // Function to resolve references to user-defined XPath extension
    // functions in XPath query expressions evaluated by using an
    // instance of this class as the XsltContext.
    public override IXsltContextFunction ResolveFunction(
        string prefix, string name,
        XPathResultType[] argTypes)
    {
        return null!;
    }

    // Function to resolve references to user-defined XPath
    // extension variables in XPath query.
    public override IXsltContextVariable ResolveVariable(string prefix, string name)
    {
        return (Variables.TryGetValue(name, out var variable) ? new CustomVariable(variable) : null)!;
    }

    // Empty implementation, returns false.
    public override bool PreserveWhitespace(XPathNavigator node)
    {
        return false;
    }

    // empty implementation, returns 0.
    public override int CompareDocument(string baseUri, string nextbaseUri)
    {
        return 0;
    }

    public override bool Whitespace => true;

    private Dictionary<string, object> Variables { get; }
}