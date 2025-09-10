using PdfSharp.Pdf;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.IO;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using Scalesoft.DisplayTool.Renderer.Models.Exceptions;

namespace Scalesoft.DisplayTool.Renderer.DocumentRenderers.Tools;

public class HtmlToPdfConverter
{
    private readonly PdfRendererOptions m_pdfRendererOptions;

    public HtmlToPdfConverter(PdfRendererOptions pdfRendererOptions)
    {
        m_pdfRendererOptions = pdfRendererOptions;
    }
    
    public async Task<byte[]> ConvertHtmlToPdf(string html, byte[] fileAttachment, InputFormat inputFormat)
    {
        if (m_pdfRendererOptions.DownloadChromium == false && string.IsNullOrWhiteSpace(m_pdfRendererOptions.ChromiumExePath))
        {
            throw new ChromiumPathNotSpecifiedException();
        }

        var executablePath = m_pdfRendererOptions.ChromiumExePath;

        if (m_pdfRendererOptions.DownloadChromium)
        {
            var browserFetcher = new BrowserFetcher(new BrowserFetcherOptions
            {
                Path = string.IsNullOrEmpty(m_pdfRendererOptions.DownloadChromiumPath)
                    ? null
                    : m_pdfRendererOptions.DownloadChromiumPath,
            });
            var selectedInstalledBrowser = await browserFetcher.DownloadAsync();

            executablePath = selectedInstalledBrowser.GetExecutablePath();
        }

        var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true,
            ExecutablePath = executablePath,
        });
        
        await using var page = await browser.NewPageAsync();

        await page.SetContentAsync(html);
        var pdfOptions = new PdfOptions
        {
            Format = PaperFormat.A4,
            PrintBackground = true, 
            MarginOptions = new MarginOptions
            {
                Top = "50px",
                Bottom = "50px",
                Left = "40px",
                Right = "40px"
            }
        };
        
        var pdfBytes = await page.PdfDataAsync(pdfOptions);

        return AttachFileToPdf(pdfBytes, fileAttachment, inputFormat);
    }
    
    private byte[] AttachFileToPdf(byte[] pdf, byte[] fileAttachment, InputFormat inputFormat)
    {
        using var pdfStream = new MemoryStream(pdf);
        var document = PdfReader.Open(pdfStream, PdfDocumentOpenMode.Modify); 
    
        using var stream = new MemoryStream(fileAttachment); 
        var embFileStream = new PdfEmbeddedFileStream(document, stream);
        var fileName = BuildFileNameWithSuffix(inputFormat);
        
        var fileSpec = new PdfFileSpecification(document, embFileStream, fileName);

        document.Internals.Catalog.Elements.SetObject(
            "/Names",
            new PdfDictionary
            {
                Elements = {
                    { "/EmbeddedFiles", new PdfDictionary
                        {
                            Elements = {
                                { "/Names", new PdfArray {
                                        Elements =
                                        {
                                            new PdfString(fileName), fileSpec
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        );

        using var outputStream = new MemoryStream();
        document.Save(outputStream);
        return outputStream.ToArray();
    }

    private string BuildFileNameWithSuffix(InputFormat inputFormat)
    {
        switch (inputFormat)
        {
            case InputFormat.Pdf:
            {
                return "OriginalFile.pdf";
            }
            case InputFormat.FhirJson:
            {
                return "OriginalFile.json";
            }
            case InputFormat.FhirXml:
            case InputFormat.Cda:
            case InputFormat.Dasta:
            {
                return "OriginalFile.xml";
            }
            default:
            {
                throw new NotSupportedException($"Input format {inputFormat} is not supported for embedding to PDF");
            }
        }
    }
}
