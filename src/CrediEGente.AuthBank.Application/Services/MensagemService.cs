using CapitalNerd.Laranjinhai.Application.Interfaces;
using CapitalNerd.Laranjinhai.Domain.Entities;
using CapitalNerd.Laranjinhai.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CapitalNerd.Laranjinhai.Application.Services
{
    public class MensagemService : IMensagemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMensagemRepository _repository;
        private readonly ILogger<MensagemService> _logger;

        public MensagemService(
            IUnitOfWork unitOfWork,
            IMensagemRepository repository,
            ILogger<MensagemService> logger)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _logger = logger;
        }

        public async Task SalvarMensagemRecebidaAsync(JsonElement json)
        {
            try
            {
                var telefone = json.GetProperty("from").GetString();
                var conteudo = json.GetProperty("body").GetString();
                var tipo = json.GetProperty("type").GetString();
                var direcao = tipo == "chat" ? "Recebida" : "Enviada";

                if (string.IsNullOrWhiteSpace(telefone) || string.IsNullOrWhiteSpace(conteudo))
                {
                    _logger.LogWarning("Dados insuficientes para salvar a mensagem. Telefone: {Telefone}, Conteúdo: {Conteudo}", telefone, conteudo);
                    return;
                }

                var clienteRepository = _unitOfWork.Repository<Cliente>();
                var cliente = await clienteRepository.Query()
                    .FirstOrDefaultAsync(c => c.Telefone == telefone);

                if (cliente == null)
                {
                    _logger.LogWarning("Cliente não encontrado para o telefone: {Telefone}", telefone);
                    return;
                }

                var mensagem = new Mensagem
                {
                    Id = Guid.NewGuid(),
                    TenantId = cliente.TenantId,
                    ClienteId = cliente.Id,
                    Conteudo = conteudo,
                    Direcao = direcao,
                    CriadoEm = DateTime.UtcNow
                };

                await _repository.CreateAsync(mensagem);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation("📥 Mensagem persistida com sucesso. ID: {Id}", mensagem.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar mensagem recebida.");
                throw;
            }
        }
    }
}
