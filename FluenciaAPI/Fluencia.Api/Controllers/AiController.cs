using Microsoft.AspNetCore.Mvc;
using Fluencia.Servicos;
using Fluencia.Servicos.Interfaces;

namespace Fluencia.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AiController : ControllerBase
    {
        private readonly IAiService _aiService;

        public AiController(IAiService aiService)
        {
            _aiService = aiService;
        }

        // POST api/ai/completar - Endpoint oficial do tutorial Itera360
        [HttpPost]
        [Route("completar")]
        public async Task<IActionResult> Completar([FromBody] string prompt)
        {
            try
            {
                var resposta = await _aiService.GetAiResponseAsync(prompt);
                return Ok(resposta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}