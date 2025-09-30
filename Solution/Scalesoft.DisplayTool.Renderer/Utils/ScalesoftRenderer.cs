using System.Globalization;
using System.Text;
using ZXing;
using ZXing.Common;
using ZXing.OneD;
using ZXing.Rendering;

namespace Scalesoft.DisplayTool.Renderer.Utils;

public class ScalesoftRenderer : SvgRenderer, IBarcodeRenderer<ScalesoftRenderer.SvgImage>
{
    public required int SvgHeight { get; init; }
    public required int SvgWidth { get; init; }
    public required string? OptionalClass { get; init; }

    public new SvgImage Render(BitMatrix matrix, BarcodeFormat format, string content)
    {
        return Render(matrix, format, content, null);
    }

    public new SvgImage Render(
        BitMatrix matrix,
        BarcodeFormat format,
        string content,
        EncodingOptions? options
    )
    {
        var image = new SvgImage(matrix.Width, matrix.Height);
        Create(image, matrix, format, content, options);
        return image;
    }

    private void Create(
        SvgImage image,
        BitMatrix? matrix,
        BarcodeFormat format,
        string content,
        EncodingOptions? options
    )
    {
        if (matrix == null)
            return;
        var width = matrix.Width;
        var height = matrix.Height;
        var num1 = options != null && options.PureBarcode || string.IsNullOrEmpty(content)
            ? 0
            : (format == BarcodeFormat.CODE_39 || format == BarcodeFormat.CODE_93 || format == BarcodeFormat.CODE_128 ||
               format == BarcodeFormat.EAN_13 || format == BarcodeFormat.EAN_8 || format == BarcodeFormat.CODABAR ||
               format == BarcodeFormat.ITF || format == BarcodeFormat.UPC_A || format == BarcodeFormat.UPC_E ||
               format == BarcodeFormat.MSI
                ? 1
                : (format == BarcodeFormat.PLESSEY ? 1 : 0));
        if (num1 != 0)
        {
            var num2 = FontSize < 1 ? 10 : FontSize;
            height += num2 + 3;
        }

        image.AddHeader();
        image.AddTag(SvgWidth, SvgHeight, width, height, Background, Foreground, OptionalClass);
        AppendDarkCell(image, matrix, 0, 0);
        if (num1 != 0)
        {
            var fontName = string.IsNullOrEmpty(FontName) ? "Arial" : FontName;
            var fontSize = FontSize < 1 ? 10 : FontSize;
            content = ModifyContentDependingOnBarcodeFormat(format, content);
            image.AddText(content, fontName, fontSize);
        }

        image.AddEnd();
    }

    private static void AppendDarkCell(
        SvgImage image,
        BitMatrix? matrix,
        int offsetX,
        int offSetY
    )
    {
        if (matrix == null)
            return;
        var width = matrix.Width;
        var height = matrix.Height;
        var processed = new BitMatrix(width, height);
        var flag = false;
        var startPosX = 0;
        var startPosY = 0;
        for (var x = 0; x < width; ++x)
        {
            int endPosX;
            for (var index = 0; index < height; ++index)
            {
                if (!processed[x, index])
                {
                    processed[x, index] = true;
                    if (matrix[x, index])
                    {
                        if (!flag)
                        {
                            startPosX = x;
                            startPosY = index;
                            flag = true;
                        }
                    }
                    else if (flag)
                    {
                        FindMaximumRectangle(matrix, processed, startPosX, startPosY, index, out endPosX);
                        image.AddRec(startPosX + offsetX, startPosY + offSetY, endPosX - startPosX + 1,
                            index - startPosY);
                        flag = false;
                    }
                }
            }

            if (flag)
            {
                FindMaximumRectangle(matrix, processed, startPosX, startPosY, height, out endPosX);
                image.AddRec(startPosX + offsetX, startPosY + offSetY, endPosX - startPosX + 1, height - startPosY);
                flag = false;
            }
        }
    }

    private string ModifyContentDependingOnBarcodeFormat(BarcodeFormat format, string content)
    {
        switch (format)
        {
            case BarcodeFormat.EAN_8:
            case BarcodeFormat.UPC_E:
                if (content.Length < 8)
                    content = OneDimensionalCodeWriter.CalculateChecksumDigitModulo10(content);
                if (content.Length > 4)
                {
                    content = content.Insert(4, "   ");
                }

                break;
            case BarcodeFormat.EAN_13:
                if (content.Length < 13)
                    content = OneDimensionalCodeWriter.CalculateChecksumDigitModulo10(content);
                if (content.Length > 7)
                    content = content.Insert(7, "   ");
                if (content.Length > 1)
                {
                    content = content.Insert(1, "   ");
                }

                break;
            case BarcodeFormat.UPC_A:
                if (content.Length < 12)
                    content = OneDimensionalCodeWriter.CalculateChecksumDigitModulo10(content);
                if (content.Length > 11)
                    content = content.Insert(11, "   ");
                if (content.Length > 6)
                    content = content.Insert(6, "   ");
                if (content.Length > 1)
                {
                    content = content.Insert(1, "   ");
                }

                break;
        }

        return content;
    }

