using System;

namespace CapitalNerd.Laranjinhai.Application.Dtos
{
    public record MensagemGetDto(
        Guid Id,
        Guid TenantId,
        Guid ClienteId,
        string Conteudo,
        string Direcao,
        bool? RespondidoPorIa,
        bool? RespondidoPorOperador,
        DateTime? CriadoEm
    )
    {
        public static implicit operator MensagemGetDto(Domain.Entities.Mensagem entity)
            => new(
                entity.Id,
                entity.TenantId,
                entity.ClienteId,
                entity.Conteudo,
                entity.Direcao,
                entity.RespondidoPorIa,
                entity.RespondidoPorOperador,
                entity.CriadoEm
            );
    }
}
