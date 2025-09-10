using System.Collections.ObjectModel;

namespace Scalesoft.DisplayTool.Renderer.Utils.Language;

public class LocalizedAbbreviations
{
    public ReadOnlyDictionary<LanguageOptions, string> Abbreviations { get; }

    public LocalizedAbbreviations(IDictionary<LanguageOptions, string> abbreviations)
    {
        Abbreviations = new ReadOnlyDictionary<LanguageOptions, string>(abbreviations);
    }
    
    public LocalizedAbbreviations(string csAbbr, string enAbbr)
    {
        Abbreviations = new ReadOnlyDictionary<LanguageOptions, string>(new Dictionary<LanguageOptions, string>
        {
            {LanguageOptions.Czech, csAbbr}, 
            {LanguageOptions.EnglishGreatBritain, enAbbr}, 
        });
    }
    
    public static implicit operator LocalizedAbbreviations((string csAbbr, string enAbbr) val)
    {
        return new LocalizedAbbreviations(val.csAbbr, val.enAbbr);
    }
}