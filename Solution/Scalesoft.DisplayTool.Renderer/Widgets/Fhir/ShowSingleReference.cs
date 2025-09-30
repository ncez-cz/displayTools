using System.Diagnostics.CodeAnalysis;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Utils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class ShowSingleReference : Widget
{
    private Func<XmlDocumentNavigator, IList<Widget>>? BuilderByNavigator { get; }
    private Func<ReferenceNavigatorOrDisplay, IList<Widget>>? BuilderByNavigatorOrDisplay { get; }
    private string Path { get; }

    [MemberNotNullWhen(true, nameof(BuilderByNavigator))]
    [MemberNotNullWhen(false, nameof(BuilderByNavigatorOrDisplay))]
    private bool UsingReferencedResourceOnly { get; }

    private ShowSingleReference(Func<XmlDocumentNavigator, IList<Widget>> contentBuilder, string path = ".")
    {
        Path = path;
        BuilderByNavigator = contentBuilder;
        UsingReferencedResourceOnly = true;
    }

    /// <summary>
    ///     This method build reference only for referenced resources and uses default handler for display value - display it as plain text.
    /// </summary>
    /// <param name="contentBuilder">Content builder for selected referenced resource</param>
    /// <param name="path">XPath to reference</param>
    /// <param name="referencePrefix"></param>
    /// <returns></returns>
    public static ShowSingleReference WithDefaultDisplayHandler(
        Func<XmlDocumentNavigator, IList<Widget>> contentBuilder,
        string path = "."
    ) => new ShowSingleReference(contentBuilder, path);

    public ShowSingleReference(
        Func<ReferenceNavigatorOrDisplay, IList<Widget>> contentBuilder,
        string path = "."
    )
    {
        Path = path;
        BuilderByNavigatorOrDisplay = contentBuilder;
        UsingReferencedResourceOnly = false;
    }

    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer, RenderContext context)
    {
        var displayValue = navigator.SelectSingleNode($"{Path}/f:display/@value").Node?.Value;
        if (navigator.SelectSingleNode($"{Path}/f:reference").Node == null)
        {
            if (UsingReferencedResourceOnly)
            {
                return Task.FromResult<RenderResult>(displayValue ?? string.Empty);
            }

            var widget = BuilderByNavigatorOrDisplay(displayValue);

            return widget.RenderConcatenatedResult(navigator, renderer, context);
        }

        // detect broken reference - no content
        var brokenReferences = ReferenceHandler.GetReferencesWithoutContent(navigator, Path);
        if (brokenReferences.Count != 0)
        {
            if (context.RenderMode == RenderMode.Documentation)
            {
                return Task.FromResult<RenderResult>(string.Join(' ', brokenReferences.Select(x => x.GetFullPath())));
            }
            var brokenReferenceWidget = ReferenceHandler.BuildReferenceNameWidget(brokenReferences.First(), context, false);
            return brokenReferenceWidget.Render(navigator, renderer, context);
        }

        // reference is not broken, render it
        SingleReference sr;
        if (UsingReferencedResourceOnly)
        {
            sr = new SingleReference(BuilderByNavigator, $"{Path}/f:reference");
        }
        else
        {
            sr = new SingleReference(nav => BuilderByNavigatorOrDisplay(nav), $"{Path}/f:reference");
        }

        return sr.Render(navigator, renderer, context);
    }
}