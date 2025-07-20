using CapitalNerd.Laranjinhai.Domain.Entities;

namespace CapitalNerd.Laranjinhai.Domain.Interfaces
{
    public interface IEmpresaRepository
    {
        Task<Empresa?> GetByIdAsync(Guid id);
        Task CreateAsync(Empresa entity);
        Task UpdateAsync(Empresa entity);
        Task DeleteAsync(Guid id);
        IQueryable<Empresa> Query();
    }
}
