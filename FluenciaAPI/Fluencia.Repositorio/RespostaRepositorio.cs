using Microsoft.EntityFrameworkCore;
using FluenciaAPI.Dominio;
using Fluencia.Repositorio.Interfaces;

namespace Fluencia.Repositorio
{
    public class RespostaRepositorio : BaseRepositorio, IRespostaRepositorio
    {
        public RespostaRepositorio(FluenciaContexto contexto) : base(contexto)
        {
        }

        public async Task Adicionar(Resposta resposta)
        {
            await _contexto.Respostas.AddAsync(resposta);
            await _contexto.SaveChangesAsync();
        }

        public async Task<List<Resposta>> ListarPorAluno(int alunoId)
        {
            return await _contexto.Respostas
                .Include(r => r.Questao)
                .Where(r => r.AlunoId == alunoId)
                .ToListAsync();
        }
    }
}