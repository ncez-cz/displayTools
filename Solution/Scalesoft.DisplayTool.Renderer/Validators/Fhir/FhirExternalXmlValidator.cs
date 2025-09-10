using Microsoft.Extensions.Logging;
using Scalesoft.DisplayTool.Renderer.Clients.FhirValidator;

namespace Scalesoft.DisplayTool.Renderer.Validators.Fhir;

public class FhirExternalXmlValidator : FhirExternalValidatorBase
{
    public FhirExternalXmlValidator(ILogger<FhirValidatorClient> logger) : base(logger, InputFormat.FhirXml)
    {
    }
}