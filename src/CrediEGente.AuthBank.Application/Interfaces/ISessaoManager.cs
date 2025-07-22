namespace CapitalNerd.Laranjinhai.Application.Interfaces
{
    public interface ISessaoManager
    {
        Task SetTokenAsync(string sessionName, string token, CancellationToken cancellationToken = default);
        Task<string?> GetTokenAsync(string sessionName, CancellationToken cancellationToken = default);
    }

}
