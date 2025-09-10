using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

public class Image : Widget
{
    private readonly string? m_source;
    private readonly Widget? m_sourceWidget;
    private readonly string? m_altText;
    private readonly Widget? m_altTextWidget;
    private readonly string? m_width;
    private readonly string? m_height;
    private readonly string? m_optionalClass;
    private readonly string? m_optionalStyle;
    private readonly Dictionary<string, object>? m_otherAttributes;
    private static readonly string[] m_mainAttributes = ["src", "alt", "width", "height"];

    public Image(
        string source,
        string? altText = null,
        string? width = null,
        string? height = null,
        string? optionalClass = null,
        string? optionalStyle = null
    )
    {
        m_source = source;
        m_altText = altText;
        m_width = width;
        m_height = height;
        m_optionalClass = optionalClass;
        m_optionalStyle = optionalStyle;
    }

    /// <summary>
    ///     This takes a list of attributes, where the keys are the attribute names and the values are the attribute values and
    ///     writes them to the image.
    ///     This does not filter input, so it is up to the caller to ensure that only valid image attributes are passed.
    /// </summary>
    /// <param name="attributes">A list of key-value pairs representing the attributes of the image.</param>
    /// <exception cref="ArgumentException"></exception>
    public Image(List<KeyValuePair<string, string>> attributes)
    {
        m_source = attributes.FirstOrDefault(x => x.Key.Equals("src", StringComparison.OrdinalIgnoreCase)).Value;
        m_altText = attributes.FirstOrDefault(x => x.Key.Equals("alt", StringComparison.OrdinalIgnoreCase)).Value;
        m_width = attributes.FirstOrDefault(x => x.Key.Equals("width", StringComparison.OrdinalIgnoreCase)).Value;
        m_height = attributes.FirstOrDefault(x => x.Key.Equals("height", StringComparison.OrdinalIgnoreCase)).Value;
        m_otherAttributes = attributes
            .Where(x => !m_mainAttributes.Contains(x.Key, StringComparer.OrdinalIgnoreCase))
            .ToDictionary(
                x => x.Key,
                object (x) => x.Value,
                StringComparer.OrdinalIgnoreCase
            );
    }

    public Image(
        Widget source,
        Widget? altText = null,
        string? width = null,
        string? height = null,
        string? optionalClass = null,
        string? optionalStyle = null
    )
    {
        m_sourceWidget = source;
        m_altTextWidget = altText;
        m_width = width;
        m_height = height;
        m_optionalClass = optionalClass;
        m_optionalStyle = optionalStyle;
    }

    public override async Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        if (context.RenderMode == RenderMode.Documentation)
        {
            return navigator.GetFullPath();
        }

        string? outputSource;
        string? outputAlt = null;

        if (m_sourceWidget != null)
        {
            Widget[] content;
            if (m_altTextWidget != null)
            {
                content = [m_altTextWidget, m_sourceWidget];
            }
            else
            {
                content = [m_sourceWidget];
            }

            var renderResult = await RenderInternal(navigator, renderer, context, content);
            if (renderResult.IsFatal)
            {
                return renderResult.Errors;
            }

            outputSource = renderResult.GetContent(m_sourceWidget) ?? string.Empty;
            outputAlt = m_altTextWidget != null ? renderResult.GetContent(m_altTextWidget) : string.Empty;
        }
        else if (m_source != null)
        {
            outputSource = m_source;
            if (m_altText != null)
            {
                outputAlt = m_altText;
            }
        }
        else
        {
            return RenderResult.NullResult;
        }

        var viewModel = new ViewModel
        {
            Source = outputSource,
            AltText = outputAlt,
            Width = m_width,
            Height = m_height,
            OtherAttributes = m_otherAttributes,
            CustomClass = m_optionalClass,
            CustomStyle = m_optionalStyle
        };

        var view = await renderer.RenderImage(viewModel);
        return new RenderResult(view);
    }

    public class ViewModel : ViewModelBase
    {
        public required string Source { get; set; }
        public required string? AltText { get; set; }
        public required string? Width { get; set; }
        public required string? Height { get; set; }
        public Dictionary<string, object>? OtherAttributes { get; set; }
        public string? CustomStyle { get; set; }
    }
}