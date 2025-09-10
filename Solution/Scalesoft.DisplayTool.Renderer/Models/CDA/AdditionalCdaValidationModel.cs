using Scalesoft.DisplayTool.Renderer.Clients.CdaValidator.Contracts;

namespace Scalesoft.DisplayTool.Renderer.Models.CDA;

public class AdditionalCdaValidationModel
{
    public required string DocumentWellFormed { get; init; }
    public required DocumentValidXSD DocumentXsdValid { get; init; }
}