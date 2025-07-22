using System.Collections.Concurrent;
using CapitalNerd.Laranjinhai.Application.Interfaces;
using CapitalNerd.Laranjinhai.Domain.Entities;
using CapitalNerd.Laranjinhai.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CapitalNerd.Laranjinhai.Application.Commands
{
    public class ProcessarMensagemCommandHandler : IRequestHandler<ProcessarMensagemCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProcessarMensagemCommandHandler> _logger;
        private readonly IOpenAIService _openAIService;
        private readonly IWhatsAppSender _whatsAppSender;

        // Controle de concorrência por telefone
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new();

        public ProcessarMensagemCommandHandler(
            IUnitOfWork unitOfWork,
            ILogger<ProcessarMensagemCommandHandler> logger,
            IOpenAIService openAIService,
            IWhatsAppSender whatsAppSender)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _openAIService = openAIService;
            _whatsAppSender = whatsAppSender;
        }

        public async Task Handle(ProcessarMensagemCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Mensagem;

            if (string.IsNullOrWhiteSpace(dto.From) || string.IsNullOrWhiteSpace(dto.Body))
            {
                _logger.LogWarning("⚠️ Mensagem ignorada: From ou Body vazio. From: {From}, Body: {Body}", dto.From, dto.Body);
                return;
            }

            var sem = _locks.GetOrAdd(dto.From, _ => new SemaphoreSlim(1, 1));
            await sem.WaitAsync(cancellationToken);

            try
            {
                var clienteRepo = _unitOfWork.Repository<Cliente>();

                var cliente = await clienteRepo.Query()
                    .FirstOrDefaultAsync(c => c.Telefone == dto.From, cancellationToken);

                if (cliente is null)
                {
                    cliente = new Cliente
                    {
                        Id = Guid.NewGuid(),
                        TenantId = Guid.Parse("a86f1c8e-d8b4-4c1c-b2fc-98b944a9eb8c"),
                        Nome = string.IsNullOrWhiteSpace(dto.NotifyName) ? dto.From : dto.NotifyName,
                        Telefone = dto.From,
                        CriadoEm = DateTime.UtcNow
                    };

                    await clienteRepo.AddAsync(cliente);
                    _logger.LogInformation("🆕 Cliente criado: {Telefone}", cliente.Telefone);
                }

                var mensagemRepo = _unitOfWork.Repository<Mensagem>();
                var mensagemRecebida = new Mensagem
                {
                    Id = Guid.NewGuid(),
                    TenantId = cliente.TenantId,
                    ClienteId = cliente.Id,
                    Conteudo = dto.Body,
                    Direcao = "Recebida",
                    CriadoEm = DateTime.UtcNow
                };

                await mensagemRepo.AddAsync(mensagemRecebida);
                await _unitOfWork.CommitAsync(cancellationToken);

                _logger.LogInformation("📥 Mensagem registrada. ClienteId: {ClienteId}, Conteúdo: {Conteudo}", cliente.Id, mensagemRecebida.Conteudo);

                // 🧠 Obter resposta com OpenAI
                var resposta = await _openAIService.ObterRespostaAsync(dto.Body, cancellationToken);
                _logger.LogInformation("🤖 Resposta IA: {Resposta}", resposta);

                // 📤 Enviar resposta via WhatsApp
                await _whatsAppSender.EnviarMensagemAsync(dto.From, cliente.Id, resposta, request.Mensagem.Session, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Erro ao processar mensagem de {Telefone}", dto.From);
            }
            finally
            {
                sem.Release();
            }
        }
    }
}
