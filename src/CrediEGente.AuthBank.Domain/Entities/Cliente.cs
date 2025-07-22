
using CapitalNerd.Laranjinhai.Domain.Interfaces;

namespace CapitalNerd.Laranjinhai.Domain.Entities
{
    public class Cliente : IIdentifiable
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public string Nome { get; set; } = default!;
        public string Telefone { get; set; } = default!;
        public DateTime? CriadoEm { get; set; }

        public ICollection<Mensagem> Mensagens { get; set; } = new List<Mensagem>();
    }
}