using CapitalNerd.Laranjinhai.Domain.Interfaces;

namespace CapitalNerd.Laranjinhai.Domain.Entities
{
    public class Resposta : IIdentifiable
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public Guid MensagemId { get; set; }
        public Guid? OperadorId { get; set; }
        public string Conteudo { get; set; } = default!;
        public DateTime? CriadoEm { get; set; }

        public Mensagem Mensagem { get; set; } = default!;
        public Operador? Operador { get; set; }
    }
}
