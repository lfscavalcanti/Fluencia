using System.Text.Json;
using Fluencia.Repositorio.Interfaces;
using FluenciaAPI.Dominio;
using Fluencia.Servicos.Interfaces;

namespace Fluencia.Servicos
{
    public class AvaliacaoService : IAvaliacaoService
    {
        private readonly IRespostaRepositorio _respostaRepositorio;
        private readonly IResultadoRepositorio _resultadoRepositorio;

        public AvaliacaoService(
            IRespostaRepositorio respostaRepositorio,
            IResultadoRepositorio resultadoRepositorio)
        {
            _respostaRepositorio = respostaRepositorio;
            _resultadoRepositorio = resultadoRepositorio;
        }

        public async Task<Resultado> Avaliar(int alunoId)
        {
            var respostas = await _respostaRepositorio.ListarPorAluno(alunoId);

            if (!respostas.Any())
                throw new InvalidOperationException("Nenhuma resposta encontrada para este aluno.");

            // Calcula o aproveitamento por tópico gramatical
            var aproveitamento = respostas
                .GroupBy(r => r.Questao.Topico)
                .ToDictionary(
                    grupo => grupo.Key,
                    grupo => (double)grupo.Count(r => r.Acertou && !r.NãoSei) / grupo.Count()
                );

            // Tópicos com 70% ou mais de acerto o aluno domina
            var dominados = aproveitamento
                .Where(a => a.Value >= 0.7)
                .Select(a => a.Key)
                .ToList();

            // Tópicos com menos de 70% de acerto ou marcados como "Não sei" precisam de reforço
            var pendentes = aproveitamento
                .Where(a => a.Value < 0.7)
                .Select(a => a.Key)
                .ToList();

            string nivel = DeterminarNivel(respostas);

            var resultado = new Resultado
            {
                AlunoId = alunoId,
                NivelIdentificado = nivel,
                TopicosDominados = JsonSerializer.Serialize(dominados),
                TopicosPendentes = JsonSerializer.Serialize(pendentes),
                PlanoEstudos = string.Empty,
                DataAvaliacao = DateTime.Now
            };

            await _resultadoRepositorio.Adicionar(resultado);
            return resultado;
        }

        private static string DeterminarNivel(List<Resposta> respostas)
        {
            var niveis = new[] { "A1", "A2", "B1", "B2", "C1", "C2" };
            string nivelFinal = "A1";

            foreach (var nivel in niveis)
            {
                var questoesDoNivel = respostas.Where(r => r.Questao.Nivel == nivel).ToList();
                if (!questoesDoNivel.Any()) continue;

                double aproveitamentoNivel = (double)questoesDoNivel.Count(r => r.Acertou && !r.NãoSei)
                                             / questoesDoNivel.Count;

                if (aproveitamentoNivel >= 0.5)
                    nivelFinal = nivel;
                else
                    break;
            }

            return nivelFinal;
        }
    }
}