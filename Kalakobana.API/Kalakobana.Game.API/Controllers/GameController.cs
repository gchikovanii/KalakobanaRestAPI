using Kalakobana.Game.Application.Services;
using Kalakobana.Game.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kalakobana.Game.API.Controllers
{
    [Authorize(Policy = "UserOnly")]
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly IVotingService _votingService;

        public GameController(IGameService gameService, IVotingService votingService)
        {
            _gameService = gameService;
            _votingService = votingService;
        }

        [HttpPost("submit-answers")]
        public async Task<IActionResult> SubmitAnswers([FromBody] AnswerRequestDTO request)
        {
            var response = await _gameService.SubmitAnswersAsync(request);
            return Ok(response);
        }
        [HttpPost("vote-answer")]
        public async Task<IActionResult> Post([FromBody] PendingData data)
        {
            var result = await _votingService.CreatePendingDataAsync(data);
            if (result.Succeeded)
                return Ok(new { id = result.ResultValue });

            return BadRequest(new { error = result.Error });
        }
    }
}
