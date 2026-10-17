using FluenciaAPI.Dominio;

namespace Fluencia.Servicos.Interfaces
{
    public interface IAvaliacaoService
    {
        Task<Resultado> Avaliar(int alunoId);
    }
}