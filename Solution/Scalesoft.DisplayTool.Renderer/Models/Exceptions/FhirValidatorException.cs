namespace Scalesoft.DisplayTool.Renderer.Models.Exceptions;

public class FhirValidatorException : Exception
{
    public FhirValidatorException()
    {
    }

    public FhirValidatorException(string message) : base(message)
    {
    }
}