using System.Runtime.Serialization;

namespace Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;

public enum Priority
{
    [EnumMember(Value = "routine")]
    Routine = 0,
    
    [EnumMember(Value = "urgent")]
    Urgent = 1,
    
    [EnumMember(Value = "asap")]
    Asap = 2,
    
    [EnumMember(Value = "stat")]
    Stat = 3,
}