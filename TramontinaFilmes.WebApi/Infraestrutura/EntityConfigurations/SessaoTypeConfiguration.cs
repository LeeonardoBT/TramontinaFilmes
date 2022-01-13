using TramontinaFilmes.WebApi.Dominio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace TramontinaFilmes.WebApi.Infraestrutura.EntityConfigurations
{
    public sealed class SessaoTypeConfiguration : IEntityTypeConfiguration<Sessao>
    {
        public void Configure(EntityTypeBuilder<Sessao> builder)
        {
            builder.ToTable("Sessoes");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.FilmeId).HasColumnType("uniqueidentifier");
            builder.Property(c => c.Dia).HasColumnType("integer");
            builder.Property(c => c.Mes).HasColumnType("integer");
            builder.Property(c => c.Ano).HasColumnType("integer");
            builder.Property(c => c.HoraInicio).HasColumnType("integer");
            builder.Property(c => c.MinutoInicio).HasColumnType("integer");
            builder.Property(c => c.QuantidadeLugares).HasColumnType("integer");
            builder.Property(c => c.QuantidadeDisponivel).HasColumnType("integer");
            builder.Property(c => c.ValorIngresso).HasColumnType("float");
            builder.Property<DateTime>("DataUltimaAlteracao");
            builder.Property<DateTime>("DataCadastro");
        }
    }
}
