using Microsoft.AspNetCore.Mvc;
using FluenciaAPI.Dominio;
using Fluencia.Api.Models;
using Fluencia.Repositorio.Interfaces;
using Fluencia.Servicos.Interfaces;

namespace Fluencia.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TesteController : ControllerBase
    {
        private readonly IQuestaoRepositorio _questaoRepositorio;
        private readonly IRespostaRepositorio _respostaRepositorio;
        private readonly IAvaliacaoService _avaliacaoService;
        private readonly IPlanoService _planoService;

        public TesteController(
            IQuestaoRepositorio questaoRepositorio,
            IRespostaRepositorio respostaRepositorio,
            IAvaliacaoService avaliacaoService,
            IPlanoService planoService)
        {
            _questaoRepositorio = questaoRepositorio;
            _respostaRepositorio = respostaRepositorio;
            _avaliacaoService = avaliacaoService;
            _planoService = planoService;
        }

        // GET api/teste/questoes - Entrega a prova para o aluno sem o gabarito
        [HttpGet]
        [Route("questoes")]
        public async Task<IActionResult> ObterQuestoesParaTeste()
        {
            try
            {
                var questoes = await _questaoRepositorio.ListarTodos();

                if (!questoes.Any())
                    return NotFound("Nenhuma questão cadastrada para o teste.");

                // Retorna as alternativas sem expor a RespostaCorreta
                var questoesProva = questoes.Select(q => new
                {
                    q.Id,
                    q.Nivel,
                    q.Topico,
                    q.Enunciado,
                    q.OpcaoA,
                    q.OpcaoB,
                    q.OpcaoC,
                    q.OpcaoD,
                    q.OpcaoE
                });

                return Ok(questoesProva);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/teste/responder/{alunoId} - Envia as respostas, avalia e gera o plano com IA
        [HttpPost]
        [Route("responder/{alunoId}")]
        public async Task<IActionResult> Responder(int alunoId, [FromBody] List<EnviarRespostaModel> respostasEnviadas)
        {
            if (respostasEnviadas == null || !respostasEnviadas.Any())
                return BadRequest("Nenhuma resposta enviada.");

            try
            {
                var questoes = await _questaoRepositorio.ListarTodos();

                // 1. Grava cada resposta no banco avaliando se acertou ou se marcou "Não sei"
                foreach (var item in respostasEnviadas)
                {
                    var questao = questoes.FirstOrDefault(q => q.Id == item.QuestaoId);
                    if (questao == null) continue;

                    char escolha = char.ToUpperInvariant(item.OpcaoEscolhida);
                    bool marcouNaoSei = (escolha == 'E');

                    var resposta = new Resposta
                    {
                        AlunoId = alunoId,
                        QuestaoId = item.QuestaoId,
                        OpcaoEscolhida = escolha,
                        NãoSei = marcouNaoSei,
                        Acertou = !marcouNaoSei && (escolha == questao.RespostaCorreta)
                    };

                    await _respostaRepositorio.Adicionar(resposta);
                }

                // 2. Calcula o nível CEFR (A1 a C2) e separa os tópicos
                var resultado = await _avaliacaoService.Avaliar(alunoId);

                // 3. Aciona a IA (GitHub Models) para formular o plano de estudos
                await _planoService.GerarPlano(resultado);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}