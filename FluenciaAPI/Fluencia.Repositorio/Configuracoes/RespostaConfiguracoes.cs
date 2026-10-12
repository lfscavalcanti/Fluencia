using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FluenciaAPI.Dominio;

namespace Fluencia.Repositorio.Configuracoes
{
    public class RespostaConfiguracoes : IEntityTypeConfiguration<Resposta>
    {
        public void Configure(EntityTypeBuilder<Resposta> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.AlunoId).IsRequired();
            builder.Property(r => r.QuestaoId).IsRequired();
            builder.Property(r => r.OpcaoEscolhida ).IsRequired();
            builder.Property(r => r.NãoSei).IsRequired();
            builder.Property(r => r.Acertou).IsRequired();
        }
    }
}