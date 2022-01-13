using TramontinaFilmes.WebApi.Dominio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace TramontinaFilmes.WebApi.Infraestrutura.EntityConfigurations
{
    public sealed class FilmeTypeConfiguration : IEntityTypeConfiguration<Filme>
    {
        public void Configure(EntityTypeBuilder<Filme> builder)
        {
            builder.ToTable("Filmes");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Titulo).HasColumnType("varchar(100)");
            builder.Property(c => c.Duracao).HasColumnType("integer");
            builder.Property(c => c.Sinopse).HasColumnType("varchar(100)");
            builder.Property<DateTime>("DataUltimaAlteracao");
            builder.Property<DateTime>("DataCadastro");
        }
    }
}
