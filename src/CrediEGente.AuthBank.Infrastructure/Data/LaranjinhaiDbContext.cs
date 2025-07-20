using CapitalNerd.Laranjinhai.Domain.Entities;
using CapitalNerd.Laranjinhai.Infrastructure.Data.Mappings;
using Microsoft.EntityFrameworkCore;

namespace CapitalNerd.Laranjinhai.Infrastructure.Data
{
    public class LaranjinhaiDbContext(DbContextOptions<LaranjinhaiDbContext> options) : DbContext(options)
    {
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Operador> Operadores { get; set; }
        public DbSet<Mensagem> Mensagens { get; set; }
        public DbSet<Resposta> Respostas { get; set; }
        public DbSet<SessaoWhatsapp> SessoesWhatsapp { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mapeamentos com Fluent API (crie as classes correspondentes depois)
            modelBuilder.ApplyConfiguration(new ClienteMapping());
            modelBuilder.ApplyConfiguration(new EmpresaMapping());
            modelBuilder.ApplyConfiguration(new OperadorMapping());
            modelBuilder.ApplyConfiguration(new MensagemMapping());
            modelBuilder.ApplyConfiguration(new RespostaMapping());
            modelBuilder.ApplyConfiguration(new SessaoWhatsappMapping());
        }
    }
}
