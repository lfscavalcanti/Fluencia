using Microsoft.AspNetCore.Mvc;
using FluenciaAPI.Dominio;
using Fluencia.Api.Models;
using Fluencia.Repositorio.Interfaces;
using Fluencia.Servicos.Interfaces;

namespace Fluencia.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestaoController : ControllerBase
    {
        private readonly IQuestaoRepositorio _questaoRepositorio;
        private readonly IExtracaoService _extracaoService;

        public QuestaoController(
            IQuestaoRepositorio questaoRepositorio,
            IExtracaoService extracaoService)
        {
            _questaoRepositorio = questaoRepositorio;
            _extracaoService = extracaoService;
        }

        // POST api/questao/cadastrar - Cadastro manual pelo professor
        [HttpPost]
        [Route("cadastrar")]
        public async Task<IActionResult> Cadastrar([FromBody] CadastrarQuestaoModel model)
        {
            try
            {
                var questao = new Questao
                {
                    Nivel = model.Nivel,
                    Topico = model.Topico,
                    Enunciado = model.Enunciado,
                    OpcaoA = model.OpcaoA,
                    OpcaoB = model.OpcaoB,
                    OpcaoC = model.OpcaoC,
                    OpcaoD = model.OpcaoD,
                    OpcaoE = string.IsNullOrWhiteSpace(model.OpcaoE) ? "Não sei responder" : model.OpcaoE,
                    RespostaCorreta = model.RespostaCorreta
                };

                await _questaoRepositorio.Adicionar(questao);
                return Ok(questao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/questao/upload - Importação em lote de arquivo .txt ou .docx
        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> Upload(IFormFile arquivo)
        {
            if (arquivo == null || arquivo.Length == 0)
                return BadRequest("Nenhum arquivo enviado.");

            try
            {
                using var stream = arquivo.OpenReadStream();
                int quantidade = await _extracaoService.ExtrairQuestoes(stream, arquivo.FileName);
                return Ok(new { Mensagem = $"{quantidade} questão(ões) importada(s) com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/questao/listar
        [HttpGet]
        [Route("listar")]
        public async Task<IActionResult> ListarTodos()
        {
            try
            {
                var questoes = await _questaoRepositorio.ListarTodos();
                return Ok(questoes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/questao/obter/{id}
        [HttpGet]
        [Route("obter/{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            try
            {
                var questao = await _questaoRepositorio.BuscarPorId(id);

                if (questao == null)
                    return NotFound("Questão não encontrada.");

                return Ok(questao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/questao/remover/{id}
        [HttpDelete]
        [Route("remover/{id}")]
        public async Task<IActionResult> Remover(int id)
        {
            try
            {
                var questao = await _questaoRepositorio.BuscarPorId(id);
                if (questao == null)
                    return NotFound("Questão não encontrada.");

                await _questaoRepositorio.Remover(id);
                return Ok("Questão removida com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}