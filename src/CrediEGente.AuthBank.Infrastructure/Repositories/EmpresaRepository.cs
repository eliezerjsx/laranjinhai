using CapitalNerd.Laranjinhai.Domain.Entities;
using CapitalNerd.Laranjinhai.Domain.Interfaces;
using CapitalNerd.Laranjinhai.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CapitalNerd.Laranjinhai.Infrastructure.Repositories
{
    public class EmpresaRepository : IEmpresaRepository
    {
        private readonly LaranjinhaiDbContext _context;

        public EmpresaRepository(LaranjinhaiDbContext context)
        {
            _context = context;
        }

        public async Task<Empresa?> GetByIdAsync(Guid id)
        {
            return await _context.Empresas.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task CreateAsync(Empresa entity)
        {
            await _context.Empresas.AddAsync(entity);
        }

        public async Task UpdateAsync(Empresa entity)
        {
            _context.Empresas.Update(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);

            if (entity is null)
                throw new KeyNotFoundException("Empresa não encontrado");

            _context.Empresas.Remove(entity);
        }

        public IQueryable<Empresa> Query() => _context.Empresas.AsQueryable();
    }
}
