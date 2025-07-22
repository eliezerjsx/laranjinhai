using CapitalNerd.Laranjinhai.Domain.Entities;

namespace CapitalNerd.Laranjinhai.Domain.Interfaces
{
    public interface IMensagemRepository
    {
        Task<Mensagem?> GetByIdAsync(Guid id);
        Task CreateAsync(Mensagem entity);
        Task UpdateAsync(Mensagem entity);
        Task DeleteAsync(Guid id);
        IQueryable<Mensagem> Query();
    }
}
