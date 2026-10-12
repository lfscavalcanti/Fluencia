using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FluenciaAPI.Dominio;

namespace Fluencia.Repositorio.Configuracoes
{
    public class QuestaoConfiguracoes : IEntityTypeConfiguration<Questao>
    {
        public void Configure(EntityTypeBuilder<Questao> builder)
        {
            builder.HasKey(q => q.Id);
            builder.Property(q => q.Nivel).IsRequired().HasMaxLength(50);
            builder.Property(q => q.Topico).IsRequired().HasMaxLength(100);
            builder.Property(q => q.Enunciado).IsRequired();
            builder.Property(q => q.OpcaoA).IsRequired();
            builder.Property(q => q.OpcaoB).IsRequired();
            builder.Property(q => q.OpcaoC).IsRequired();
            builder.Property(q => q.OpcaoD).IsRequired();
            builder.Property(q => q.OpcaoE).IsRequired();
            builder.Property(q => q.RespostaCorreta).IsRequired();
        }
    }
}