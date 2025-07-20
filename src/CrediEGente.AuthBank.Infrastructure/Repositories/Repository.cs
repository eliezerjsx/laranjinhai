using CapitalNerd.Laranjinhai.Domain.Interfaces;
using CapitalNerd.Laranjinhai.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using CapitalNerd.Laranjinhai.Domain.Commons;

namespace CapitalNerd.Laranjinhai.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, IIdentifiable
    {
        protected readonly LaranjinhaiDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(LaranjinhaiDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(Guid id, Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            IQueryable<T> query = _dbSet;

            if (include != null)
                query = include(query);

            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _dbSet.ToListAsync();

        public async Task<IEnumerable<T>?> GetAllAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.Where(predicate)?.ToListAsync();

        public async Task<T?> FirstAsync(Expression<Func<T, bool>> predicate) 
            => await _context.Set<T>().FirstOrDefaultAsync(predicate);
        
        public IQueryable<T> Query()
            => _dbSet.AsQueryable();

        public async Task AddAsync(T entity)
            => await _dbSet.AddAsync(entity);
        
        public void Update(T entity)
            => _dbSet.Update(entity);
        
        public void Remove(T entity)
            => _dbSet.Remove(entity);

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.CountAsync(predicate);

        public async Task<PagedResult<T>> GetPagedAsync(
            Expression<Func<T, bool>>? predicate = null,
            int page = 1,
            int pageSize = 10,
            Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            var query = _context.Set<T>().AsQueryable();

            if (include != null)
                query = include(query);

            if (predicate != null)
                query = query.Where(predicate);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
