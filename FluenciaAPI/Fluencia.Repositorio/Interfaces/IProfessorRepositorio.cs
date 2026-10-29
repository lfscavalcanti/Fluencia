using System.Threading.Tasks;
using Fluencia.Dominio.Entidades;

namespace Fluencia.Repositorio.Interfaces
{
    public interface IProfessorRepositorio
    {
        Task<Professor?> ObterPorCodigoAsync(string codigo);
        Task<Professor> CadastrarAsync(Professor professor);
        Task<bool> AlterarSenhaAsync(int id, string novaSenha);
        Task<string> ObterProximoCodigoAsync();
    }
}