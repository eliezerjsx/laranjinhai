using CapitalNerd.Laranjinhai.Domain.Interfaces;

namespace CapitalNerd.Laranjinhai.Domain.Entities
{
    public class Operador : IIdentifiable
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public string Nome { get; set; } = default!;
        public string? Email { get; set; }
        public bool Ativo { get; set; } = true;
        public DateTime? CriadoEm { get; set; }

        public ICollection<Resposta> Respostas { get; set; } = new List<Resposta>();
    }
}
