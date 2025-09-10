using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Widgets.Fhir;

public class ShowMoney(string path = ".") : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer, RenderContext context)
    {
        if (IsDataAbsent(navigator, path))
        {
            return new AbsentData(path).Render(navigator, renderer, context);
        }

        if (context.RenderMode == RenderMode.Documentation)
        {
            return Task.FromResult<RenderResult>(navigator.SelectSingleNode(path).GetFullPath());
        }

        var tree = new ChangeContext(path, new Optional("f:value", new ShowDecimal()),
            new Condition("f:value and f:currency", new ConstantText(" ")),
            new Optional("f:currency", new EnumLabel(".", "http://hl7.org/fhir/ValueSet/currencies")));

        return tree.Render(navigator, renderer, context);
    }
}