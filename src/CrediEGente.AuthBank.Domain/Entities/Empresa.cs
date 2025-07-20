using CapitalNerd.Laranjinhai.Domain.Interfaces;

namespace CapitalNerd.Laranjinhai.Domain.Entities
{
    public class Empresa : IIdentifiable
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = default!;
        public string? EmailResponsavel { get; set; }
        public string? TelefoneContato { get; set; }
        public bool Ativo { get; set; } = true;
        public DateTime? CriadoEm { get; set; }

        public ICollection<SessaoWhatsapp> SessoesWhatsapp { get; set; } = new List<SessaoWhatsapp>();
    }
}
