using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using CapitalNerd.Laranjinhai.Application.Interfaces;
using CapitalNerd.Laranjinhai.Domain.Entities;
using CapitalNerd.Laranjinhai.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace CapitalNerd.Laranjinhai.Application.Services
{
    public class WhatsAppSender : IWhatsAppSender
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WhatsAppSender> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISessaoManager _sessaoManager;
        public WhatsAppSender(
            HttpClient httpClient,
            ILogger<WhatsAppSender> logger,
            IUnitOfWork unitOfWork,
            ISessaoManager sessaoManager)
        {
            _httpClient = httpClient;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _sessaoManager = sessaoManager;
        }

        public async Task EnviarMensagemAsync(string telefone, Guid clienteId, string mensagem, string session, CancellationToken cancellationToken = default)
        {
            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://wppconnect-production-1747.up.railway.app/api/" + session + "/send-message");


                requestMessage.Content = JsonContent.Create(new
                {
                    phone = telefone,
                    message = mensagem,
                    isGroup = false
                });
               

                var token = await _sessaoManager.GetTokenAsync(session);
                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);                

                var response = await _httpClient.SendAsync(requestMessage, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                    _logger.LogWarning("Erro ao enviar mensagem para {Telefone}. Status: {StatusCode}. Erro: {Erro}", telefone, response.StatusCode, errorContent);
                }
                else
                {
                    _logger.LogInformation("📤 Mensagem enviada com sucesso para {Telefone}", telefone);
                }

                // Persistência
                var mensagemRepo = _unitOfWork.Repository<Mensagem>();
                var mensagemEnviada = new Mensagem
                {
                    Id = Guid.NewGuid(),
                    TenantId = Guid.Parse("a86f1c8e-d8b4-4c1c-b2fc-98b944a9eb8c"),
                    ClienteId = clienteId,
                    Conteudo = mensagem,
                    Direcao = "Enviada",
                    CriadoEm = DateTime.UtcNow
                };

                await mensagemRepo.AddAsync(mensagemEnviada);
                await _unitOfWork.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao enviar e registrar mensagem para {Telefone}", telefone);
            }
        }

    }
}
