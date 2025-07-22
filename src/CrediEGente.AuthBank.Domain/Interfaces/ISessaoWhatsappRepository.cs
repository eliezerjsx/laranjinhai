using CapitalNerd.Laranjinhai.Domain.Entities;

namespace CapitalNerd.Laranjinhai.Domain.Interfaces
{
    public interface ISessaoWhatsappRepository
    {
        Task<SessaoWhatsapp?> GetByIdAsync(Guid id);
        Task CreateAsync(SessaoWhatsapp entity);
        Task UpdateAsync(SessaoWhatsapp entity);
        Task DeleteAsync(Guid id);
        IQueryable<SessaoWhatsapp> Query();
    }
}
