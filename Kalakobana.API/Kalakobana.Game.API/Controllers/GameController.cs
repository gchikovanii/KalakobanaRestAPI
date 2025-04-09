using Kalakobana.Game.Application.Services;
using Kalakobana.Game.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kalakobana.Game.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpPost("submit-answers")]
        public async Task<IActionResult> SubmitAnswers([FromBody] AnswerRequestDTO request)
        {
            var response = await _gameService.SubmitAnswersAsync(request);
            return Ok(response);
        }
    }
}
