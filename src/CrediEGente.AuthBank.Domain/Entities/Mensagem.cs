using CapitalNerd.Laranjinhai.Domain.Interfaces;

namespace CapitalNerd.Laranjinhai.Domain.Entities
{
    public class Mensagem : IIdentifiable    
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public Guid ClienteId { get; set; }
        public string Conteudo { get; set; } = default!;
        public string Direcao { get; set; } = default!;
        public bool? RespondidoPorIa { get; set; }
        public bool? RespondidoPorOperador { get; set; }
        public DateTime? CriadoEm { get; set; }

        public Cliente Cliente { get; set; } = default!;
        public ICollection<Resposta> Respostas { get; set; } = new List<Resposta>();
    }
}
