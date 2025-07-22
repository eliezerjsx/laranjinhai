using CapitalNerd.Laranjinhai.Application.Dtos;

namespace CapitalNerd.Laranjinhai.Application.Interfaces
{
    public interface IProcessarMensagemCommand
    {
        Task ExecuteAsync(WebhookMessageDto dto);
    }
}
