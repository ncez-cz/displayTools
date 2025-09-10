namespace Scalesoft.DisplayTool.Renderer.Utils.Language;

public class Language
{
    public LanguageOptions Primary { get; set; } = LanguageOptions.Czech;
    public LanguageOptions Fallback { get; private set; } = LanguageOptions.EnglishGreatBritain;
}