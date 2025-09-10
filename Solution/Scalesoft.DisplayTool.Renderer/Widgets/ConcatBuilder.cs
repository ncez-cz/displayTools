using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets;

/// <summary>
///     Selects all nodes matching itemsPath, constructs a widget for each of them using itemBuilder,
///     then concatenates the results.
/// </summary>
/// <param name="itemsPath">Xpath expression that should match multiple nodes</param>
/// <param name="itemBuilder">
///     Gets called for every matching node, with the first parameter being the current index,
///     the second being the total number of nodes
/// </param>
public class ConcatBuilder(
    string itemsPath,
    Func<int, int, XmlDocumentNavigator, IList<Widget>> itemBuilder,
    Widget? separator = null,
    string? orderSelector = null,
    bool orderAscending = true
)
    : ParsingWidget(itemsPath)
{
    public ConcatBuilder(
        string itemsPath,
        Func<int, IList<Widget>> itemBuilder,
        string separator = "",
        string? orderSelector = null,
        bool orderAscending = true
    ) : this(itemsPath,
        (i, _, _) => itemBuilder(i), new ConstantText(separator), orderSelector, orderAscending)
    {
    }

    public ConcatBuilder(
        string itemsPath,
        Func<int, IList<Widget>> itemBuilder,
        Widget separator,
        string? orderSelector = null,
        bool orderAscending = true
    ) : this(itemsPath,
        (i, _, _) => itemBuilder(i), separator, orderSelector, orderAscending)
    {
    }

    public ConcatBuilder(
        string itemsPath,
        Func<int, int, IList<Widget>> itemBuilder,
        string separator = "",
        string? orderSelector = null,
        bool orderAscending = true
    ) : this(itemsPath,
        (i, totalCount, _) => itemBuilder(i, totalCount), new ConstantText(separator), orderSelector, orderAscending)
    {
    }


    public override async Task<RenderResult> Render(
        XmlDocumentNavigator data,
        IWidgetRenderer renderer,
        RenderContext context
    )
    {
        var elements = data.SelectAllNodes(Path).ToList();
        var count = elements.Count;
        IEnumerable<XmlDocumentNavigator> elementsToRender;
        if (!string.IsNullOrEmpty(orderSelector))
        {
            elementsToRender = orderAscending
                ? elements.OrderBy(nav => nav.EvaluateNumber(orderSelector))
                : elements.OrderByDescending(nav => nav.EvaluateNumber(orderSelector));
        }
        else
        {
            elementsToRender = elements;
        }

        List<RenderResult> children = [];
        foreach (var (element, i) in elementsToRender.Select((e, i) => (e, i)))
        {
            children.Add(await itemBuilder(i, count, element).RenderConcatenatedResult(element, renderer, context));
        }

        var separatorRendered = separator == null ? string.Empty : await separator.Render(data, renderer, context);


        var errors = children.SelectMany(x => x.Errors).ToList();
        errors.AddRange(errors);
        if (errors.MaxSeverity() >= ErrorSeverity.Fatal)
        {
            return errors;
        }

        var content = children.Select(x => x.Content).OfType<string>().ToList();
        var concatenated = string.Join(separatorRendered.Content, content);
        return new RenderResult(concatenated, errors);
    }
}