using CapitalNerd.Laranjinhai.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CapitalNerd.Laranjinhai.Infrastructure.Data.Mappings;

public class OperadorMapping : IEntityTypeConfiguration<Operador>
{
    public void Configure(EntityTypeBuilder<Operador> builder)
    {
        builder.ToTable("operadors");

        builder.HasKey(e => e.Id);

                builder.Property(e => e.Id).HasColumnName("id").IsRequired();
                builder.Property(e => e.TenantId).HasColumnName("tenantid").IsRequired();
                builder.Property(e => e.Nome).HasColumnName("nome").IsRequired();
                builder.Property(e => e.Email).HasColumnName("email");
                builder.Property(e => e.Ativo).HasColumnName("ativo");
                builder.Property(e => e.CriadoEm).HasColumnName("criadoem");
            }
        }