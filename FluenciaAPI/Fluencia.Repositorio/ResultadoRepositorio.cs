using Microsoft.EntityFrameworkCore;
using FluenciaAPI.Dominio;
using Fluencia.Repositorio.Interfaces;

namespace Fluencia.Repositorio
{
    public class ResultadoRepositorio : BaseRepositorio, IResultadoRepositorio
    {
        public ResultadoRepositorio(FluenciaContexto contexto) : base(contexto)
        {
        }

        public async Task Adicionar(Resultado resultado)
        {
            await _contexto.Resultados.AddAsync(resultado);
            await _contexto.SaveChangesAsync();
        }

        public async Task Atualizar(Resultado resultado)
        {
            _contexto.Resultados.Update(resultado);
            await _contexto.SaveChangesAsync();
        }

        public async Task<Resultado?> BuscarPorId(int id)
        {
            return await _contexto.Resultados.FindAsync(id);
        }

        public async Task<Resultado?> BuscarPorAluno(int alunoId)
        {
            return await _contexto.Resultados
                .Where(r => r.AlunoId == alunoId)
                .OrderByDescending(r => r.DataAvaliacao)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Resultado>> ListarTodos()
        {
            return await _contexto.Resultados
                .OrderByDescending(r => r.DataAvaliacao)
                .ToListAsync();
        }
    }
}