using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

public class ItemList : Widget
{
    private readonly ItemListType m_type;
    private readonly IList<Widget> m_content;
    private readonly IdentifierSource? m_idSource;
    private readonly IdentifierSource? m_visualIdSource;

    public ItemList(ItemListType type, IList<Widget> content, IdentifierSource? idSource = null, IdentifierSource? visualIdSource = null)
    {
        m_type = type;
        m_content = content;
        m_idSource = idSource;
        m_visualIdSource = visualIdSource ?? idSource;
    }

    public override async Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
        var children = await RenderInternal(navigator, renderer, m_content, context);

        if (children.IsFatal)
        {
            return children.Errors;
        }

        var viewModel = new ViewModel
            { Type = m_type, Rows = children.GetValidContents().ToList() };
        HandleIds(context, navigator, viewModel, m_idSource, m_visualIdSource);
        var result = await renderer.RenderItemList(viewModel);
        return new RenderResult(result, children.Errors);
    }

    public class ViewModel : ViewModelBase    {
        public required ItemListType Type { get; init; }
        public required List<string> Rows { get; init; }
    }
}