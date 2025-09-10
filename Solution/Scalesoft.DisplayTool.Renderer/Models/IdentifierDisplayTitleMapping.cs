namespace Scalesoft.DisplayTool.Renderer.Models;

public class IdentifierDisplayTitleMapping
{
    private int m_count;
    private readonly Dictionary<string, string> m_mapping = new();

    public bool Add(string identifier)
    {
        m_count++;
        return m_mapping.TryAdd(identifier, m_count.ToString());
    }

    public string? GetTitle(string identifier)
    {
        m_mapping.TryGetValue(identifier, out var val);

        return val;
    }
}