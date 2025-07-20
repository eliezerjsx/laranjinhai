using System.Text.Json;

namespace CapitalNerd.Laranjinhai.Application.Interfaces
{
    public interface IMensagemService
    {
        Task SalvarMensagemRecebidaAsync(JsonElement mensagem);
    }
}
