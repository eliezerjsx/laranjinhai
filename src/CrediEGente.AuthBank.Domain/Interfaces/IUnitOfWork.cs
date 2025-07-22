namespace CapitalNerd.Laranjinhai.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> Repository<T>() where T : class, IIdentifiable;
        Task<int> CommitAsync();
        Task<int> CommitAsync(CancellationToken cancellationToken = default);
    }
}