    private static void FindMaximumRectangle(
        BitMatrix matrix,
        BitMatrix processed,
        int startPosX,
        int startPosY,
        int endPosY,
        out int endPosX
    )
    {
        endPosX = startPosX;
        for (var x = startPosX + 1; x < matrix.Width; ++x)
        {
            for (var y = startPosY; y < endPosY; ++y)
            {
                if (!matrix[x, y])
                    return;
            }

            endPosX = x;
            for (var y = startPosY; y < endPosY; ++y)
                processed[x, y] = true;
        }
    }


    public new class SvgImage
    {
        private readonly StringBuilder m_content;

        /// <summary>Gets or sets the content.</summary>
        /// <value>The content.</value>
        public string? Content
        {
            get => m_content.ToString();
            set
            {
                m_content.Length = 0;
                if (value == null)
                    return;
                m_content.Append(value);
            }
        }

        /// <summary>The original height of the bitmatrix for the barcode</summary>
        public int Height { get; set; }

        /// <summary>The original width of the bitmatrix for the barcode</summary>
        public int Width { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:ZXing.Rendering.SvgRenderer.SvgImage" /> class.
        /// </summary>
        public SvgImage() => m_content = new StringBuilder();

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:ZXing.Rendering.SvgRenderer.SvgImage" /> class.
        /// </summary>
        public SvgImage(int width, int height)
        {
            m_content = new StringBuilder();
            Width = width;
            Height = height;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:ZXing.Rendering.SvgRenderer.SvgImage" /> class.
        /// </summary>
        /// <param name="content">The content.</param>
        public SvgImage(string content) => m_content = new StringBuilder(content);

        /// <summary>Gives the XML representation of the SVG image</summary>
        public override string ToString() => m_content.ToString();

        public void AddHeader()
        {
            // Including a header would make the resulting HTML invalid
        }

        public void AddEnd() => m_content.Append("</svg>");

        public void AddTag(
            int displaysizeX,
            int displaysizeY,
            int viewboxSizeX,
            int viewboxSizeY,
            Color background,
            Color fill,
            string? optionalClass = null
        )
        {
            m_content.Append(
                $"<svg xmlns=\"http://www.w3.org/2000/svg\" version=\"1.2\" baseProfile=\"tiny\" shape-rendering=\"crispEdges\" viewBox=\"0 0 {viewboxSizeX} {viewboxSizeY}\" viewport-fill=\"rgb({GetColorRgb(background)})\" viewport-fill-opacity=\"{ConvertAlpha(background)}\" fill=\"rgb({GetColorRgb(fill)})\" fill-opacity=\"{ConvertAlpha(fill)}\" {GetBackgroundStyle(background)} {(displaysizeX > 0 ? $"width='{displaysizeX}'" : "")} {(displaysizeY > 0 ? $"height='{displaysizeY}'" : "")} {(optionalClass != null ? $"class='{optionalClass}'" : "")}>");
        }

        public void AddText(string text, string fontName, int fontSize)
        {
            m_content.AppendFormat(CultureInfo.InvariantCulture,
                "<text x=\"50%\" y=\"98%\" style=\"font-family: {0}; font-size: {1}px\" text-anchor=\"middle\">{2}</text>",
                fontName, fontSize, text);
        }

        public void AddRec(int posX, int posY, int width, int height)
        {
            m_content.AppendFormat(CultureInfo.InvariantCulture,
                "<rect x=\"{0}\" y=\"{1}\" width=\"{2}\" height=\"{3}\"/>", posX, posY, width,
                height);
        }

        private static double ConvertAlpha(Color alpha)
        {
            return Math.Round(alpha.A / (double)byte.MaxValue, 2);
        }

        private static string GetBackgroundStyle(Color color)
        {
            var num = ConvertAlpha(color);
            return string.Format(
                "style=\"background-color:rgb({0},{1},{2});background-color:rgba({0}, {1}, {2}, {3});\"",
                color.R, color.G, color.B, num);
        }

        private static string GetColorRgb(Color color)
        {
            return $"{color.R.ToString()},{color.G.ToString()},{color.B.ToString()}";
        }
    }
}