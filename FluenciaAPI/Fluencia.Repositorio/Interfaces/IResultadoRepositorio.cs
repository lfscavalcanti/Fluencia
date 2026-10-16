using FluenciaAPI.Dominio;

namespace Fluencia.Repositorio.Interfaces
{
    public interface IResultadoRepositorio
    {
        Task Adicionar(Resultado resultado);
        Task Atualizar(Resultado resultado);
        Task<Resultado?> BuscarPorId(int id);
        Task<Resultado?> BuscarPorAluno(int alunoId);
        Task<List<Resultado>> ListarTodos();
    }
}