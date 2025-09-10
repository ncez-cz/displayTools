using System.Reflection;
using Scalesoft.DisplayTool.Renderer.Models;

namespace Scalesoft.DisplayTool.Renderer.Providers;

public class FrontendResourceProvider
{
    private const string DistPath = "HtmlResources/dist/";
    
    public FrontendResources ReadResources()
    {
        var type = typeof(DocumentRenderer);
        var assembly = type.Assembly;
        var embeddedResources = assembly.GetManifestResourceNames();
        var embeddedDistPath = type.Namespace + '.' + DistPath.Replace('/', '.');
        var embeddedJs = embeddedResources.Where(x => x.StartsWith(embeddedDistPath) && x.EndsWith(".js"));
        var embeddedCss = embeddedResources.Where(x => x.StartsWith(embeddedDistPath) && x.EndsWith(".css"));
        var scripts = new List<string>();
        var styles = new List<string>();
        foreach (var jsPath in embeddedJs)
        {
            scripts.Add(ReadEmbeddedFile(assembly, jsPath));
        }
        foreach (var cssPath in embeddedCss)
        {
            styles.Add(ReadEmbeddedFile(assembly, cssPath));
        }

        return new FrontendResources
        {
            Scripts = scripts,
            Styles = styles,
        };
    }

    private string ReadEmbeddedFile(Assembly assembly, string fullPath)
    {
        using var scriptStream = assembly.GetManifestResourceStream(fullPath);
        if (scriptStream == null)
        {
            throw new InvalidOperationException($"Could not find embedded resource: {fullPath}");
        }

        using var sr = new StreamReader(scriptStream);
        return sr.ReadToEnd();
    }
}
