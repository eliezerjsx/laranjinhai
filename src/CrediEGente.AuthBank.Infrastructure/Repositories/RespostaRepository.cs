using CapitalNerd.Laranjinhai.Domain.Entities;
using CapitalNerd.Laranjinhai.Domain.Interfaces;
using CapitalNerd.Laranjinhai.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CapitalNerd.Laranjinhai.Infrastructure.Repositories
{
    public class RespostaRepository : IRespostaRepository
    {
        private readonly LaranjinhaiDbContext _context;

        public RespostaRepository(LaranjinhaiDbContext context)
        {
            _context = context;
        }

        public async Task<Resposta?> GetByIdAsync(Guid id)
        {
            return await _context.Respostas.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task CreateAsync(Resposta entity)
        {
            await _context.Respostas.AddAsync(entity);
        }

        public Task UpdateAsync(Resposta entity)
        {
            _context.Respostas.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);

            if (entity is null)
                throw new KeyNotFoundException("Resposta não encontrado");

            _context.Respostas.Remove(entity);
        }

        public IQueryable<Resposta> Query() => _context.Respostas.AsQueryable();
    }
}
