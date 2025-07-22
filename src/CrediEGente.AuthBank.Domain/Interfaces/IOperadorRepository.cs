using CapitalNerd.Laranjinhai.Domain.Entities;

namespace CapitalNerd.Laranjinhai.Domain.Interfaces
{
    public interface IOperadorRepository
    {
        Task<Operador?> GetByIdAsync(Guid id);
        Task CreateAsync(Operador entity);
        Task UpdateAsync(Operador entity);
        Task DeleteAsync(Guid id);
        IQueryable<Operador> Query();
    }
}
