using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FluenciaAPI.Dominio;

namespace Fluencia.Repositorio.Configuracoes
{
    public class ResultadoConfiguracoes : IEntityTypeConfiguration<Resultado>
    {
        public void Configure(EntityTypeBuilder<Resultado> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.AlunoId).IsRequired();
            builder.Property(r => r.NivelIdentificado).IsRequired().HasMaxLength(50);
            builder.Property(r => r.TopicosDominados).IsRequired();
            builder.Property(r => r.TopicosPendentes).IsRequired();
            builder.Property(r => r.PlanoEstudos).IsRequired();
            builder.Property(r => r.DataAvaliacao).IsRequired();
        }
    }
}