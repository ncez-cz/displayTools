using System.Xml.XPath;
using System.Xml.Xsl;

namespace Scalesoft.DisplayTool.Shared.DocumentNavigation;

public class CustomVariable : IXsltContextVariable
{
    private readonly object m_value;

    public CustomVariable(object value)
    {
        m_value = value;
    }

    public object Evaluate(XsltContext xsltContext)
    {
        return m_value;
    }

    public bool IsLocal => false;
    public bool IsParam => false;
    public XPathResultType VariableType => XPathResultType.Any;
}