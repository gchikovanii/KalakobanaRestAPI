using Kalakobana.Game.Domain.Models;

namespace Kalakobana.Game.Application.Repos
{
    public interface IVotingRepository
    {
        Task<int> AddPendingDataAsync(PendingData data);
    }
}
