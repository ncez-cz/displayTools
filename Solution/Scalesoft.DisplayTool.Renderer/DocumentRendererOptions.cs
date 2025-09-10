using Microsoft.Extensions.Logging;

namespace Scalesoft.DisplayTool.Renderer;

public class DocumentRendererOptions
{
    public ILoggerFactory? LoggerFactory { get; set; }
    public bool UseExternalValidators { get; set; }
    public PdfRendererOptions? PdfRenderer { get; set; }
}

public class PdfRendererOptions
{
    public string? ChromiumExePath { get; set; }
    public bool DownloadChromium { get; set; }
    public string? DownloadChromiumPath { get; set; }
}