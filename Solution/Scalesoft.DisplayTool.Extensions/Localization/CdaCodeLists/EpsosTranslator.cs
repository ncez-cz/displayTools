using System.Xml;
using Scalesoft.DisplayTool.Shared.Translation;

namespace Scalesoft.DisplayTool.Extensions.Localization.CdaCodeLists;

public class EpsosTranslator : ICodeTranslator
{
    public Task<string?> GetCodedValue(string fileName,
        string code,
        string codeSystem,
        string userLang,
        string defaultUserLang,
        bool isValueSet)
    {
        using var res = ReadEpsosCodelist(fileName);
        if (res == null)
        {
            throw new InvalidOperationException("Invalid CDA code list file - empty");
        }

        var doc = new XmlDocument();
        doc.Load(res);
        var root = doc.DocumentElement;
        if (root == null)
        {
            throw new InvalidOperationException("Missing CDA code file root element");
        }

        code = SurroundWithQuotes(code);
        codeSystem = SurroundWithQuotes(codeSystem);
        userLang = SurroundWithQuotes(userLang);
        defaultUserLang = SurroundWithQuotes(defaultUserLang);
        var foundKey = root.SelectSingleNode($"/ValueSet/concept[@code={code} and @codeSystem={codeSystem}]");
        var foundKeyLang = foundKey?.SelectSingleNode($"designation[@lang={userLang}]");
        var defFoundKeyLang = foundKey?.SelectSingleNode($"designation[@lang={defaultUserLang}]");

        return Task.FromResult(foundKeyLang == null ? defFoundKeyLang?.InnerText : foundKeyLang.InnerText);
    }

    private static Stream? ReadEpsosCodelist(string fileName)
    {
        var assembly = typeof(EpsosTranslator).Assembly;
        var embeddedFiles = assembly.GetManifestResourceNames();
        var embeddedFilename = embeddedFiles.FirstOrDefault(x => x.Contains(fileName));
        if (string.IsNullOrEmpty(embeddedFilename))
        {
            throw new InvalidOperationException("Failed to find CDA code list file");
        }

        return assembly.GetManifestResourceStream(embeddedFilename);
    }

    private static string SurroundWithQuotes(string val)
    {
        if (string.IsNullOrEmpty(val))
        {
            return "''";
        }

        if (!val.StartsWith('\''))
        {
            val = '\'' + val;
        }

        if (!val.EndsWith('\''))
        {
            val += '\'';
        }

        return val;
    }
}