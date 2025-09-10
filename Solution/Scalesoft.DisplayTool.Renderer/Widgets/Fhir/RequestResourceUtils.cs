using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public static class RequestResourceUtils
{
    public static Widget ReasonBuilder(List<ReferenceNavigatorOrDisplay> referenceData)
    {
        var result = new List<Widget>();
        var referenceWithResources = new List<XmlDocumentNavigator>();
        var referencesWithDisplay = new List<string>();
        foreach (var navigatorOrDisplay in referenceData)
        {
            if (navigatorOrDisplay.ResourceReferencePresent)
            {
                referenceWithResources.Add(navigatorOrDisplay.Navigator);
            }
            else
            {
                if (navigatorOrDisplay.ReferenceDisplay != null)
                {
                    referencesWithDisplay.Add(navigatorOrDisplay.ReferenceDisplay);
                }
            }
        }

        if (referenceWithResources.Count != 0)
        {
            foreach (var referenceWithResource in referenceWithResources)
            {
                result.Add(new Container([new RequestReason(referenceWithResource)], idSource: referenceWithResource));
            }
        }

        if (referencesWithDisplay.Count != 0)
        {
            result.Add(new ItemList(ItemListType.Unordered, [..referencesWithDisplay.Select(x => new ConstantText(x))]));
        }

        return new Container(result);
    }
}