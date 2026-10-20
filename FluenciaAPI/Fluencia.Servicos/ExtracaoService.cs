using System.Text.Json;
using Fluencia.Repositorio.Interfaces;
using FluenciaAPI.Dominio;
using Fluencia.Servicos.Interfaces;

namespace Fluencia.Servicos
{
    public class ExtracaoService : IExtracaoService
    {
        private readonly IAiService _aiService;
        private readonly IQuestaoRepositorio _questaoRepositorio;

        public ExtracaoService(IAiService aiService, IQuestaoRepositorio questaoRepositorio)
        {
            _aiService = aiService;
            _questaoRepositorio = questaoRepositorio;
        }

        public async Task<int> ExtrairQuestoes(Stream stream, string nomeArquivo)
        {
            // 1. Lê todo o texto do arquivo enviado
            using var leitor = new StreamReader(stream);
            string conteudoArquivo = await leitor.ReadToEndAsync();

            // 2. Pede para a Inteligência Artificial ler o arquivo e organizar as questões
            string prompt =
                "Leia o texto da prova abaixo e extraia todas as questoes de multipla escolha. " +
                "Retorne apenas um array JSON com os campos: " +
                "nivel, topico, enunciado, opcaoA, opcaoB, opcaoC, opcaoD e respostaCorreta (apenas A, B, C ou D). " +
                "Texto da prova: " + conteudoArquivo;

            string respostaIA = await _aiService.GetAiResponseAsync(prompt);

            // 3. Limpa a resposta da IA e transforma em lista C#
            string jsonLimpo = respostaIA.Replace("```json", "").Replace("```", "").Trim();

            var opcoes = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var questoes = JsonSerializer.Deserialize<List<QuestaoExtraida>>(jsonLimpo, opcoes);

            if (questoes == null || questoes.Count == 0)
                throw new InvalidOperationException("Nenhuma questão foi encontrada no arquivo.");

            // 4. Salva cada questão no banco de dados
            foreach (var item in questoes)
            {
                var novaQuestao = new Questao
                {
                    Nivel = item.Nivel,
                    Topico = item.Topico,
                    Enunciado = item.Enunciado,
                    OpcaoA = item.OpcaoA,
                    OpcaoB = item.OpcaoB,
                    OpcaoC = item.OpcaoC,
                    OpcaoD = item.OpcaoD,
                    OpcaoE = "Não sei responder",
                    RespostaCorreta = char.ToUpperInvariant(item.RespostaCorreta.Trim()[0])
                };

                await _questaoRepositorio.Adicionar(novaQuestao);
            }

            return questoes.Count;
        }
    }

    // Classe simples para receber os dados retornados pela IA
    public class QuestaoExtraida
    {
        public string Nivel { get; set; } = string.Empty;
        public string Topico { get; set; } = string.Empty;
        public string Enunciado { get; set; } = string.Empty;
        public string OpcaoA { get; set; } = string.Empty;
        public string OpcaoB { get; set; } = string.Empty;
        public string OpcaoC { get; set; } = string.Empty;
        public string OpcaoD { get; set; } = string.Empty;
        public string RespostaCorreta { get; set; } = "A";
    }
}