using CapitalNerd.Laranjinhai.Domain.Interfaces;
using CapitalNerd.Laranjinhai.Infrastructure.Data;
using CapitalNerd.Laranjinhai.Infrastructure.Repositories;

namespace CapitalNerd.Laranjinhai.Infrastructure.UnitOfWork
{
    public class UnitOfWork(LaranjinhaiDbContext context) : IUnitOfWork
    {
        private readonly Dictionary<Type, object> _repositories = [];

        public IRepository<T> Repository<T>() where T : class, IIdentifiable
        {
            var type = typeof(T);

            if (!_repositories.TryGetValue(type, out object? value))
            {
                var repoInstance = new Repository<T>(context);
                value = repoInstance;

                _repositories.Add(type, value);
            }

            return (IRepository<T>)value;
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    => await context.SaveChangesAsync(cancellationToken);

        public async Task<int> CommitAsync()
            => await context.SaveChangesAsync();

        public void Dispose()
        {
            context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
