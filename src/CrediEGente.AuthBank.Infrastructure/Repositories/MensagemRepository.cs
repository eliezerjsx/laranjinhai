using CapitalNerd.Laranjinhai.Domain.Entities;
using CapitalNerd.Laranjinhai.Domain.Interfaces;
using CapitalNerd.Laranjinhai.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CapitalNerd.Laranjinhai.Infrastructure.Repositories
{
    public class MensagemRepository : IMensagemRepository
    {
        private readonly LaranjinhaiDbContext _context;

        public MensagemRepository(LaranjinhaiDbContext context)
        {
            _context = context;
        }

        public async Task<Mensagem?> GetByIdAsync(Guid id)
        {
            return await _context.Mensagens.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task CreateAsync(Mensagem entity)
        {
            await _context.Mensagens.AddAsync(entity);
        }

        public Task UpdateAsync(Mensagem entity)
        {
            _context.Mensagens.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);

            if (entity is null)
                throw new KeyNotFoundException("Mensagem não encontrada");

            _context.Mensagens.Remove(entity);
            await Task.CompletedTask;
        }

        public IQueryable<Mensagem> Query() => _context.Mensagens.AsQueryable();
    }
}
