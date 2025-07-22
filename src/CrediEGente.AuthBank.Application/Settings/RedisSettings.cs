namespace CapitalNerd.Laranjinhai.Application.Settings
{
    public class RedisSettings
    {
        public string Host { get; set; } = default!;
        public int Port { get; set; }
        public string Password { get; set; } = default!;
        public bool Ssl { get; set; } = true;
        public bool AbortConnect { get; set; } = false;
    }

}
