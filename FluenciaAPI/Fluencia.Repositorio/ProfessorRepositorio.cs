using System.Linq;
using System.Threading.Tasks;
using Fluencia.Dominio.Entidades;
using Fluencia.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fluencia.Repositorio
{
    public class ProfessorRepositorio : BaseRepositorio, IProfessorRepositorio
    {
        public ProfessorRepositorio(FluenciaContexto contexto) : base(contexto)
        {
        }

        public async Task<Professor?> ObterPorCodigoAsync(string codigo)
        {
            return await _contexto.Professores
                .FirstOrDefaultAsync(p => p.Codigo == codigo);
        }

        public async Task<string> ObterProximoCodigoAsync()
        {
            var total = await _contexto.Professores.CountAsync();
            int proximoNumero = total + 1;
            // Formata sempre com 4 dígitos: 0001, 0002, 0003...
            return proximoNumero.ToString("D4");
        }

        public async Task<Professor> CadastrarAsync(Professor professor)
        {
            professor.Codigo = await ObterProximoCodigoAsync();
            professor.Senha = "1111"; // Senha padrão inicial
            professor.PrimeiroAcesso = true;

            await _contexto.Professores.AddAsync(professor);
            await _contexto.SaveChangesAsync();

            return professor;
        }

        public async Task<bool> AlterarSenhaAsync(int id, string novaSenha)
        {
            var professor = await _contexto.Professores.FindAsync(id);
            if (professor == null)
            {
                return false;
            }

            professor.Senha = novaSenha;
            professor.PrimeiroAcesso = false;

            await _contexto.SaveChangesAsync();
            return true;
        }
    }
}