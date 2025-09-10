using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Utils;

public class ReferenceResult
{
    public List<XmlDocumentNavigator> Navigators { get; } = [];
    public List<ParseError> Errors { get; } = [];
}