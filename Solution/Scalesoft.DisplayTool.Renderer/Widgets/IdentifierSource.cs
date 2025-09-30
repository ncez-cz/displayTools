using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

public class IdentifierSource
{
    public XmlDocumentNavigator? IdNav { get; private set; }

    public string? ConstantVal { get; private set; }

    public bool UseContextNav { get; private set; }

    public IdentifierSource(XmlDocumentNavigator idNav)
    {
        IdNav = idNav;
        UseContextNav = false;
    }

    public IdentifierSource(string constantVal)
    {
        ConstantVal = constantVal;
        UseContextNav = false;
    }

    public IdentifierSource(bool useContextNav = true)
    {
        UseContextNav = useContextNav;
    }

    public static implicit operator IdentifierSource?(XmlDocumentNavigator? idNav)
    {
        return idNav == null ? null : new IdentifierSource(idNav);
    }

    public static implicit operator IdentifierSource(string constantVal)
    {
        return new IdentifierSource(constantVal);
    }
}