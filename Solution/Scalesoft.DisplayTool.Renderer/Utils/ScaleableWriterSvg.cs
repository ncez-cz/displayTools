using ZXing;

namespace Scalesoft.DisplayTool.Renderer.Utils;

public class ScaleableWriterSvg : BarcodeWriter<ScalesoftRenderer.SvgImage>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="T:ZXing.BarcodeWriterSvg" /> class.
    /// </summary>
    public ScaleableWriterSvg(int svgWidth, int svgHeight, string? optionalClass = null)
    {
        Renderer = new ScalesoftRenderer
        {
            SvgWidth = svgWidth,
            SvgHeight = svgHeight,
            OptionalClass = optionalClass,
        };
    }
}