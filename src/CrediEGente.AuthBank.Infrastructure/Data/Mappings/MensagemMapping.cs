using CapitalNerd.Laranjinhai.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CapitalNerd.Laranjinhai.Infrastructure.Data.Mappings;

public class MensagemMapping : IEntityTypeConfiguration<Mensagem>
{
    public void Configure(EntityTypeBuilder<Mensagem> builder)
    {
        builder.ToTable("mensagems");

        builder.HasKey(e => e.Id);

                builder.Property(e => e.Id).HasColumnName("id").IsRequired();
                builder.Property(e => e.TenantId).HasColumnName("tenantid").IsRequired();
                builder.Property(e => e.ClienteId).HasColumnName("clienteid").IsRequired();
                builder.Property(e => e.Conteudo).HasColumnName("conteudo").IsRequired();
                builder.Property(e => e.Direcao).HasColumnName("direcao").IsRequired();
                builder.Property(e => e.RespondidoPorIa).HasColumnName("respondidoporia");
                builder.Property(e => e.RespondidoPorOperador).HasColumnName("respondidoporoperador");
                builder.Property(e => e.CriadoEm).HasColumnName("criadoem");
            }
        }