namespace Scalesoft.DisplayTool.Renderer.Validators;

public class DocumentValidatorProvider
{
    private readonly List<IDocumentValidator> m_documentValidators;

    public DocumentValidatorProvider(IEnumerable<IDocumentValidator> documentValidators)
    {
        m_documentValidators = documentValidators.ToList();
    }

    public IDocumentValidator GetValidator(InputFormat inputFormat)
    {
        return m_documentValidators.First(x => x.InputFormat == inputFormat);
    }
}