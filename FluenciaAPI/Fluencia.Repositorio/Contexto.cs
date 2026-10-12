using Microsoft.EntityFrameworkCore;
using FluenciaAPI.Dominio;
using Fluencia.Repositorio.Configuracoes;

namespace Fluencia.Repositorio.Contexto
{
    public class Contexto : DbContext
    {
        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Questao> Questoes { get; set; }
        public DbSet<Resposta> Respostas { get; set; }
        public DbSet<Resultado> Resultados { get; set; }

        public Contexto(DbContextOptions<Contexto> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AlunoConfiguracoes());
            modelBuilder.ApplyConfiguration(new QuestaoConfiguracoes());
            modelBuilder.ApplyConfiguration(new RespostaConfiguracoes());
            modelBuilder.ApplyConfiguration(new ResultadoConfiguracoes());
        }
    }
}