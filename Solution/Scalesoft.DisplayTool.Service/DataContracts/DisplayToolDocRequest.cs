using System.ComponentModel.DataAnnotations;

namespace Scalesoft.DisplayTool.Service.DataContracts;

public class DisplayToolDocRequest
{
    [Required]
    public required byte[] FileContent { get; set; }

    [Required]
    public InputFormatContract? InputFormat { get; set; }

    [Required]
    public OutputFormatContract? OutputFormat { get; set; }
}