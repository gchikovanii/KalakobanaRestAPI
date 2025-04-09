using Kalakobana.Game.Application.Repos;
using Kalakobana.Game.Domain.Models;
using Kalakobana.SharedKernel.Constants;
using Kalakobana.SharedKernel.Results;

namespace Kalakobana.Game.Application.Services
{
    public class VotingService : IVotingService
    {
        private readonly IVotingRepository _repository;

        public VotingService(IVotingRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<int>> CreatePendingDataAsync(PendingData data)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(data?.TableName) || string.IsNullOrWhiteSpace(data?.Value))
                    return Result<int>.Failure("TableName and Value must not be empty.");

                var insertedId = await _repository.AddPendingDataAsync(data);
                return Result<int>.Success(insertedId);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Error inserting PendingData: {ex.Message}");
            }
        }
    }
}
