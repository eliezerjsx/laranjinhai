using System.Text.Json;

namespace CapitalNerd.Laranjinhai.Application.Dtos
{
    public class WebhookMessageDto
    {
        public string From { get; set; } = default!;
        public string To { get; set; } = default!;
        public string Body { get; set; } = default!;
        public string Type { get; set; } = default!;
        public string? NotifyName { get; set; }
        public JsonElement RawJson { get; set; }  
    }

}
