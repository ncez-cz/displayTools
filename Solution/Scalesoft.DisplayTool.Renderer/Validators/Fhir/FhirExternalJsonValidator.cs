using Microsoft.Extensions.Logging;
using Scalesoft.DisplayTool.Renderer.Clients.FhirValidator;

namespace Scalesoft.DisplayTool.Renderer.Validators.Fhir;

public class FhirExternalJsonValidator : FhirExternalValidatorBase
{
    public FhirExternalJsonValidator(ILogger<FhirValidatorClient> logger) : base(logger, InputFormat.FhirJson)
    {
    }
}