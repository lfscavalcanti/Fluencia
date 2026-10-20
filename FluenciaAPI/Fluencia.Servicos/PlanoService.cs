using System.Text;
using System.Text.Json;
using Fluencia.Repositorio.Interfaces;
using FluenciaAPI.Dominio;
using Fluencia.Servicos.Interfaces;

namespace Fluencia.Servicos
{
    public class PlanoService : IPlanoService
    {
        private readonly IAiService _aiService;
        private readonly IResultadoRepositorio _resultadoRepositorio;
        private readonly IAlunoRepositorio _alunoRepositorio;

        public PlanoService(
            IAiService aiService,
            IResultadoRepositorio resultadoRepositorio,
            IAlunoRepositorio alunoRepositorio)
        {
            _aiService = aiService;
            _resultadoRepositorio = resultadoRepositorio;
            _alunoRepositorio = alunoRepositorio;
        }

        public async Task GerarPlano(Resultado resultado)
        {
            var aluno = await _alunoRepositorio.BuscarPorId(resultado.AlunoId);
            var pendentes = JsonSerializer.Deserialize<List<string>>(resultado.TopicosPendentes) ?? new List<string>();

            try
            {
                var topicos = string.Join(", ", pendentes.Take(12));
                string prompt =
                    $"Crie um plano de estudos de inglês em português, 4 semanas, no máximo 350 palavras. " +
                    $"Aluno: {aluno?.Nome ?? "Aluno"}. Nível CEFR: {resultado.NivelIdentificado}. " +
                    $"Foque nestes tópicos onde o aluno teve dúvidas ou errou: {topicos}. " +
                    "Para cada semana, liste atividades práticas curtas e objetivas.";

                resultado.PlanoEstudos = await _aiService.GetAiResponseAsync(prompt);
            }
            catch
            {
                resultado.PlanoEstudos = MontarPlanoPadrao(
                    aluno?.Nome ?? "Aluno",
                    resultado.NivelIdentificado,
                    pendentes);
            }

            await _resultadoRepositorio.Atualizar(resultado);
        }

        private static string MontarPlanoPadrao(string nome, string nivel, List<string> pendentes)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"Plano de estudos — {nome}");
            builder.AppendLine($"Nível identificado: {nivel}");
            builder.AppendLine();

            if (pendentes.Count == 0)
            {
                builder.AppendLine("Parabéns! Não há tópicos pendentes no momento. Continue praticando o conteúdo do seu nível atual.");
                return builder.ToString();
            }

            builder.AppendLine("Tópicos a reforçar:");
            foreach (var t in pendentes)
                builder.AppendLine($"  • {t}");

            builder.AppendLine();
            builder.AppendLine("Semana 1–2: revisão teórica e exercícios guiados em cada tópico.");
            builder.AppendLine("Semana 3: conversação e produção textual curta aplicando as estruturas.");
            builder.AppendLine("Semana 4: simulado prático e revisão dos pontos com mais dúvidas.");

            return builder.ToString();
        }
    }
}