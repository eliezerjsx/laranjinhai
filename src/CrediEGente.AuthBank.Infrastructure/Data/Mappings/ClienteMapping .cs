using CapitalNerd.Laranjinhai.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CapitalNerd.Laranjinhai.Infrastructure.Data.Mappings;

public class ClienteMapping : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("clientes");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("id").IsRequired();
        builder.Property(e => e.TenantId).HasColumnName("tenantid").IsRequired();
        builder.Property(e => e.Nome).HasColumnName("nome").IsRequired();
        builder.Property(e => e.Telefone).HasColumnName("telefone").IsRequired();
        builder.Property(e => e.CriadoEm).HasColumnName("criadoem");
    }
}
