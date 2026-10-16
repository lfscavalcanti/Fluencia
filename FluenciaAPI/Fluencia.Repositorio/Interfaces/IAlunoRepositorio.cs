using FluenciaAPI.Dominio;

namespace Fluencia.Repositorio.Interfaces
{
    public interface IAlunoRepositorio
    {
        Task Adicionar(Aluno aluno);
        Task<Aluno?> BuscarPorId(int id);
        Task<List<Aluno>> ListarTodos();

        // Métodos avançados de Banco de Dados com Dapper
        Task AtualizarEmailViaProcedure(int alunoId, string novoEmail);
        Task<IEnumerable<Aluno>> ListarAlunosViaView();
        Task<string?> ObterNomeAlunoViaFunction(int alunoId);
    }
}