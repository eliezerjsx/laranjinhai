using CapitalNerd.Laranjinhai.Domain.Entities;
using CapitalNerd.Laranjinhai.Domain.Interfaces;
using CapitalNerd.Laranjinhai.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CapitalNerd.Laranjinhai.Infrastructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly LaranjinhaiDbContext _context;

        public ClienteRepository(LaranjinhaiDbContext context)
        {
            _context = context;
        }

        public async Task<Cliente?> GetByIdAsync(Guid id)
        {
            return await _context.Clientes.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task CreateAsync(Cliente entity)
        {
            await _context.Clientes.AddAsync(entity);
        }

        public async Task UpdateAsync(Cliente entity)
        {
            _context.Clientes.Update(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);

            if (entity is null)
                throw new KeyNotFoundException("Cliente não encontrado");

            _context.Clientes.Remove(entity);
        }

        public IQueryable<Cliente> Query() => _context.Clientes.AsQueryable();
    }
}
