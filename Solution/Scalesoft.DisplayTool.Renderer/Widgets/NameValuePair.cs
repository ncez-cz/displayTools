using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

public class NameValuePair : Widget
{
    private readonly IList<Widget> m_name;
    private readonly IList<Widget> m_value;
    private readonly IdentifierSource? m_idSource;
    private readonly IdentifierSource? m_visualIdSource;

    public NameValuePair(IList<Widget> name, IList<Widget> value, IdentifierSource? idSource = null, IdentifierSource? visualIdSource = null)
    {
        m_name = name;
        m_value = value;
        m_idSource = idSource;
        m_visualIdSource = visualIdSource ?? idSource;
    }

    public NameValuePair(
        Widget name,
        Widget value,
        IdentifierSource? idSource = null,
        IdentifierSource? visualIdSource = null)
        : this([name], [value], idSource, visualIdSource)
    {
    }

    public override async Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        var renderResult = await RenderInternal(navigator, renderer, context, [..m_name, ..m_value]);
        if (renderResult.IsFatal)
        {
            return renderResult.Errors;
        }

        var nameText = string.Join(string.Empty, m_name.Select(x => renderResult.GetContent(x) ?? string.Empty));
        var valueText = string.Join(string.Empty, m_value.Select(x => renderResult.GetContent(x) ?? string.Empty));
        var viewModel = new ViewModel
        {
            NameContent = nameText,
            ValueContent = valueText,
        };
        HandleIds(context, navigator, viewModel, m_idSource, m_visualIdSource);
        
        return await renderer.RenderNameValuePair(viewModel);
    }

    public class ViewModel : ViewModelBase    {
        public required string NameContent { get; set; }

        public required string ValueContent { get; set; }
    }
}
