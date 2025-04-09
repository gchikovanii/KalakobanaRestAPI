using Kalakobana.Game.Domain.Models;
using Kalakobana.SharedKernel.Results;

namespace Kalakobana.Game.Application.Services
{
    public interface IVotingService
    {
        Task<Result<int>> CreatePendingDataAsync(PendingData data);
    }
}
