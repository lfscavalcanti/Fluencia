using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using FluenciaAPI.Dominio;
using Fluencia.Repositorio.Interfaces;

namespace Fluencia.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResultadoController : ControllerBase
    {
        private readonly IResultadoRepositorio _resultadoRepositorio;
        private readonly IAlunoRepositorio _alunoRepositorio;

        public ResultadoController(
            IResultadoRepositorio resultadoRepositorio,
            IAlunoRepositorio alunoRepositorio)
        {
            _resultadoRepositorio = resultadoRepositorio;
            _alunoRepositorio = alunoRepositorio;
        }

        // GET api/resultado/obter/{id} - Busca resultado por Id
        [HttpGet]
        [Route("obter/{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            try
            {
                var resultado = await _resultadoRepositorio.BuscarPorId(id);
                if (resultado == null)
                    return NotFound("Resultado não encontrado.");

                var aluno = await _alunoRepositorio.BuscarPorId(resultado.AlunoId);
                return Ok(FormatarResultado(resultado, aluno?.Nome ?? "Aluno"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/resultado/aluno/{alunoId} - Busca resultado mais recente do aluno
        [HttpGet]
        [Route("aluno/{alunoId}")]
        public async Task<IActionResult> ObterPorAluno(int alunoId)
        {
            try
            {
                var resultado = await _resultadoRepositorio.BuscarPorAluno(alunoId);
                if (resultado == null)
                    return NotFound("Nenhum resultado encontrado para este aluno.");

                var aluno = await _alunoRepositorio.BuscarPorId(alunoId);
                return Ok(FormatarResultado(resultado, aluno?.Nome ?? "Aluno"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/resultado/listar - Listagem geral para o professor
        [HttpGet]
        [Route("listar")]
        public async Task<IActionResult> ListarTodos()
        {
            try
            {
                var resultados = await _resultadoRepositorio.ListarTodos();
                var listaFormatada = new List<object>();

                foreach (var r in resultados)
                {
                    var aluno = await _alunoRepositorio.BuscarPorId(r.AlunoId);
                    listaFormatada.Add(FormatarResultado(r, aluno?.Nome ?? "Aluno"));
                }

                return Ok(listaFormatada);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private static object FormatarResultado(Resultado r, string nomeAluno)
        {
            return new
            {
                r.Id,
                r.AlunoId,
                NomeAluno = nomeAluno,
                r.NivelIdentificado,
                TopicosDominados = JsonSerializer.Deserialize<List<string>>(r.TopicosDominados) ?? new List<string>(),
                TopicosPendentes = JsonSerializer.Deserialize<List<string>>(r.TopicosPendentes) ?? new List<string>(),
                r.PlanoEstudos,
                r.DataAvaliacao
            };
        }
    }
}