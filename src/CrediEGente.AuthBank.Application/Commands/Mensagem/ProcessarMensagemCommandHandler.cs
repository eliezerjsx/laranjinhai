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

                var clienteRepo = _unitOfWork.Repository<Cliente>();

                var cliente = await clienteRepo.Query()
                    .FirstOrDefaultAsync(c => c.Telefone == dto.From, cancellationToken);

                if (cliente is null)
                {
                    cliente = new Cliente
                    {
                        Id = Guid.NewGuid(),
                        TenantId = Guid.NewGuid(), 
                        Nome = dto.From, 
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
                    TenantId = cliente.TenantId,
                    ClienteId = cliente.Id,
                    Conteudo = dto.Body,
                    Direcao = dto.Type == "chat" ? "Recebida" : "Enviada",
                    CriadoEm = DateTime.UtcNow
                };

                await mensagemRepo.AddAsync(mensagem);
                await _unitOfWork.CommitAsync(cancellationToken);

                _logger.LogInformation("📥 Mensagem registrada com sucesso. ClienteId: {ClienteId}, Conteúdo: {Conteudo}", cliente.Id, mensagem.Conteudo);
            }
        }
    }


