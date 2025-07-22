using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapitalNerd.Laranjinhai.Application.Settings
{
    public class WhatsAppSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public string WebhookUrl { get; set; } = string.Empty;
        public bool WaitQrCode { get; set; } = true;
    }
}
