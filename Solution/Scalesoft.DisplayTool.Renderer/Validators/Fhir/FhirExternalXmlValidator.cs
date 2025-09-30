using Scalesoft.DisplayTool.Renderer.Clients.FhirValidator;

namespace Scalesoft.DisplayTool.Renderer.Validators.Fhir;

public class FhirExternalXmlValidator : FhirExternalValidatorBase
{
    public FhirExternalXmlValidator(FhirValidatorClient client) : base(InputFormat.FhirXml, client)
    {
    }
}