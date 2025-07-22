using CapitalNerd.Laranjinhai.Domain.Entities;
using CapitalNerd.Laranjinhai.Domain.Interfaces;
using CapitalNerd.Laranjinhai.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CapitalNerd.Laranjinhai.Infrastructure.Repositories
{
    public class SessaoWhatsappRepository : ISessaoWhatsappRepository
    {
        private readonly LaranjinhaiDbContext _context;

        public SessaoWhatsappRepository(LaranjinhaiDbContext context)
        {
            _context = context;
        }

        public async Task<SessaoWhatsapp?> GetByIdAsync(Guid id)
        {
            return await _context.SessoesWhatsapp.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task CreateAsync(SessaoWhatsapp entity)
        {
            await _context.SessoesWhatsapp.AddAsync(entity);
        }

        public async Task UpdateAsync(SessaoWhatsapp entity)
        {
            _context.SessoesWhatsapp.Update(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);

            if (entity is null)
                throw new KeyNotFoundException("SessaoWhatsapp não encontrado");

            _context.SessoesWhatsapp.Remove(entity);
        }

        public IQueryable<SessaoWhatsapp> Query() => _context.SessoesWhatsapp.AsQueryable();
    }
}
