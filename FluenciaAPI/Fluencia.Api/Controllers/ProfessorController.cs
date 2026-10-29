using System.Threading.Tasks;
using Fluencia.Api.Models;
using Fluencia.Dominio.Entidades;
using Fluencia.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fluencia.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfessorController : ControllerBase
    {
        private readonly IProfessorRepositorio _repositorio;

        public ProfessorController(IProfessorRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        [HttpPost("cadastrar")]
        public async Task<IActionResult> Cadastrar([FromBody] CadastrarProfessorModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Nome))
            {
                return BadRequest("O nome do professor é obrigatório.");
            }

            var professor = new Professor
            {
                Nome = model.Nome
            };

            var professorCriado = await _repositorio.CadastrarAsync(professor);

            return Ok(new
            {
                mensagem = "Professor cadastrado com sucesso!",
                id = professorCriado.Id,
                codigo = professorCriado.Codigo,
                nome = professorCriado.Nome,
                primeiroAcesso = professorCriado.PrimeiroAcesso
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginProfessorModel model)
        {
            var professor = await _repositorio.ObterPorCodigoAsync(model.Codigo);

            if (professor == null || professor.Senha != model.Senha)
            {
                return BadRequest("Código de acesso ou senha inválidos.");
            }

            return Ok(new
            {
                id = professor.Id,
                codigo = professor.Codigo,
                nome = professor.Nome,
                primeiroAcesso = professor.PrimeiroAcesso
            });
        }

        [HttpPut("alterar-senha")]
        public async Task<IActionResult> AlterarSenha([FromBody] AlterarSenhaModel model)
        {
            if (string.IsNullOrWhiteSpace(model.NovaSenha) || model.NovaSenha.Length != 4)
            {
                return BadRequest("A nova senha deve conter exatamente 4 dígitos.");
            }

            var atualizado = await _repositorio.AlterarSenhaAsync(model.Id, model.NovaSenha);

            if (!atualizado)
            {
                return NotFound("Professor não encontrado.");
            }

            return Ok(new { mensagem = "Senha alterada com sucesso!" });
        }
    }
}