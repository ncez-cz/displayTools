﻿using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

public class NameValuePair : Widget
{
    private readonly IList<Widget> m_name;
    private readonly IList<Widget> m_value;
    private readonly IdentifierSource? m_idSource;
    private readonly FlexDirection m_direction;
    private readonly NameValuePairSize m_size;
    private readonly NameValuePairStyle m_style;
    private readonly IdentifierSource? m_visualIdSource;

    public NameValuePair(
        IList<Widget> name,
        IList<Widget> value,
        IdentifierSource? idSource = null,
        IdentifierSource? visualIdSource = null,
        FlexDirection direction = FlexDirection.Row,
        NameValuePairSize size = NameValuePairSize.Regular,
        NameValuePairStyle style =
            NameValuePairStyle.Initial // ideally, a specific style should be specified as default
    )
    {
        m_name = name;
        m_value = value;
        m_idSource = idSource;
        m_direction = direction;
        m_size = size;
        m_style = style;
        m_visualIdSource = visualIdSource ?? idSource;
    }

    public NameValuePair(
        Widget name,
        Widget value,
        IdentifierSource? idSource = null,
        IdentifierSource? visualIdSource = null,
        FlexDirection direction = FlexDirection.Row,
        NameValuePairSize size = NameValuePairSize.Regular,
        NameValuePairStyle style =
            NameValuePairStyle.Initial // ideally, a specific style should be specified as default
    )
        : this([name], [value], idSource, visualIdSource, direction, size, style)
    {
    }

    public override async Task<RenderResult> Render(
        XmlDocumentNavigator navigator,
        IWidgetRenderer renderer,
        RenderContext context
    )
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
            Direction = m_direction,
            Size = m_size,
            Style = m_style,
        };
        HandleIds(context, navigator, viewModel, m_idSource, m_visualIdSource);

        return await renderer.RenderNameValuePair(viewModel);
    }

    public class ViewModel : ViewModelBase
    {
        public required string NameContent { get; set; }

        public required string ValueContent { get; set; }

        public required FlexDirection Direction { get; set; }

        public required NameValuePairSize Size { get; set; }

        public required NameValuePairStyle Style { get; set; }
    }

    public enum NameValuePairSize
    {
        Regular = 1,
        Small = 2,
    }

    public enum NameValuePairStyle
    {
        Initial = 0, // unstyled, ideally should not be used 
        Primary = 1,
        Secondary = 2,
    }
}