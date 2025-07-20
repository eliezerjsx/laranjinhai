namespace CapitalNerd.Laranjinhai.Application.Dtos
{
    public class MensagemPostDto
    {
        public Guid TenantId { get; set; }
        public Guid ClienteId { get; set; }
        public string Conteudo { get; set; } = default!;
        public string Direcao { get; set; } = default!;
    }
}
