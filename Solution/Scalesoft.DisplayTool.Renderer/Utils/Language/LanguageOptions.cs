namespace Scalesoft.DisplayTool.Renderer.Utils.Language;

public sealed record LanguageOptions(string Code)
{
    public static readonly LanguageOptions Czech = new("cs-CZ");
    public static readonly LanguageOptions EnglishGreatBritain = new("en-GB");

    public string ShortCode => Code.Split('-')[0];
}