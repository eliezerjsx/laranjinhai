using CapitalNerd.Laranjinhai.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CapitalNerd.Laranjinhai.Infrastructure.Data.Mappings;

public class RespostaMapping : IEntityTypeConfiguration<Resposta>
{
    public void Configure(EntityTypeBuilder<Resposta> builder)
    {
        builder.ToTable("respostas");

        builder.HasKey(e => e.Id);

                builder.Property(e => e.Id).HasColumnName("id").IsRequired();
                builder.Property(e => e.TenantId).HasColumnName("tenantid").IsRequired();
                builder.Property(e => e.MensagemId).HasColumnName("mensagemid").IsRequired();
                builder.Property(e => e.OperadorId).HasColumnName("operadorid");
                builder.Property(e => e.Conteudo).HasColumnName("conteudo").IsRequired();
                builder.Property(e => e.CriadoEm).HasColumnName("criadoem");
            }
        }