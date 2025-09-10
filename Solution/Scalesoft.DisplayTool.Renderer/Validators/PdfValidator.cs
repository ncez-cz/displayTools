using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace Scalesoft.DisplayTool.Renderer.Validators;

public static class PdfValidator
{
    // Another points to consider:
    //
    // Signature Validation:
    //      Check if the pdf was signed and if so, if the signature is valid and the content wasnt tempted with.

    private static readonly byte[] m_pdfSignature = "%PDF"u8.ToArray(); 

    /// <summary>
    /// Validates if the provided byte array represents a valid PDF file with a full structural check.
    /// </summary>
    /// <param name="fileBytes">The file content as a byte array.</param>
    /// <returns>True if the file is a valid PDF, otherwise false.</returns>
    public static bool IsValidPdf(byte[] fileBytes)
    {
        if (!HasPdfHeadingSignature(fileBytes))
        {
            return false;
        }

        // Trying to open file also checks the structural side (trailer, and a cross-reference table)
        // These are mandatory in a valid PDF.
        PdfDocument pdfDocument;
        try
        {
            using var stream = new MemoryStream(fileBytes);
            pdfDocument = PdfReader.Open(stream);
        }
        catch
        {
            return false; 
        }

        if (!HasValidContent(pdfDocument))
        {
            return false; 
        }

        return true;
    }

    /// <summary>
    /// Checks if the byte array contains the PDF signature "%PDF".
    /// </summary>
    private static bool HasPdfHeadingSignature(byte[] fileBytes)
    {
        if (fileBytes.Length < m_pdfSignature.Length)
        {
            return false; 
        }

        // Check the first few bytes for the PDF signature.
        return fileBytes.Take(m_pdfSignature.Length).SequenceEqual(m_pdfSignature);
    }

    /// <summary>
    /// Checks the integrity of the content in the PDF document.
    /// </summary>
    private static bool HasValidContent(PdfDocument pdfDocument)
    {
        foreach (var page in pdfDocument.Pages)
        {
            if (page.Elements.Count == 0)
            {
                return false; 
            }
        }

        return true;
    }
}