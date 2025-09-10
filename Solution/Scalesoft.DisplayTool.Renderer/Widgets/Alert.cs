using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

public class Alert : Widget
{
    private readonly ParseError[]? m_errors;
    private readonly Widget? m_message;
    private readonly Severity m_severity;
    private readonly IdentifierSource? m_idSource;
    private readonly IdentifierSource? m_visualIdSource;

    public Alert(ParseError[] errors, Severity severity, IdentifierSource? idSource = null, IdentifierSource? visualIdSource = null)
    {
        m_errors = errors;
        m_message = null;
        m_severity = severity;
        m_idSource = idSource;
        m_visualIdSource = visualIdSource ?? idSource;
    }

    public Alert(Widget message, Severity severity, IdentifierSource? idSource = null, IdentifierSource? visualIdSource = null)
    {
        m_errors = null;
        m_message = message;
        m_severity = severity;
        m_idSource = idSource;
        m_visualIdSource = visualIdSource ?? idSource;
    }
    
    public override async Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        string? messageWidgetContent = null;
        if (m_message != null)
        {
            var renderResult = await RenderInternal(navigator, renderer, context, m_message);
            if (renderResult.IsFatal)
            {
                return renderResult.Errors;
            }
            messageWidgetContent = renderResult.GetContent(m_message);
        }
        
        var message = m_errors?.FirstOrDefault()?.Message ?? messageWidgetContent ?? "Error appeared";
        var viewModel = new ViewModel
        {
            Severity = m_severity,
            MainMessage = message,
        }; 
        HandleIds(context, navigator, viewModel, m_idSource, m_visualIdSource);
        var result = await renderer.RenderAlert(viewModel);
        return new RenderResult(result);
    }

    public class ViewModel : ViewModelBase
    {
        public required Severity Severity { get; set; }

        public required string MainMessage { get; set; }

        public string? HeaderMessage { get; set; }

        public string? FooterMessage { get; set; }
    }
}