using Scalesoft.DisplayTool.Renderer.Clients.FhirValidator;

namespace Scalesoft.DisplayTool.Renderer.Validators.Fhir;

public class FhirExternalJsonValidator : FhirExternalValidatorBase
{
    public FhirExternalJsonValidator(FhirValidatorClient client) : base(InputFormat.FhirJson, client)
    {
    }
}