using CapitalNerd.Laranjinhai.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CapitalNerd.Laranjinhai.Infrastructure.Data.Mappings;

public class SessaoWhatsappMapping : IEntityTypeConfiguration<SessaoWhatsapp>
{
    public void Configure(EntityTypeBuilder<SessaoWhatsapp> builder)
    {
        builder.ToTable("sessaowhatsapps");

        builder.HasKey(e => e.Id);

                builder.Property(e => e.Id).HasColumnName("id").IsRequired();
                builder.Property(e => e.EmpresaId).HasColumnName("empresaid").IsRequired();
                builder.Property(e => e.Status).HasColumnName("status").IsRequired();
                builder.Property(e => e.QrCodeBase64).HasColumnName("qrcodebase64");
                builder.Property(e => e.NumeroConectado).HasColumnName("numeroconectado");
                builder.Property(e => e.UltimaAtualizacao).HasColumnName("ultimaatualizacao");
                builder.Property(e => e.CriadoEm).HasColumnName("criadoem");
            }
        }