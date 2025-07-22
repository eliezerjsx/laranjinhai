using CapitalNerd.Laranjinhai.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CapitalNerd.Laranjinhai.Infrastructure.Data.Mappings;

public class EmpresaMapping : IEntityTypeConfiguration<Empresa>
{
    public void Configure(EntityTypeBuilder<Empresa> builder)
    {
        builder.ToTable("empresas");

        builder.HasKey(e => e.Id);

                builder.Property(e => e.Id).HasColumnName("id").IsRequired();
                builder.Property(e => e.Nome).HasColumnName("nome").IsRequired();
                builder.Property(e => e.EmailResponsavel).HasColumnName("emailresponsavel");
                builder.Property(e => e.TelefoneContato).HasColumnName("telefonecontato");
                builder.Property(e => e.Ativo).HasColumnName("ativo");
                builder.Property(e => e.CriadoEm).HasColumnName("criadoem");
            }
        }