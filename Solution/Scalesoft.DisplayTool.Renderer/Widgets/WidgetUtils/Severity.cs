using System.Runtime.Serialization;

namespace Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;

public enum Severity
{
    [EnumMember(Value = "primary")]
    Primary,
    
    [EnumMember(Value = "secondary")]
    Secondary,
    
    [EnumMember(Value = "info")]
    Info,
    
    [EnumMember(Value = "success")]
    Success,
    
    [EnumMember(Value = "warning")]
    Warning,
    
    [EnumMember(Value = "error")]
    Error,
    
    [EnumMember(Value = "gray")]
    Gray,
}