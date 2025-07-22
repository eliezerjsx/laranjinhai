using CapitalNerd.Laranjinhai.Domain.Entities;

namespace CapitalNerd.Laranjinhai.Domain.Interfaces
{
    public interface IRespostaRepository
    {
        Task<Resposta?> GetByIdAsync(Guid id);
        Task CreateAsync(Resposta entity);
        Task UpdateAsync(Resposta entity);
        Task DeleteAsync(Guid id);
        IQueryable<Resposta> Query();
    }
}
