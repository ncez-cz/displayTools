using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils.Language;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

/// <summary>
///     Represents a section for rendering IPS (International Patient Summary) views, designed to process
///     and render coded sections based on provided navigators and associated data.
///     <br />
///     This widget expects to be called in the context of a Composition.
/// </summary>
public class ParentSection : Widget
{
    private readonly Widget m_title;
    private readonly LocalizedAbbreviations? m_titleAbbreviations;
    private readonly List<Widget> m_subsections;
    private readonly Severity? m_severity;
    
    public ParentSection(string loincCode,
        LocalizedAbbreviations? titleAbbreviations,
        List<Widget> subsections,
        Severity? severity = null
    )
    {
        m_title = new CodedValue(loincCode, "http://loinc.org", loincCode);
        m_titleAbbreviations = titleAbbreviations;
        m_subsections = subsections;
        m_severity = severity;
    }
    
    public ParentSection(
        ConstantText titleConstant,
        LocalizedAbbreviations? titleAbbreviations,
        List<Widget> subsections,
        Severity? severity = null
    )
    {
        m_title = titleConstant;
        m_titleAbbreviations = titleAbbreviations;
        m_subsections = subsections;
        m_severity = severity;
    }

    public override async Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var widget = new Section(".", null, [m_title], m_subsections,
            titleAbbreviations: m_titleAbbreviations, severity: m_severity);


        return await widget.Render(navigator, renderer, context);
    }
}