using Microsoft.AspNetCore.Mvc;
using FluenciaAPI.Dominio;
using Fluencia.Api.Models;
using Fluencia.Repositorio.Interfaces;

namespace Fluencia.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlunoController : ControllerBase
    {
        private readonly IAlunoRepositorio _alunoRepositorio;

        public AlunoController(IAlunoRepositorio alunoRepositorio)
        {
            _alunoRepositorio = alunoRepositorio;
        }

        // POST api/aluno/cadastrar
        [HttpPost]
        [Route("cadastrar")]
        public async Task<IActionResult> Cadastrar([FromBody] CadastrarAlunoModel model)
        {
            try
            {
                var aluno = new Aluno
                {
                    Nome = model.Nome,
                    Email = model.Email,
                    DataCadastro = DateTime.Now
                };

                await _alunoRepositorio.Adicionar(aluno);
                return Ok(aluno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/aluno/listar
        [HttpGet]
        [Route("listar")]
        public async Task<IActionResult> ListarTodos()
        {
            try
            {
                var alunos = await _alunoRepositorio.ListarTodos();
                return Ok(alunos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/aluno/obter/{id}
        [HttpGet]
        [Route("obter/{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            try
            {
                var aluno = await _alunoRepositorio.BuscarPorId(id);

                if (aluno == null)
                    return NotFound("Aluno não encontrado.");

                return Ok(aluno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        // Endpoints Dapper        

        // PUT api/aluno/atualizar-email
        [HttpPut]
        [Route("atualizar-email")]
        public async Task<IActionResult> AtualizarEmail(int id, [FromBody] string novoEmail)
        {
            try
            {
                await _alunoRepositorio.AtualizarEmailViaProcedure(id, novoEmail);
                return Ok("E-mail atualizado com sucesso via Stored Procedure!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/aluno/view
        [HttpGet]
        [Route("view")]
        public async Task<IActionResult> ListarViaView()
        {
            try
            {
                var alunos = await _alunoRepositorio.ListarAlunosViaView();
                return Ok(alunos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/aluno/nome-funcao/{id}
        [HttpGet]
        [Route("nome-funcao/{id}")]
        public async Task<IActionResult> ObterNomeFuncao(int id)
        {
            try
            {
                var nome = await _alunoRepositorio.ObterNomeAlunoViaFunction(id);
                return Ok(new { AlunoId = id, Nome = nome });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}