namespace Scalesoft.DisplayTool.Renderer.Utils;

[AttributeUsage(AttributeTargets.Field)]
public class GroupAttribute : Attribute
{
    public string GroupName { get; }

    public GroupAttribute(string groupName)
    {
        GroupName = groupName;
    }
}