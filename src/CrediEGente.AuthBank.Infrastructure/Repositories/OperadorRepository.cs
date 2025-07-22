using CapitalNerd.Laranjinhai.Domain.Entities;
using CapitalNerd.Laranjinhai.Domain.Interfaces;
using CapitalNerd.Laranjinhai.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CapitalNerd.Laranjinhai.Infrastructure.Repositories
{
    public class OperadorRepository : IOperadorRepository
    {
        private readonly LaranjinhaiDbContext _context;

        public OperadorRepository(LaranjinhaiDbContext context)
        {
            _context = context;
        }

        public async Task<Operador?> GetByIdAsync(Guid id)
        {
            return await _context.Operadores.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task CreateAsync(Operador entity)
        {
            await _context.Operadores.AddAsync(entity);
        }

        public Task UpdateAsync(Operador entity)
        {
            _context.Operadores.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);

            if (entity is null)
                throw new KeyNotFoundException("Operador não encontrado");

            _context.Operadores.Remove(entity);
            await Task.CompletedTask;
        }

        public IQueryable<Operador> Query() => _context.Operadores.AsQueryable();
    }
}
