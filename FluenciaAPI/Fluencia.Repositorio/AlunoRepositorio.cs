using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using FluenciaAPI.Dominio;
using Fluencia.Repositorio.Interfaces;

namespace Fluencia.Repositorio
{
    public class AlunoRepositorio : BaseRepositorio, IAlunoRepositorio
    {
        public AlunoRepositorio(FluenciaContexto contexto) : base(contexto)
        {
        }

        public async Task Adicionar(Aluno aluno)
        {
            await _contexto.Alunos.AddAsync(aluno);
            await _contexto.SaveChangesAsync();
        }

        public async Task<Aluno?> BuscarPorId(int id)
        {
            return await _contexto.Alunos.FindAsync(id);
        }

        public async Task<List<Aluno>> ListarTodos()
        {
            return await _contexto.Alunos.ToListAsync();
        }        
        
        public async Task AtualizarEmailViaProcedure(int alunoId, string novoEmail)
        {
            var parametros = new { AlunoId = alunoId, NovoEmail = novoEmail };

            await _contexto.Database.GetDbConnection().ExecuteAsync(
                "spAtualizarEmailAluno",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }
        
        public async Task<IEnumerable<Aluno>> ListarAlunosViaView()
        {
            return await _contexto.Database.GetDbConnection().QueryAsync<Aluno>(
                "SELECT * FROM vwAlunos"
            );
        }
        
        public async Task<string?> ObterNomeAlunoViaFunction(int alunoId)
        {
            var parametros = new { AlunoId = alunoId };

            return await _contexto.Database.GetDbConnection().QueryFirstOrDefaultAsync<string>(
                "SELECT dbo.fnObterNomeAluno(@AlunoId)",
                parametros
            );
        }
    }
}