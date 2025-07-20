namespace CapitalNerd.Laranjinhai.Application.Settings
{
    public class KeycloakSettings
    {
        public string Authority { get; set; } = string.Empty;
        public List<string> Audiences { get; set; } = new();
        public bool RequireHttpsMetadata { get; set; } = true;
    }
}
