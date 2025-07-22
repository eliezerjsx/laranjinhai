using CapitalNerd.Laranjinhai.Application.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace CapitalNerd.Laranjinhai.Application.Services
{
    public class SessaoManager : ISessaoManager
    {
        private readonly IDistributedCache _cache;

        public SessaoManager(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task SetTokenAsync(string sessionName, string token, CancellationToken cancellationToken = default)
        {
            var key = GetKey(sessionName);

            await _cache.SetStringAsync(key, token, cancellationToken);
        }

        public async Task<string?> GetTokenAsync(string sessionName, CancellationToken cancellationToken = default)
        {
            var key = GetKey(sessionName);
            return await _cache.GetStringAsync(key, cancellationToken);
        }

        public async Task RemoverTokenAsync(string sessionName, CancellationToken cancellationToken = default)
        {
            var key = GetKey(sessionName);
            await _cache.RemoveAsync(key, cancellationToken);
        }

        private static string GetKey(string sessionName) => $"wpp:token:{sessionName}";
    }
}
