using CapitalNerd.Laranjinhai.Domain.Interfaces;
using CapitalNerd.Laranjinhai.Infrastructure.Data;
using CapitalNerd.Laranjinhai.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CapitalNerd.Laranjinhai.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LaranjinhaiDbContext _context;
        private readonly Dictionary<Type, object> _repositories = [];

        public UnitOfWork(LaranjinhaiDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IRepository<T> Repository<T>() where T : class, IIdentifiable
        {
            var type = typeof(T);

            if (!_repositories.TryGetValue(type, out var value))
            {
                var repoInstance = new Repository<T>(_context);
                value = repoInstance;
                _repositories.Add(type, value);
            }

            return (IRepository<T>)value;
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            var pendingChanges = _context.ChangeTracker.Entries().Where(e => e.State != EntityState.Unchanged).ToList();

            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
