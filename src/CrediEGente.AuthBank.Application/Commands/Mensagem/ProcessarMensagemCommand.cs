using CapitalNerd.Laranjinhai.Application.Dtos;
using MediatR;

namespace CapitalNerd.Laranjinhai.Application.Commands
{
    public class ProcessarMensagemCommand : IRequest
    {
        public WebhookMessageDto Mensagem { get; set; }

        public ProcessarMensagemCommand(WebhookMessageDto mensagem)
        {
            Mensagem = mensagem;
        }
    }
}