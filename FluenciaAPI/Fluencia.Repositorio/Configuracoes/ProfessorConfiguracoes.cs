using Fluencia.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fluencia.Repositorio.Configuracoes
{
    public class ProfessorConfiguracoes : IEntityTypeConfiguration<Professor>
    {
        public void Configure(EntityTypeBuilder<Professor> builder)
        {
            builder.ToTable("Professores");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Codigo)
                .IsRequired()
                .HasMaxLength(10);

            builder.HasIndex(p => p.Codigo)
                .IsUnique();

            builder.Property(p => p.Nome)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(p => p.Senha)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.PrimeiroAcesso)
                .IsRequired();

            builder.Property(p => p.DataCadastro)
                .IsRequired();
        }
    }
}