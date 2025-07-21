using CapitalNerd.Laranjinhai.Domain.Entities;
using CapitalNerd.Laranjinhai.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace CapitalNerd.Laranjinhai.Application.Commands
{
    public class ProcessarMensagemCommandHandler : IRequestHandler<ProcessarMensagemCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProcessarMensagemCommandHandler> _logger;

        // Controle de concorrência por número de telefone
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new();

        public ProcessarMensagemCommandHandler(
            IUnitOfWork unitOfWork,
            ILogger<ProcessarMensagemCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(ProcessarMensagemCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Mensagem;

            if (string.IsNullOrWhiteSpace(dto.From) || string.IsNullOrWhiteSpace(dto.Body))
            {
                _logger.LogWarning("Mensagem ignorada por falta de dados. From: {From}, Body: {Body}", dto.From, dto.Body);
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
                var mensagem = new Mensagem
                {
                    Id = Guid.NewGuid(),
                    TenantId = Guid.Parse("a86f1c8e-d8b4-4c1c-b2fc-98b944a9eb8c"),
                    ClienteId = cliente.Id,
                    Conteudo = dto.Body,
                    Direcao = dto.Type == "chat" ? "Recebida" : "Enviada",
                    CriadoEm = DateTime.UtcNow
                };

                await mensagemRepo.AddAsync(mensagem);
                await _unitOfWork.CommitAsync(cancellationToken);

                _logger.LogInformation("📥 Mensagem registrada com sucesso. ClienteId: {ClienteId}, Conteúdo: {Conteudo}", cliente.Id, mensagem.Conteudo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Erro ao processar mensagem do cliente {Telefone}", dto.From);
            }
            finally
            {
                sem.Release();
            }
        }
    }
}
