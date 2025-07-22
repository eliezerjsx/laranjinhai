using CapitalNerd.Laranjinhai.Domain.Commons;
using System.Linq.Expressions;

namespace CapitalNerd.Laranjinhai.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id, Func<IQueryable<T>, IQueryable<T>>? include = null);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>?> GetAllAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> Query();
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
        Task<T?> FirstAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
        Task<PagedResult<T>> GetPagedAsync(
            Expression<Func<T, bool>>? predicate = null,
            int page = 1,
            int pageSize = 10,
            Func<IQueryable<T>, IQueryable<T>>? include = null);
    }

}
