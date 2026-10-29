using Microsoft.EntityFrameworkCore;
using Fluencia.Repositorio;
using Fluencia.Repositorio.Interfaces;
using Fluencia.Servicos;
using Fluencia.Servicos.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// 1. Controladores e Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Banco de Dados SQL Server (Entity Framework Core)
builder.Services.AddDbContext<FluenciaContexto>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Injeção de Dependências - Repositórios (EF Core + Dapper)
builder.Services.AddScoped<IAlunoRepositorio, AlunoRepositorio>();
builder.Services.AddScoped<IQuestaoRepositorio, QuestaoRepositorio>();
builder.Services.AddScoped<IRespostaRepositorio, RespostaRepositorio>();
builder.Services.AddScoped<IResultadoRepositorio, ResultadoRepositorio>();
builder.Services.AddScoped<IProfessorRepositorio, ProfessorRepositorio>();

// 4. Injeção de Dependências - Serviços e IA (GitHub Models)
builder.Services.AddScoped<IAiService, AiService>();
builder.Services.AddScoped<IAvaliacaoService, AvaliacaoService>();
builder.Services.AddScoped<IPlanoService, PlanoService>();
builder.Services.AddScoped<IExtracaoService, ExtracaoService>();

// 5. Configuração de CORS para o React
builder.Services.AddCors(options =>
{
    options.AddPolicy("Dev", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();


app.UseCors("Dev");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();