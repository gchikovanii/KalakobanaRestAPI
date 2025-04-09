using Kalakobana.Game.Domain.Models;

namespace Kalakobana.Game.Application.Services
{
    public interface IGameService
    {
        Task<AnswerResponseDTO> SubmitAnswersAsync(AnswerRequestDTO request);
    }
}
