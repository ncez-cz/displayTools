using System.Text;
using System.Xml.XPath;

namespace Scalesoft.DisplayTool.Renderer.DocumentNavigation;

/// <summary>
/// Exposed internal methods from System.Xml.XPath.XPathNavigator with minimum required modifications and convert them to extensions
/// </summary>
public static class XPathNavigatorExtensions
{
    /// <summary>
    /// Returns ordinal number of attribute, namespace or child node within its parent.
    /// Order is reversed for attributes and child nodes to avoid O(N**2) running time.
    /// This property is useful for debugging, and also used in UniqueId implementation.
    /// </summary>
    private static uint IndexInParent(XPathNavigator navigator)
    {
        XPathNavigator nav = navigator.Clone();
        uint idx = 0;

        switch (navigator.NodeType)
        {
            case XPathNodeType.Attribute:
                while (nav.MoveToNextAttribute())
                {
                    idx++;
                }

                break;
            case XPathNodeType.Namespace:
                while (nav.MoveToNextNamespace())
                {
                    idx++;
                }

                break;
            default:
                while (nav.MoveToNext())
                {
                    idx++;
                }

                break;
        }

        return idx;
    }

    // (R)oot
    // (E)lement
    // (A)ttribute
    // (N)amespace
    // (T)ext
    // (S)ignificantWhitespace
    // (W)hitespace
    // (P)rocessingInstruction
    // (C)omment
    // (X) All
    private const string NodeTypeLetter = "REANTSWPCX";

    private const string UniqueIdTbl = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456";

    // Requirements for id:
    //  1. must consist of alphanumeric characters only
    //  2. must begin with an alphabetic character
    //  3. same id is generated for the same node
    //  4. ids are unique
    //
    //  id = node type letter + reverse path to root in terms of encoded IndexInParent integers from node to root separated by 0's if needed
    public static string UniqueId(this XPathNavigator navigator)
    {
        XPathNavigator nav = navigator.Clone();
        StringBuilder sb = new StringBuilder();

        // Ensure distinguishing attributes, namespaces and child nodes
        sb.Append(NodeTypeLetter[(int)navigator.NodeType]);

        while (true)
        {
            uint idx = IndexInParent(nav);
            if (!nav.MoveToParent())
            {
                break;
            }

            if (idx <= 0x1f)
            {
                sb.Append(UniqueIdTbl[(int)idx]);
            }
            else
            {
                sb.Append('0');
                do
                {
                    sb.Append(UniqueIdTbl[(int)(idx & 0x1f)]);
                    idx >>= 5;
                } while (idx != 0);

                sb.Append('0');
            }
        }

        return sb.ToString();
    }
}