using Microsoft.EntityFrameworkCore;
using FluenciaAPI.Dominio;
using Fluencia.Repositorio.Interfaces;

namespace Fluencia.Repositorio
{
    public class QuestaoRepositorio : BaseRepositorio, IQuestaoRepositorio
    {
        public QuestaoRepositorio(FluenciaContexto contexto) : base(contexto)
        {
        }

        public async Task Adicionar(Questao questao)
        {
            await _contexto.Questoes.AddAsync(questao);
            await _contexto.SaveChangesAsync();
        }

        public async Task<Questao?> BuscarPorId(int id)
        {
            return await _contexto.Questoes.FindAsync(id);
        }

        public async Task<List<Questao>> ListarTodos()
        {
            return await _contexto.Questoes.ToListAsync();
        }

        public async Task Remover(int id)
        {
            var questao = await BuscarPorId(id);
            if (questao != null)
            {
                _contexto.Questoes.Remove(questao);
                await _contexto.SaveChangesAsync();
            }
        }
    }
}