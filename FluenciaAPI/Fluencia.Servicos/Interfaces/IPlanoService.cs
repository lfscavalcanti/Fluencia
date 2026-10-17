using FluenciaAPI.Dominio;

namespace Fluencia.Servicos.Interfaces
{
    public interface IPlanoService
    {
        Task GerarPlano(Resultado resultado);
    }
}