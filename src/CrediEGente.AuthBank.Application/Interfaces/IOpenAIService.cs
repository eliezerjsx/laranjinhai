namespace CapitalNerd.Laranjinhai.Application.Interfaces
{
    public interface IOpenAIService
    {
        Task<string> ObterRespostaAsync(string pergunta, CancellationToken cancellationToken = default);
    }
}