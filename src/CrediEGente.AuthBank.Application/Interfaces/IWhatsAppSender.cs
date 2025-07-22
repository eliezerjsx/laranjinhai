using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapitalNerd.Laranjinhai.Application.Interfaces
{
    public interface IWhatsAppSender
    {
        Task EnviarMensagemAsync(string telefone, Guid clienteId, string mensagem, string session, CancellationToken cancellationToken = default);
    }
}
