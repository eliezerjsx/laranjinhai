using CapitalNerd.Laranjinhai.Domain.Interfaces;

namespace CapitalNerd.Laranjinhai.Domain.Entities
{
    public class SessaoWhatsapp : IIdentifiable
    {
        public Guid Id { get; set; }
        public Guid EmpresaId { get; set; }
        public string Status { get; set; } = "aguardando_qr";
        public string? QrCodeBase64 { get; set; }
        public string? NumeroConectado { get; set; }
        public DateTime? UltimaAtualizacao { get; set; }
        public DateTime? CriadoEm { get; set; }

        public Empresa Empresa { get; set; } = default!;
    }
}
