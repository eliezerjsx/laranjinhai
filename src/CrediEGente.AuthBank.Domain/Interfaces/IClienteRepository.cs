using CapitalNerd.Laranjinhai.Domain.Entities;

namespace CapitalNerd.Laranjinhai.Domain.Interfaces
{
    public interface IClienteRepository
    {
        Task<Cliente?> GetByIdAsync(Guid id);
        Task CreateAsync(Cliente entity);
        Task UpdateAsync(Cliente entity);
        Task DeleteAsync(Guid id);
        IQueryable<Cliente> Query();
    }
}
