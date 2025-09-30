namespace Scalesoft.DisplayTool.Shared.Translation;

public interface ICodeTranslator
{
    public Task<string?> GetCodedValue(string fileName,
        string code,
        string codeSystem,
        string language,
        string fallbackLanguage,
        bool isValueSet = false);
}