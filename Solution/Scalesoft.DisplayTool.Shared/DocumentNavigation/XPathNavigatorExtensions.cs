using System.Xml.XPath;
using System.Xml;


namespace Scalesoft.DisplayTool.Shared.DocumentNavigation;

public static class XPathNavigatorExtensions
{
    public static IEnumerable<XPathNavigator> SelectAllNodes(this XPathNavigator nav, string xpath,
        IXmlNamespaceResolver? resolver = null)
    {
        var iterator = resolver == null ? nav.Select(xpath) : nav.Select(xpath, resolver);

        return SelectAllNodes(iterator);
    }

    public static IEnumerable<XPathNavigator> SelectAllNodes(this XPathNavigator nav, XPathExpression expression)
    {
        var iterator = nav.Select(expression);

        return SelectAllNodes(iterator);
    }

    public static IEnumerable<XPathNavigator> SelectAllNodes(this IEnumerable<XPathNavigator> navs, string xpath,
        IXmlNamespaceResolver? resolver = null)
    {
        return navs.SelectMany(nav => SelectAllNodes(nav, xpath, resolver));
    }

    private static IEnumerable<XPathNavigator> SelectAllNodes(XPathNodeIterator iterator)
    {
        return iterator.OfType<XPathNavigator>();
    }
}