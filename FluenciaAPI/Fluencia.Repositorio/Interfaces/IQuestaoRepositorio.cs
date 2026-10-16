using FluenciaAPI.Dominio;

namespace Fluencia.Repositorio.Interfaces
{
    public interface IQuestaoRepositorio
    {
        Task Adicionar(Questao questao);
        Task<Questao?> BuscarPorId(int id);
        Task<List<Questao>> ListarTodos();
        Task Remover(int id);
    }
}