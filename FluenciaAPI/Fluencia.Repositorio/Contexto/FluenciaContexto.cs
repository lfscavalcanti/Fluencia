using Microsoft.EntityFrameworkCore;
using FluenciaAPI.Dominio;
using Fluencia.Dominio.Entidades;
using Fluencia.Repositorio.Configuracoes;

public class FluenciaContexto : DbContext
{
    public DbSet<Aluno> Alunos { get; set; } = null!;
    public DbSet<Questao> Questoes { get; set; } = null!;
    public DbSet<Resposta> Respostas { get; set; } = null!;
    public DbSet<Resultado> Resultados { get; set; } = null!;
    public DbSet<Professor> Professores { get; set; } = null!;

    public FluenciaContexto() { }
    public FluenciaContexto(DbContextOptions options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                "Server=ESCRITORIO\\SQLEXPRESS;Database=FluenciaDB;Trusted_Connection=True;TrustServerCertificate=True;"
            );
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AlunoConfiguracoes());
        modelBuilder.ApplyConfiguration(new QuestaoConfiguracoes());
        modelBuilder.ApplyConfiguration(new RespostaConfiguracoes());
        modelBuilder.ApplyConfiguration(new ResultadoConfiguracoes());
    }
}