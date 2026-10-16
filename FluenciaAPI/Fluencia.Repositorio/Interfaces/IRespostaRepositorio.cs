using FluenciaAPI.Dominio;

namespace Fluencia.Repositorio.Interfaces
{
    public interface IRespostaRepositorio
    {
        Task Adicionar(Resposta resposta);
        Task<List<Resposta>> ListarPorAluno(int alunoId);
    }
}